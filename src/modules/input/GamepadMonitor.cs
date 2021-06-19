using Microsoft.Xna.Framework.Input;

namespace Ladybug.Input
{
	public class GamepadMonitor : InputMonitor<GamePadState,Buttons>
  {
    public override InputState GetInputState(Buttons b)
    {
      var pK = (previousState != null && previousState.IsButtonDown(b));
      var cK = currentState.IsButtonDown(b);
      bool stateChanged = (cK =! pK);

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