using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Ladybug.UserInput
{
	/// <summary>
	/// Monitors changes in Mouse button state and cursor position
	/// </summary>
	public class MouseMonitor : InputMonitor<MouseState, MouseButtons>
	{
		/// <summary>
		/// Gets the <see cref="InputState"/> of the given mouse button
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public override InputState GetInputState(MouseButtons key)
		{
			ButtonState cs;
			ButtonState ps;

			switch (key)
			{
				case MouseButtons.Right:
					cs = currentState.RightButton;
					ps = previousState.RightButton;
					break;
				case MouseButtons.Left:
					cs = currentState.LeftButton;
					ps = previousState.LeftButton;
					break;
				default:
					cs = ButtonState.Released;
					ps = ButtonState.Released;
					break;
			}

			var pK = (previousState != null && ps == ButtonState.Pressed);
			var cK = cs == ButtonState.Pressed;

			bool stateChanged = (previousState == null) ? false : cK != pK;

			InputState state;

			if (cK)
			{
				state = stateChanged ? InputState.Pressed : InputState.Down;
			}
			else
			{
				state = stateChanged ? InputState.Released : InputState.Up;
			}

			return state;
		}

		/// <summary>
		/// Gets the window-space cursor position
		/// </summary>
		/// <returns></returns>
		public Vector2 GetCursorPosition() => new Vector2(currentState.X, currentState.Y);
	}
}