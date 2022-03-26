using Microsoft.Xna.Framework.Input;

namespace Ladybug.UserInput
{
	/// <summary>
	/// Monitors changes in Keyboard key state
	/// </summary>
	public class KeyboardMonitor : InputMonitor<KeyboardState, Keys>
	{
		/// <summary>
		/// Gets the <see cref="InputState"/> of the given key
		/// </summary>
		/// <param name="k"></param>
		/// <returns></returns>
		public override InputState GetInputState(Keys k)
		{
			bool pK = (previousState != null && previousState.IsKeyDown(k));
			bool cK = currentState.IsKeyDown(k);
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