using Microsoft.Xna.Framework.Input;

namespace Ladybug.UserInput
{
	/// <summary>
	/// Monitors changes in GamePad button state
	/// </summary>
	public class GamePadMonitor : InputMonitor<GamePadState, Buttons>
	{
		/// <summary>
		/// Gets the <see cref="InputState"/> of the given button
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public override InputState GetInputState(Buttons b)
		{
			var pK = (previousState != null && previousState.IsButtonDown(b));
			var cK = currentState.IsButtonDown(b);
			bool stateChanged = previousState == null ? false : (cK != pK);

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
	}
}