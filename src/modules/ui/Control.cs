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
		private List<Control> _children = new List<Control>();
		#endregion // Fields

		/// <summary>
		/// Create a new Control
		/// </summary>
		public Control()
		{
			Children = _children.AsReadOnly();
		}
		
		#region Properties
		/// <summary>
		/// Reference to the managing UI's ResourceCatalog
		/// </summary>
		public ResourceCatalog ResourceCatalog => UI?.ResourceCatalog;

		/// <summary>
		/// Access one of this Control's children by name
		/// </summary>
		/// <value></value>
		public Control this[string name]
		{
			get => _children.Where(c => c.Name == name).FirstOrDefault();
		}

		/// <summary>
		/// Name of this Control
		/// </summary>
		/// <value></value>
		public string Name { get; set; }

		/// <summary>
		/// This Control's parent Control
		/// </summary>
		/// <value></value>
		public Control Parent { get; private set; }

		/// <summary>
		/// This Control's Children
		/// </summary>
		/// <value></value>
		public IList<Control> Children { get; private set; }

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
		protected virtual void Attach(Control parentControl) { }
		internal void _Attach(Control parentControl)
		{
			Parent = parentControl;
			UI = parentControl.UI;
			UI.RegisterControl(this);
			ZIndex = parentControl.ZIndex + 1;
			Attach(parentControl);
			_Initialize();
		}

		/// <summary>
		/// Called when a child control is attached to this Control
		/// </summary>
		/// <param name="childControl"></param>
		protected virtual void AddChild(Control childControl) { }
		internal void _AddChild(Control childControl)
		{
			if (!_children.Contains(childControl))
			{
				_children.Add(childControl);
				AddChild(childControl);
			}
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
		/// Called when this Control is updated each frame
		/// </summary>
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

		/// <summary>
		/// Called when this Control is drawn
		/// </summary>
		/// <param name="spriteBatch"></param>
		protected virtual void Draw(SpriteBatch spriteBatch) { }
		internal void _Draw(SpriteBatch spriteBatch)
		{
			Draw(spriteBatch);
		}
		#endregion // Virtual Lifecycle Methods
	}
}