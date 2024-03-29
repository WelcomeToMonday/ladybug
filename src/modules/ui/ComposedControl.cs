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
		private Action<IControlContainer> _onAttach = (IControlContainer parentControl) => { };
		private Action<InputState> _onClick = (InputState state) => { };
		private Action _onFocus = () => { };
		private Action _onUnfocus = () => { };
		private Action _onCursorEnter = () => { };
		private Action _onCursorLeave = () => { };
		private Action<Rectangle, Rectangle> _onUpdateBounds = (Rectangle oldBounds, Rectangle newBounds) => { };
		private Action<bool> _onToggleVisible = (bool value) => { };
		private Action<bool> _onToggleActive = (bool value) => { };
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
		public ComposedControl OnAttach(Action<IControlContainer> action)
		{
			_onAttach = action;
			return this;
		}
		protected override void Attach(IControlContainer parentControl) => _onAttach(parentControl);

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
		/// Action run when this Control's <see cref="Control.Visible"/> property is updated
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnToggleVisible(Action<bool> action)
		{
			_onToggleVisible = action;
			return this;
		}
		protected override void ToggleVisible(bool value)
		{
			_onToggleVisible(value);
		}

		/// <summary>
		/// Action run when this Control's <see cref="Control.Active"/> property is updated
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedControl OnToggleActive(Action<bool> action)
		{
			_onToggleActive = action;
			return this;
		}
		protected override void ToggleActive(bool value)
		{
			_onToggleActive(value);
		}

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
		public override void Update() => _onUpdate();

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
		public override void Draw(SpriteBatch spriteBatch) => _onDraw(spriteBatch);
	}
}
#pragma warning restore 1591