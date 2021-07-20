using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.UserInput;

namespace Ladybug.Beta.UI
{
	public abstract class Control
	{
		public event EventHandler<InputState> Clicked;
		public event EventHandler Focused;
		public event EventHandler Unfocused;
		public event EventHandler CursorEntered;
		public event EventHandler CursorLeft;

		public ResourceCatalog ResourceCatalog => UI?.ResourceCatalog;

		private int _zIndex = 0;
		private List<Control> _children = new List<Control>();

		/// <summary>
		/// Begin inline composing of a new Control
		/// </summary>
		/// <returns></returns>
		public static ComposedControl Compose() => new ComposedControl();

		public Control this[string name]
		{
			get => _children.Where(c => c.Name == name).FirstOrDefault();
		}

		/// <summary>
		/// Adds a new child Control of type T to this Control
		/// </summary>
		/// <param name="name">Name of the new Control</param>
		/// <typeparam name="T">Type of the new Control</typeparam>
		/// <returns>Reference to the current Control</returns>
		public Control AddControl<T>(string name = null) where T : Control, new() => AddControl<T>(name, out T control);

		/// <summary>
		/// Adds a new child Control of type T to this Control
		/// </summary>
		/// <param name="control">Reference to the new Control</param>
		/// <typeparam name="T">Type of the new Control</typeparam>
		/// <returns>Reference to the current Control</returns>
		public Control AddControl<T>(out T control) where T : Control, new() => AddControl<T>(null, out control);

		/// <summary>
		/// Adds a new child Control of type T to this Control
		/// </summary>
		/// <param name="name">Name of the new Control</param>
		/// <param name="control">Reference to the new Control</param>
		/// <typeparam name="T">Type of the new Control</typeparam>
		/// <returns>Reference to the current Control</returns>
		public Control AddControl<T>(string name, out T control) where T : Control, new()
		{
			control = new T();

			if (name != null && name != string.Empty)
			{
				control.Name = name;
			}
			_AddChild(control);
			control._Attach(this);
			return this;
		}

		/// <summary>
		/// Adds an existing Control as a child of this Control
		/// </summary>
		/// <param name="control">Control to add as a child of this Control</param>
		/// <returns>Reference to the current Control</returns>
		public Control AddControl(Control control)
		{
			_AddChild(control);
			control._Attach(this);
			return this;
		}

		public string Name { get; set; }

		public Control Parent { get; private set; }

		public IList<Control> Children { get; private set; }

		public UI UI { get; internal set; }

		public Scene Scene => UI?.Scene;

		public Game Game => UI?.Scene?.Game;

		public bool BlockCursor { get; set; } = true;

		public bool ContainsCursor { get; private set; }

		public Rectangle Bounds { get; private set; }

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

		protected virtual void Initialize() { }
		internal void _Initialize()
		{
			if (!Initialized)
			{
				Initialize();
				Initialized = true;
			}
		}

		protected virtual void Attach(Control parentControl) { }
		internal void _Attach(Control parentControl)
		{
			Parent = parentControl;
			UI = parentControl.UI;
			ZIndex = parentControl.ZIndex + 1;
			Attach(parentControl);
			_Initialize();
		}

		protected virtual void AddChild(Control childControl) { }
		internal void _AddChild(Control childControl)
		{
			if (!_children.Contains(childControl))
			{
				_children.Add(childControl);
				AddChild(childControl);
			}
		}

		protected virtual void Click(InputState state) { }
		internal void _Click(InputState state)
		{
			Click(state);
			Clicked?.Invoke(this, state);
		}

		protected virtual void Focus() { }
		internal void _Focus()
		{
			Focus();
			Focused?.Invoke(this, new EventArgs());
		}

		protected virtual void Unfocus() { }
		internal void _Unfocus()
		{
			Unfocus();
			Unfocused.Invoke(this, new EventArgs());
		}

		protected virtual void CursorEnter() { }
		internal void _CursorEnter()
		{
			CursorEnter();
			CursorEntered?.Invoke(this, new EventArgs());
		}

		protected virtual void CursorLeave() { }
		internal void _CursorLeave()
		{
			CursorLeave();
			CursorLeft?.Invoke(this, new EventArgs());
		}

		protected virtual void Update() { }
		internal void _Update()
		{
			var _containsCursor = Bounds.Contains(UI.GetCursorPosition());

			if (_containsCursor && !ContainsCursor)
			{
				_CursorEnter();
				ContainsCursor = true;
			}
			else if (!_containsCursor && ContainsCursor)
			{
				_CursorLeave();
				ContainsCursor = false;
			}

			Update();
		}

		protected virtual void Draw(SpriteBatch spriteBatch) { }
		internal void _Draw(SpriteBatch spriteBatch)
		{
			Draw(spriteBatch);
		}
	}
}