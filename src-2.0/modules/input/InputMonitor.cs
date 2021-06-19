using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Ladybug.Input
{
  public abstract class InputMonitor<T,K> : IInputMonitor<T,K>
  {
    protected T previousState;
    protected T currentState;

    public bool UpdateActive {get; protected set;} = false;

    public void BeginUpdate (T newState)
    {
      currentState = newState;
      UpdateActive = true;
    }

    public void EndUpdate()
    {
      UpdateActive = false;
      previousState = currentState;
    }

    public abstract InputState GetInputState(K key);

    public bool CheckButton(K button, InputState state) => GetInputState(button) == state;

    public bool CheckAnyButton(K[] keys, InputState state)
    {
      bool res = false;

      foreach (var key in keys)
      {
        if (CheckButton(key, state)) res = true;
      }

      return res;
    }

    public bool CheckAllButtons(K[] keys, InputState state)
    {
      bool res = true;

      foreach (var key in keys)
      {
        if (!CheckButton(key, state)) res = false;
      }

      return res;
    }
  }
}