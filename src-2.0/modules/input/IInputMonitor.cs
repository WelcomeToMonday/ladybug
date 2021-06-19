namespace Ladybug.Input
{
	public enum InputState{Down, Up, Pressed, Released}

  public interface IInputMonitor<T,K>
  {
    void BeginUpdate(T newState);
    void EndUpdate();
    InputState GetInputState(K key);
    bool CheckButton(K key, InputState state);
    bool CheckAnyButton(K[] keys, InputState state);
    bool CheckAllButtons(K[] keys, InputState state);
  }
}