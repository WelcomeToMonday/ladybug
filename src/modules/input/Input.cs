using System;
using System.Collections.Generic;

using MGInput = Microsoft.Xna.Framework.Input;

namespace Ladybug.UserInput
{
	/// <summary>
	/// Static input monitor class
	/// </summary>
	public static class Input
	{
		/// <summary>
		/// Default number of GamePads
		/// </summary>
		public const int DEFAULT_GAMEPAD_COUNT = 4;

		/// <summary>
		/// A registered input action has been detected
		/// </summary>
		public static event EventHandler<InputActionEventArgs> Action;

		/// <summary>
		/// Global <see cref="KeyboardMonitor"/>
		/// </summary>
		public static readonly KeyboardMonitor Keyboard;

		/// <summary>
		/// Global <see cref="MouseMonitor"/>
		/// </summary>
		public static readonly MouseMonitor Mouse;

		private static Dictionary<string, MGInput.Keys[]> _keyboardActions = new Dictionary<string, MGInput.Keys[]>();
		private static Dictionary<string, MGInput.Buttons[]> _gamePadActions = new Dictionary<string, MGInput.Buttons[]>();

		static Input()
		{
			Keyboard = new KeyboardMonitor();
			Mouse = new MouseMonitor();
			SetGamepadCount(DEFAULT_GAMEPAD_COUNT);
		}

		/// <summary>
		/// Global list of <see cref="GamePadMonitor"/>s
		/// </summary>
		/// <value></value>
		public static GamePadMonitor[] GamePads { get; private set; }

		/// <summary>
		/// Sets the available number of GamePads
		/// </summary>
		/// <param name="count"></param>
		public static void SetGamepadCount(int count)
		{
			if (count < 0 || count > MGInput.GamePad.MaximumGamePadCount)
			{
				throw new InvalidOperationException($"Invalid Gamepad count value specified ({count}). Must be between 0 and {MGInput.GamePad.MaximumGamePadCount}");
			}

			GamePads = new GamePadMonitor[count];
			for (var i = 0; i < count; i++)
			{
				GamePads[i] = new GamePadMonitor();
			}
		}

		/// <summary>
		/// Creates or sets a new input action bound to one or more keys
		/// </summary>
		/// <param name="name">Name of the action</param>
		/// <param name="keys">Keys bound to the action</param>
		public static void SetAction(string name, params MGInput.Keys[] keys)
		{
			if (!_keyboardActions.TryAdd(name, keys))
			{
				_keyboardActions[name] = keys;
			}
		}

		/// <summary>
		/// Creates or sets a new input action bound to one or more buttons
		/// </summary>
		/// <param name="name">Name of the action</param>
		/// <param name="buttons">Buttons bound to the action</param>
		public static void SetAction(string name, params MGInput.Buttons[] buttons)
		{
			if (!_gamePadActions.TryAdd(name, buttons))
			{
				_gamePadActions[name] = buttons;
			}
		}

		internal static void Begin()
		{
			Keyboard.BeginUpdate(MGInput.Keyboard.GetState());
			Mouse.BeginUpdate(MGInput.Mouse.GetState());
			for (var i = 0; i < GamePads.Length; i++)
			{
				GamePads[i].BeginUpdate(MGInput.GamePad.GetState(i));
			}
			Update();
		}

		private static void Update()
		{
			foreach (var a in _keyboardActions)
			{
				var action = a.Key;
				var keys = a.Value;
				foreach (var k in keys)
				{
					var state = Keyboard.GetInputState(k);

					if (state != InputState.Up)
					{
						Action?.Invoke(Keyboard, new InputActionEventArgs(action, InputSource.Keyboard, state));
					}
				}
			}

			for (var i = 0; i < GamePads.Length; i++)
			{
				var gp = GamePads[i];
				foreach (var a in _gamePadActions)
				{
					var action = a.Key;
					var buttons = a.Value;
					foreach (var b in buttons)
					{
						var state = gp.GetInputState(b);

						if (state != InputState.Up)
						{
							Action?.Invoke(gp, new InputActionEventArgs(action, InputSource.GamePad, state, i));
						}
					}
				}
			}
		}

		internal static void End()
		{
			Keyboard.EndUpdate();
			Mouse.EndUpdate();
			for (var i = 0; i < GamePads.Length; i++)
			{
				GamePads[i].EndUpdate();
			}
		}
	}
}