using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Ladybug.Input;

namespace Ladybug.ECS.Components
{
	[Obsolete("Ladybug.ECS is being deprecated upon 2.0 release. Use Ladybug.Entities instead.", false)]
	public class InputComponent : Component
	{
		private int _gamepadIndex = 0;
		
		public bool MonitorKeyboard {get;set;} = false;
		
		public bool MonitorMouse {get;set;} = false;
		
		public bool MonitorGamepad {get;set;} = false;
		
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

		public override void Initialize()
		{
			KeyboardMonitor = new KeyboardMonitor();
			MouseMonitor = new MouseMonitor();
			GamepadMonitor = new GamepadMonitor();
		}

		public override void PreUpdate(GameTime gameTime)
		{
			if (MonitorKeyboard) KeyboardMonitor.BeginUpdate(Keyboard.GetState());
			if (MonitorMouse) MouseMonitor.BeginUpdate(Mouse.GetState());
			if (MonitorGamepad) GamepadMonitor.BeginUpdate(GamePad.GetState(GamepadIndex));
		}

		public override void PostUpdate(GameTime gameTime)
		{
			if (KeyboardMonitor.UpdateActive) KeyboardMonitor.EndUpdate();
			if (MouseMonitor.UpdateActive) MouseMonitor.EndUpdate();
			if (GamepadMonitor.UpdateActive) GamepadMonitor.EndUpdate();
		}
	}
}