using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.UserInput;

namespace Ladybug.Beta.UI
{
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
		private Action _onUpdate = () => { };
		private Action<SpriteBatch> _onDraw = (SpriteBatch spriteBatch) => { };

		internal ComposedControl() { }

		public ComposedControl OnInitialize(Action action)
		{
			_onInitialize = action;
			return this;
		}
		protected override void Initialize() => _onInitialize();

		public ComposedControl OnAttach(Action<Control> action)
		{
			_onAttach = action;
			return this;
		}
		protected override void Attach(Control parentControl) => _onAttach(parentControl);

		public ComposedControl OnAddChild(Action<Control> action)
		{
			_onAddChild = action;
			return this;
		}
		protected override void AddChild(Control childControl) => _onAddChild(childControl);

		public ComposedControl OnClick(Action<InputState> action)
		{
			_onClick = action;
			return this;
		}
		protected override void Click(InputState state) => _onClick(state);

		public ComposedControl OnFocus(Action action)
		{
			_onFocus = action;
			return this;
		}
		protected override void Focus() => _onFocus();

		public ComposedControl OnUnfocus(Action action)
		{
			_onUnfocus = action;
			return this;
		}
		protected override void Unfocus() => _onUnfocus();

		public ComposedControl OnCursorEnter(Action action)
		{
			_onCursorEnter = action;
			return this;
		}
		protected override void CursorEnter() => _onCursorEnter();

		public ComposedControl OnCursorLeave(Action action)
		{
			_onCursorLeave = action;
			return this;
		}
		protected override void CursorLeave() => _onCursorLeave();

		public ComposedControl OnUpdate(Action action)
		{
			_onUpdate = action;
			return this;
		}
		protected override void Update() => _onUpdate();

		public ComposedControl OnDraw(Action<SpriteBatch> action)
		{
			_onDraw = action;
			return this;
		}
		protected override void Draw(SpriteBatch spriteBatch) => _onDraw(spriteBatch);
	}
}