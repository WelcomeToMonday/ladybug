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
		public event EventHandler Click;
		public event EventHandler Focus;
		public event EventHandler Unfocus;

		private int _zIndex = 0;
		private List<Control> _children = new List<Control>();

		private Action _onInitialize = () => { };
		private Action<Control> _onAttach = (Control parentControl) => { };
		private Action<Control> _onAddChild = (Control childControl) => { };
		private Action<InputState> _onClick = (InputState state) => { };
		private Action _onFocus = () => { };
		private Action _onUnfocus = () => { };
		private Action _onCursorEnter = () => { };
		private Action _onCursorLeave = () => { };
		private Action _onUpdate = () => { };
		private Action<SpriteBatch> _onDraw = (SpriteBatch spriteBatch) => { };

		internal Control() { }

		public Control this[string name]
		{
			get => _children.Where(c => c.Name == name).FirstOrDefault();
		}

		public Control AddControl<T>(string name = null) where T : Control, new() => AddControl<T>(name, out T control);

		public Control AddControl<T>(out T control) where T : Control, new() => AddControl<T>(null, out control);

		public Control AddControl<T>(string name, out T control) where T : Control, new()
		{
			control = new T();

			if (name != null && name != string.Empty)
			{
				control.Name = name;
			}
			_OnAddChild(control);
			control._OnAttach(this);
			return this;
		}

		public string Name { get; set; }

		public Control Parent { get; private set; }

		public IList<Control> Children { get; private set; }

		public UI UI { get; private set; }

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

		/// <summary>
		/// Sets the action performed when this control is initialized
		/// </summary>
		/// <param name="action"></param>
		public Control OnInitialize(Action action)
		{
			_onInitialize = action;
			return this;
		}
		internal void _OnInitialize()
		{
			if (!Initialized)
			{
				_onInitialize();
				Initialized = true;
			}
		}

		/// <summary>
		/// Sets the action performed when this control is added as a child of another control
		/// </summary>
		/// <param name="action"></param>
		public Control OnAttach(Action<Control> action)
		{
			_onAttach = action;
			return this;
		}
		internal void _OnAttach(Control parentControl)
		{
			Parent = parentControl;
			UI = parentControl.UI;
			ZIndex = parentControl.ZIndex + 1;
			_onAttach(parentControl);
			_OnInitialize();
		}

		public Control OnAddChild(Action<Control> action)
		{
			_onAddChild = action;
			return this;
		}
		internal void _OnAddChild(Control childControl)
		{
			if (!_children.Contains(childControl))
			{
				_children.Add(childControl);
				_onAddChild(childControl);
			}
		}

		public Control OnClick(Action<InputState> action)
		{
			_onClick = action;
			return this;
		}
		internal void _OnClick(InputState state)
		{
			_OnClick(state);
		}

		public Control OnFocus(Action action)
		{
			_onFocus = action;
			return this;
		}
		internal void _OnFocus()
		{
			_onFocus();
			Focus?.Invoke(this, new EventArgs());
		}

		public Control OnUnfocus(Action action)
		{
			_onUnfocus = action;
			return this;
		}
		internal void _OnUnfocus()
		{
			_onUnfocus();
			Unfocus.Invoke(this, new EventArgs());
		}

		public Control OnCursorEnter(Action action)
		{
			_onCursorEnter = action;
			return this;
		}
		internal void _OnCursorEnter()
		{
			_onCursorEnter();
		}

		public Control OnCursorLeave(Action action)
		{
			_onCursorLeave = action;
			return this;
		}
		internal void _OnCursorLeave()
		{
			_onCursorLeave();
		}

		/// <summary>
		/// Sets the action performed when this control is updated
		/// </summary>
		/// <param name="action"></param>
		public Control OnUpdate(Action action)
		{
			_onUpdate = action;
			return this;
		}
		internal void _OnUpdate()
		{
			var _containsCursor = Bounds.Contains(UI.GetCursorPosition());

			if (_containsCursor && !ContainsCursor)
			{
				_OnCursorEnter();
				ContainsCursor = true;
			}
			else if (!_containsCursor && ContainsCursor)
			{
				_OnCursorLeave();
				ContainsCursor = false;
			}

			_onUpdate();
		}

		/// <summary>
		///  Sets the action performed when this control is drawn
		/// </summary>
		/// <param name="action"></param>
		public Control OnDraw(Action<SpriteBatch> action)
		{
			_onDraw = action;
			return this;
		}
		internal void _OnDraw(SpriteBatch spriteBatch)
		{
			_onDraw(spriteBatch);
		}
	}
}