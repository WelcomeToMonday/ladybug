using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Ladybug.Input;
using Ladybug.ECS;

namespace Ladybug.Core.Components
{
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

		public override void WriteXml(System.Xml.XmlWriter writer)
		{
			writer.WriteStartElement(ToString());
			
			writer.WriteAttributeString("mouse", $"{MonitorMouse}");
			writer.WriteAttributeString("keyboard", $"{MonitorKeyboard}");
			writer.WriteAttributeString("gamepad", $"{MonitorGamepad}");
			writer.WriteAttributeString("index", $"{GamepadIndex}");

			writer.WriteEndElement();
		}

		public override void ReadXml(System.Xml.XmlReader reader)
		{
			bool mouse = false;
			bool keyboard = false;
			bool gamepad = false;
			int index = 0;

			while (reader.Read())
			{
				reader.MoveToAttribute("mouse");
				mouse = reader.Value.ToBool();

				reader.MoveToAttribute("keyboard");
				keyboard = reader.Value.ToBool();

				reader.MoveToAttribute("gamepad");
				gamepad = reader.Value.ToBool();

				reader.MoveToAttribute("index");
				int.TryParse(reader.Value, out index);
			}

			MonitorMouse = mouse;
			MonitorKeyboard = keyboard;
			MonitorGamepad = gamepad;
			GamepadIndex = index;

		}
	}
}