using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Core.UI
{
	/// <summary>
	/// Base Class for all Menu Controls
	/// </summary>
	public abstract class Control
	{
		#region Constants

		private const int CHILD_CONTROL_COUNT_DEFAULT = 10;

		#endregion

		#region Enums

		#endregion

		#region Events

		public event EventHandler Focus;
		public event EventHandler UnFocus;

		public event EventHandler Enabled;
		public event EventHandler Disabled;

		public event EventHandler<ControlMoveEvent> PositionChanged;
		public event EventHandler SizeChanged;

		public event EventHandler Click;
		public event EventHandler ClickStart;
		public event EventHandler ClickHold;
		public event EventHandler ClickEnd;
		public event EventHandler ClickOut;

		public event EventHandler CursorEnter;
		public event EventHandler CursorLeave;

		#endregion

		#region Member Variables

		private bool _containsCursor = false;

		#endregion

		#region Constructors
		/*
		public Control()
		{

		}
		*/
		public Control(Control parentControl = null, string name = "")
		{
			Name = name;
			if (parentControl != null)
			{
				parentControl.AddControl(this);
			}
			Initialize();
		}

		#endregion

		#region Properties

		public Control this[string name] { get => FindControl(name); }

		public UI UI { get; set; }

		public Rectangle Bounds { get; set; }

		public Vector2 LocalPosition { get => Bounds.Location.ToVector2() - Parent.Bounds.Location.ToVector2(); }

		public Vector2 GlobalPosition { get => Bounds.Location.ToVector2(); }

		public Control Parent { get; set; }

		public string Name { get; set; }

		public int ID { get; set; }

		public List<Control> Controls
		{
			get
			{
				if (_controls == null)
				{
					_controls = new List<Control>(CHILD_CONTROL_COUNT_DEFAULT);
				}
				return _controls;
			}
			set => _controls = value;
		}
		private List<Control> _controls;

		public Dictionary<string, string> Attributes
		{
			get
			{
				if (_attributes == null)
				{
					_attributes = new Dictionary<string, string>();
				}
				return _attributes;
			}
			set => _attributes = value;
		}
		private Dictionary<string, string> _attributes;

		public bool IsEnabled { get; protected set; } = true;

		public bool Visible { get; set; } = true;

		public bool HasFocus
		{
			get
			{
				var res = false;
				if (UI != null)
				{
					res = UI.FocusedControl == this;
				}
				return res;
			}
		}

		public Texture2D BackgroundImage { get; set; }

		public SpriteFont Font { get; protected set; }

		#endregion

		#region Methods

		public virtual void Initialize()
		{
			if (UI != null)
			{
				UI.FocusChange += OnUIFocusChange;
				UI.ClickStart += OnUIClickStart;
				UI.ClickHold += OnUIClickHold;
				UI.ClickEnd += OnUIClickEnd;
			}
		}

		protected virtual void OnUIFocusChange(object sender, UIControlChangeEvent e)
		{
			if (e.NewControl == this)
			{
				Focus?.Invoke(this, new EventArgs());
			}

			if (e.PreviousControl == this)
			{
				UnFocus?.Invoke(this, new EventArgs());
			}
		}

		public virtual void OnUIClickStart(object sender, UIClickEvent e)
		{
			if (IsEnabled && Bounds.Contains(e.CursorPosition))
			{
				ClickStart?.Invoke(this, new EventArgs());
			}
		}

		public virtual void OnUIClickHold(object sender, UIClickEvent e)
		{
			if (IsEnabled && Bounds.Contains(e.CursorPosition))
			{
				ClickHold?.Invoke(this, new EventArgs());
			}
		}

		public virtual void OnUIClickEnd(object sender, UIClickEvent e)
		{
			if (IsEnabled && Bounds.Contains(e.CursorPosition))
			{
				ClickEnd?.Invoke(this, new EventArgs());
				Click?.Invoke(this, new EventArgs());
			}
			else
			{
				ClickOut?.Invoke(this, new EventArgs());
			}
		}

		public virtual void Enable()
		{
			if (!IsEnabled)
			{
				IsEnabled = true;
				OnEnable();
			}
		}

		protected void OnEnable()
		{
			Enabled?.Invoke(this, new EventArgs());
		}

		public virtual void Disable()
		{
			if (IsEnabled)
			{
				IsEnabled = false;
				OnDisable();
			}
		}

		protected void OnDisable()
		{
			Disabled?.Invoke(this, new EventArgs());
		}

		public virtual void Update()
		{
			if (IsEnabled)
			{
				if (Bounds.Contains(UI.CursorPosition))
				{
					if (!_containsCursor)
					{
						_containsCursor = true;
						CursorEnter?.Invoke(this, new EventArgs());
					}
				}
				else
				{
					if (_containsCursor)
					{
						_containsCursor = false;
						CursorLeave?.Invoke(this, new EventArgs());
					}
				}
				foreach (var c in Controls)
				{
					c.Update();
				}
			}
			else if (!IsEnabled && _containsCursor)
			{
				_containsCursor = false;
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			foreach (var c in Controls)
			{
				if (c.Visible)
				{
					c.Draw(spriteBatch);
				}
			}
		}

		public void SetBounds(Rectangle newBounds)
		{
			//Vector2 pos = PositionMode == PositionMode.ABSOLUTE ? newBounds.Location.ToVector2() : Parent.Bounds.Location.ToVector2() + newBounds.Location.ToVector2();
			var oldBounds = Bounds;
			Vector2 pos = newBounds.Location.ToVector2();
			Bounds = new Rectangle((int)pos.X, (int)pos.Y, newBounds.Width, newBounds.Height);
			PositionChanged?.Invoke(this, new ControlMoveEvent(oldBounds.Location.ToVector2(), newBounds.Location.ToVector2()));
			SizeChanged?.Invoke(this, new EventArgs());
		}

		public void SetBounds(int x, int y, int width, int height) => SetBounds(new Rectangle(x, y, width, height));

		public virtual void SetFont(SpriteFont font) => Font = font;

		public void Move(int x, int y) => Move(new Vector2(x, y));

		public void Move(Vector2 newPos)
		{
			var oldBounds = Bounds;
			var newBounds = new Rectangle
			(
				(int)Bounds.Location.X + (int)newPos.X,
				(int)Bounds.Location.Y + (int)newPos.Y,
				Bounds.Width,
				Bounds.Height
			);
			SetBounds(newBounds);
			PositionChanged?.Invoke(this, new ControlMoveEvent(oldBounds.Location.ToVector2(), newBounds.Location.ToVector2()));
		}

		public void SetPosition(Vector2 newPos)
		{
			Rectangle newBounds = new Rectangle((int)newPos.X, (int)newPos.Y, Bounds.Width, Bounds.Height);
			SetBounds(newBounds);
		}

		public void AddControl(Control newControl)
		{
			if (!Controls.Contains(newControl))
			{
				newControl.Parent = this;
				newControl.UI = UI;

				newControl.Font = UI?.DefaultFont;

				if (UI?.DefaultBackground != null) // keep an eye on this
				{
					newControl.BackgroundImage = UI.DefaultBackground;
				}

				Controls.Add(newControl);
			}
		}

		public Control FindControl(string name, bool recurse = false)
		{
			return FindControl<Control>(name, recurse);
		}

		public T FindControl<T>(string name = null, bool recurse = false, bool strictTypeMatch = false) where T : Control
		{
			var res = strictTypeMatch
			? Controls.OfType<T>().Where(control => (name == null ? true : control.Name == name) && control.GetType() == typeof(T)).FirstOrDefault()
			: Controls.OfType<T>().Where(control => (name == null ? true : control.Name == name)).FirstOrDefault();

			if (recurse && res == null)
			{
				foreach (var c in Controls)
				{
					var subRes = c.FindControl<T>(name, recurse, strictTypeMatch);
					if (subRes != null)
					{
						res = subRes;
						break;
					}
				}
			}

			return res;
		}

		public List<T> FindControls<T>(bool strictTypeMatch = false, bool recurse = true) where T : Control
		{
			var res = strictTypeMatch
			? Controls.OfType<T>().Where(control => control.GetType() == typeof(T)).ToList()
			: Controls.OfType<T>().ToList();

			if (recurse)
			{
				Controls.ToList().ForEach(
					control =>
					{
						var subRes = control.FindControls<T>(strictTypeMatch, recurse);
						subRes.ForEach(item => res.Add(item));
					}
				);
			}

			return res;
		}
		#endregion
	}
}