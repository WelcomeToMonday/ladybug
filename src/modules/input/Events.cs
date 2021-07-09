using System;

namespace Ladybug.UserInput
{
	/// <summary>
	/// Event parameters containing information on an input event's name, source, and state
	/// </summary>
	public class InputActionEventArgs : EventArgs
	{
		/// <summary>
		/// Creates a new input action event argument
		/// </summary>
		/// <param name="name">Name of the action</param>
		/// <param name="source">Input source</param>
		/// <param name="state">Input state</param>
		/// <param name="playerIndex">GamePad index (only used when source is GamePad)</param>
		public InputActionEventArgs(string name, InputSource source, InputState state, int playerIndex = -1)
		{
			Name = name;
			InputSource = source;
			InputState = state;
			PlayerIndex = playerIndex;
		}
		/// <summary>
		/// Name of the input action
		/// </summary>
		/// <value></value>
		public string Name { get; private set; }
		/// <summary>
		/// Source of the input action
		/// </summary>
		/// <value></value>
		public InputSource InputSource { get; private set; }
		/// <summary>
		/// State of the input action
		/// </summary>
		/// <value></value>
		public InputState InputState { get; private set; }
		/// <summary>
		/// Index of the GamePad, if the action's source is GamePad
		/// </summary>
		/// <value></value>
		public int PlayerIndex { get; private set; }
	}
}