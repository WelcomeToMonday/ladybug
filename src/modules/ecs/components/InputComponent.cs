using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Ladybug.Input;

namespace Ladybug.ECS.Components
{
	/// <summary>
	/// ComponentSystem responsible for processing <see cref="Ladybug.ECS.Components.InputComponent"/> Components
	/// </summary>
	public class InputComponentSystem : ComponentSystem<InputComponent> { }

	/// <summary>
	/// Ladybug standard component for input handling
	/// </summary>
	public class InputComponent : Component
	{
		private int _gamepadIndex = 0;

		/// <summary>
		/// Creates a new InputComponent instance
		/// </summary>
		public InputComponent()
		{
			OnInitialize(Initialize);
			OnUpdate("PreUpdate", PreUpdate);
			OnUpdate("PostUpdate", PostUpdate);
		}

		/// <summary>
		/// Whether to monitor the keyboard for input
		/// </summary>
		/// <value></value>
		public bool MonitorKeyboard { get; set; } = true;

		/// <summary>
		/// Whether to monitor the mouse for input
		/// </summary>
		/// <value></value>
		public bool MonitorMouse { get; set; } = true;

		/// <summary>
		/// Whether to monitor a gamepad for input
		/// </summary>
		/// <value></value>
		public bool MonitorGamepad { get; set; } = true;

		/// <summary>
		/// Index of the gamepad to monitor for input
		/// </summary>
		/// <value></value>
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

		/// <summary>
		/// The <see cref="Ladybug.Input.GamepadMonitor"/> instance used by this component
		/// to monitor gamepad input
		/// </summary>
		/// <value></value>
		public GamepadMonitor GamepadMonitor { get; private set; }

		/// <summary>
		/// The <see cref="Ladybug.Input.MouseMonitor"/> instance used by this component
		/// to monitor mouse input
		/// </summary>
		/// <value></value>
		public MouseMonitor MouseMonitor { get; private set; }

		/// <summary>
		/// The <see cref="Ladybug.Input.KeyboardMonitor"/> instance used by this component
		/// to monitor keyboard input
		/// </summary>
		/// <value></value>
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