using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Ladybug.Input;

namespace Ladybug.ECS.Components
{
	public class InputComponentSystem : ComponentSystem<InputComponent> { }

	public class InputComponent : Component
	{
		private int _gamepadIndex = 0;

		public InputComponent()
		{
			OnInitialize(Initialize);
			OnUpdate("PreUpdate", PreUpdate);
			OnUpdate("PostUpdate", PostUpdate);
		}

		public bool MonitorKeyboard { get; set; } = true;
		public bool MonitorMouse { get; set; } = true;
		public bool MonitorGamepad { get; set; } = true;

		public int GamepadIndex
		{
			get => _gamepadIndex;
			set
			{
				if (value >= 0)
				{
					_gamepadIndex = value;
				}
			}
		}

		public GamepadMonitor GamepadMonitor { get; private set; }

		public MouseMonitor MouseMonitor { get; private set; }

		public KeyboardMonitor KeyboardMonitor { get; private set; }

		private void Initialize()
		{
			KeyboardMonitor = new KeyboardMonitor();
			MouseMonitor = new MouseMonitor();
			GamepadMonitor = new GamepadMonitor();
		}

		private void PreUpdate(GameTime gameTime)
		{
			if (MonitorKeyboard) KeyboardMonitor.BeginUpdate(Keyboard.GetState());
			if (MonitorMouse) MouseMonitor.BeginUpdate(Mouse.GetState());
			if (MonitorGamepad) GamepadMonitor.BeginUpdate(GamePad.GetState(GamepadIndex));
		}

		private void PostUpdate(GameTime gameTime)
		{
			if (KeyboardMonitor.UpdateActive) KeyboardMonitor.EndUpdate();
			if (MouseMonitor.UpdateActive) MouseMonitor.EndUpdate();
			if (GamepadMonitor.UpdateActive) GamepadMonitor.EndUpdate();
		}
	}
}