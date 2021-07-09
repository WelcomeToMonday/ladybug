namespace Ladybug.UserInput
{
	/// <summary>
	/// Represents variou states a key can be in at any given time
	/// </summary>
	public enum InputState
	{
		/// <summary>
		/// The key is pressed, and may or may not have been pressed the previous frame
		/// </summary>
		Down,
		/// <summary>
		/// They key is not pressed, and may or may not have been pressed the previous frame
		/// </summary>
		Up,
		/// <summary>
		/// The key is pressed, and was not pressed the previous frame
		/// </summary>
		Pressed,
		/// <summary>
		/// The key is not pressed, and was pressed the previous frame
		/// </summary>
		Released
	}

	/// <summary>
	/// Represents an Input Source
	/// </summary>
	public enum InputSource
	{
		/// <summary>
		/// Keyboard input source
		/// </summary>
		Keyboard,
		/// <summary>
		/// Mouse input source
		/// </summary>
		Mouse,
		/// <summary>
		/// GamePad input source
		/// </summary>
		GamePad
	}

	/// <summary>
	/// Represents mouse buttons
	/// </summary>
	public enum MouseButtons
	{
		/// <summary>
		/// Right mouse button
		/// </summary>
		Right,
		/// <summary>
		/// Left mouse button
		/// </summary>
		Left
	}
}