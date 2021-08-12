#pragma warning disable 1591 // Hide XMLdoc warnings.
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.UserInput;

namespace Ladybug.UI
{
	/// <summary>
	/// A Comoposable Control
	/// </summary>
	public sealed class ComposedControl : Control
	{
		private Action _onInitialize = () => { };
		private Action<Control> _onAttach = (Control parentControl) => { };
		private Action<Control> _onAddChild = (Control childControl) => { };
		private Action<InputState> _onClick = (InputState state) => { };
		private Action _onFocus = () => { };
		private Action _onUnfocus = () => { };
		private Action _onCursorEnter = () => { };
		private Action _onCursorLeave = () => { };
		private Action<Rectangle, Rectangle> _onUpdateBounds = (Rectangle oldBounds, Rectangle newBounds) => { };
		private Action _onUpdate = () => { };
		private Action<SpriteBatch> _onDraw = (SpriteBatch spriteBatch) => { };

		internal ComposedControl() { }

		/// <summary>
		/// Sets the action run when this Control is initialized
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnInitialize(Action action)
		{
			_onInitialize = action;
			return this;
		}
		protected override void Initialize() => _onInitialize();

		/// <summary>
		/// Sets the action run when this Control is attached to a parent control
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnAttach(Action<Control> action)
		{
			_onAttach = action;
			return this;
		}
		protected override void Attach(Control parentControl) => _onAttach(parentControl);

		/// <summary>
		/// Action run when a child control is attached to this Control
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnAddChild(Action<Control> action)
		{
			_onAddChild = action;
			return this;
		}
		protected override void AddChild(Control childControl) => _onAddChild(childControl);

		/// <summary>
		/// Action run when this Control is clicked
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnClick(Action<InputState> action)
		{
			_onClick = action;
			return this;
		}
		protected override void Click(InputState state) => _onClick(state);

		/// <summary>
		/// Action run when this Control gains focus
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnFocus(Action action)
		{
			_onFocus = action;
			return this;
		}
		protected override void Focus() => _onFocus();

		/// <summary>
		/// Action run when this Control loses focus
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnUnfocus(Action action)
		{
			_onUnfocus = action;
			return this;
		}
		protected override void Unfocus() => _onUnfocus();

		/// <summary>
		/// Action run when the cursor enters this Control's bounds
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnCursorEnter(Action action)
		{
			_onCursorEnter = action;
			return this;
		}
		protected override void CursorEnter() => _onCursorEnter();

		/// <summary>
		/// Action run when the cursor leaves this Control's bounds
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnCursorLeave(Action action)
		{
			_onCursorLeave = action;
			return this;
		}
		protected override void CursorLeave() => _onCursorLeave();

		/// <summary>
		/// Action run when this Control's bounds are updated
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnUpdateBounds(Action<Rectangle, Rectangle> action)
		{
			_onUpdateBounds = action;
			return this;
		}
		protected override void UpdateBounds(Rectangle oldBounds, Rectangle newBounds) => _onUpdateBounds(oldBounds, newBounds);

		/// <summary>
		/// Action run when this Control is updated each frame
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnUpdate(Action action)
		{
			_onUpdate = action;
			return this;
		}
		protected override void Update() => _onUpdate();

		/// <summary>
		/// Action run when this Control is drawn
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnDraw(Action<SpriteBatch> action)
		{
			_onDraw = action;
			return this;
		}
		protected override void Draw(SpriteBatch spriteBatch) => _onDraw(spriteBatch);
	}
}
#pragma warning restore 1591