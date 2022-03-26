using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.UserInput;

namespace Ladybug.UI
{
	/// <summary>
	/// Base Ladybug UI Control class
	/// </summary>
	public abstract class Control
	{
		#region Events
		/// <summary>
		/// The Control has been clicked
		/// </summary>
		public event EventHandler<InputState> Clicked;

		/// <summary>
		/// The control is in focus, but a space it does 
		/// not occupy has been clicked
		/// </summary>
		public event EventHandler ClickedOut;

		/// <summary>
		/// The Control has gained focus
		/// </summary>
		public event EventHandler Focused;

		/// <summary>
		/// The Control has lost focus
		/// </summary>
		public event EventHandler Unfocused;

		/// <summary>
		/// The cursor has entered the control's bounds
		/// </summary>
		public event EventHandler CursorEntered;

		/// <summary>
		/// The cursor has left the control's bounds
		/// </summary>
		public event EventHandler CursorLeft;
		#endregion // Events

		#region Fields
		private int _zIndex = 0;
		#endregion // Fields

		/// <summary>
		/// Create a new Control
		/// </summary>
		public Control()
		{
			//	Children = _children.AsReadOnly();
		}

		#region Properties
		/// <summary>
		/// Reference to the managing UI's ResourceCatalog
		/// </summary>
		public ResourceCatalog ResourceCatalog => UI?.ResourceCatalog;

		/// <summary>
		/// Name of this Control
		/// </summary>
		/// <value></value>
		public string Name { get; set; }

		/// <summary>
		/// This Control's parent Control
		/// </summary>
		/// <value></value>
		public IControlContainer Parent { get; private set; }

		/// <summary>
		/// This Control's managing UI
		/// </summary>
		/// <value></value>
		public UI UI { get; internal set; }

		/// <summary>
		/// This Controls' managing scene
		/// </summary>
		public Scene Scene => UI?.Scene;

		/// <summary>
		/// This control's managing game
		/// </summary>
		public Game Game => UI?.Scene?.Game;

		/// <summary>
		/// Whether the Control currently has focus
		/// </summary>
		public bool HasFocus => UI?.FocusedControl == this;

		/// <summary>
		/// Whether the Control is currently active
		/// </summary>
		/// <value></value>
		public bool Active
		{
			get => m_Active;
			set
			{
				if (m_Active != value)
				{
					m_Active = value;
					_ToggleActive(value);
				}
			}
		}
		private bool m_Active = true;

		/// <summary>
		/// Whether the Control is currently visible
		/// </summary>
		/// <value></value>
		public bool Visible
		{
			get => m_Visible;
			set
			{
				if (m_Visible != value)
				{
					m_Visible = value;
					_ToggleVisible(value);
				}
			}
		}
		private bool m_Visible = true;

		/// <summary>
		/// Whether to allow the cursor to target controls beneath this Control
		/// </summary>
		/// <value></value>
		public bool BlockCursor { get; set; } = true;

		/// <summary>
		/// Whether the cursor is within this Control's bounds
		/// </summary>
		/// <value></value>
		public bool ContainsCursor { get; private set; }

		/// <summary>
		/// The cursor's position, width, and height
		/// </summary>
		/// <value></value>
		public Rectangle Bounds { get; private set; }

		/// <summary>
		/// The draw depth of this Control
		/// </summary>
		/// <value></value>
		public int ZIndex
		{
			get => _zIndex;
			set
			{
				if (value != _zIndex)
				{
					_zIndex = value;
					UI.RequestSort();
				}
			}
		}

		/// <summary>
		/// Whether the control has been initialized
		/// </summary>
		/// <value></value>
		public bool Initialized { get; private set; } = false;
		#endregion // Properties

		#region Standard Methods
		/// <summary>
		/// Begin inline composing of a new Control
		/// </summary>
		/// <returns></returns>
		public static ComposedControl Compose() => new ComposedControl();

		/// <summary>
		/// Sets the Control's Bounds
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void SetBounds(int x, int y, int width, int height) => SetBounds(new Rectangle(x, y, width, height));

		/// <summary>
		/// Sets the Control's Bounds
		/// </summary>
		/// <param name="newBounds"></param>
		public void SetBounds(Rectangle newBounds)
		{
			var oldBounds = Bounds;
			Bounds = newBounds;
			_UpdateBounds(oldBounds, newBounds);
		}

		/// <summary>
		/// Sets the Control's Position
		/// </summary>
		/// <param name="newPos"></param>
		public void SetPosition(Vector2 newPos) => SetPosition((int)newPos.X, (int)newPos.Y);

		/// <summary>
		/// Sets the Control's Position
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void SetPosition(int x, int y) => SetBounds(Bounds.CopyAtPosition(x, y));

		/// <summary>
		/// Moves the Control relative to its current position
		/// </summary>
		/// <param name="newPos"></param>
		public void Move(Vector2 newPos) => Move((int)newPos.X, (int)newPos.Y);

		/// <summary>
		/// Moves the Control relative to its current position
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Move(int x, int y) => SetBounds(Bounds.CopyAtOffset(x, y));

		#endregion // Standard Methods

		#region Virtual Lifecycle Methods
		/// <summary>
		/// Called when the Control is Initialized
		/// </summary>
		protected virtual void Initialize() { }
		internal void _Initialize()
		{
			if (!Initialized)
			{
				Initialize();
				Initialized = true;
			}
		}

		/// <summary>
		/// Called when the Control is attached to a parent control
		/// </summary>
		/// <param name="parentControl"></param>
		protected virtual void Attach(IControlContainer parentControl) { }
		internal void _Attach(IControlContainer parentControl)
		{
			Parent = parentControl;
			UI = parentControl.UI;
			ZIndex = parentControl.ZIndex + 1;
			UI.RegisterControl(this);
			Attach(parentControl);
			_Initialize();
		}

		/// <summary>
		/// Called when this Control is clicked
		/// </summary>
		/// <param name="state"></param>
		protected virtual void Click(InputState state) { }
		internal void _Click(InputState state)
		{
			Click(state);
			Clicked?.Invoke(this, state);
		}

		/// <summary>
		/// Called when this Control is in focus,
		/// but a space outside its bounds has been clicked
		/// </summary>
		protected virtual void ClickOut() { }
		internal void _ClickOut()
		{
			ClickOut();
			ClickedOut?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Called when this Control gains focus
		/// </summary>
		protected virtual void Focus() { }
		internal void _Focus()
		{
			Focus();
			Focused?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Called when this Control loses focus
		/// </summary>
		protected virtual void Unfocus() { }
		internal void _Unfocus()
		{
			Unfocus();
			Unfocused?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Called when the cursor enters this Control's bounds
		/// </summary>
		protected virtual void CursorEnter() { }
		internal void _CursorEnter()
		{
			CursorEnter();
			CursorEntered?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Called when the cursor leaves this Control's bounds
		/// </summary>
		protected virtual void CursorLeave() { }
		internal void _CursorLeave()
		{
			CursorLeave();
			CursorLeft?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Called when this Control's bounds are updated
		/// </summary>
		/// <param name="oldBounds"></param>
		/// <param name="newBounds"></param>
		protected virtual void UpdateBounds(Rectangle oldBounds, Rectangle newBounds) { }
		internal void _UpdateBounds(Rectangle oldBounds, Rectangle newBounds)
		{
			UpdateBounds(oldBounds, newBounds);
		}

		/// <summary>
		/// Called when this Control's <see cref="Visible"/> property is changed
		/// </summary>
		/// <param name="value"></param>
		protected virtual void ToggleVisible(bool value) { }
		internal void _ToggleVisible(bool value)
		{
			ToggleVisible(value);
		}

		/// <summary>
		/// Called when this Control's <see cref="Active"/> property is changed
		/// </summary>
		/// <param name="value"></param>
		protected virtual void ToggleActive(bool value) { }
		internal void _ToggleActive(bool value)
		{
			ToggleActive(value);
		}

		/// <summary>
		/// Called when this Control is updated each frame
		/// </summary>
		public virtual void Update()
		{
			var _containsCursor = Bounds.Contains(UI.GetCursorPosition());

			if (_containsCursor && !ContainsCursor)
			{
				_CursorEnter();
				ContainsCursor = true;
			}
			else if (
				(!_containsCursor && ContainsCursor) ||  // used to contain cursor
				(ContainsCursor && (!Active || !Visible))// contains cursor, but was set not visible/active
				)
			{
				_CursorLeave();
				ContainsCursor = false;
			}
		}

		/// <summary>
		/// Called when this Control is drawn
		/// </summary>
		/// <param name="spriteBatch"></param>
		public virtual void Draw(SpriteBatch spriteBatch) { }
		#endregion // Virtual Lifecycle Methods
	}
}