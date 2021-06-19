using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Ladybug.Input
{
	public enum MouseButtons {RightClick, LeftClick}

  public class MouseMonitor : InputMonitor<MouseState,MouseButtons>
  {
    public override InputState GetInputState(MouseButtons key)
    {
      ButtonState cs;
      ButtonState ps;

      switch (key)
      {
        case MouseButtons.RightClick:
          cs = currentState.RightButton;
          ps = previousState.RightButton;
        break;
        case MouseButtons.LeftClick:
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
    
    public Vector2 GetCursorPosition() => new Vector2(currentState.X, currentState.Y);

    public bool CheckMouseOver(Rectangle r)
    {
      var pos = GetCursorPosition();

      bool check = 
      (
        pos.X >= r.Left &&
        pos.X <= r.Right &&
        pos.Y >= r.Top &&
        pos.Y <= r.Bottom
      );

      return check;
    }
  }
}