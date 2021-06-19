using Microsoft.Xna.Framework.Input;

namespace Ladybug.Input
{
	public class KeyboardMonitor : InputMonitor<KeyboardState,Keys>
  {
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