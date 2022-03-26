namespace Ladybug.UserInput
{
	/// <summary>
	/// Abstract class providing basic input monitoring functionality
	/// </summary>
	/// <typeparam name="T">Input State type</typeparam>
	/// <typeparam name="K">Button/Key type</typeparam>
	public abstract class InputMonitor<T, K>
	{
		/// <summary>
		/// Previous input state
		/// </summary>
		protected T previousState;

		/// <summary>
		/// Current input state
		/// </summary>
		protected T currentState;

		/// <summary>
		/// Whether the input monitor is currently being updated
		/// </summary>
		/// <value></value>
		public bool UpdateActive { get; protected set; } = false;

		/// <summary>
		/// Begins the input monitor update process
		/// </summary>
		/// <param name="newState"></param>
		public void BeginUpdate(T newState)
		{
			currentState = newState;
			UpdateActive = true;
		}


		/// <summary>
		/// Completes the input monitor update process
		/// </summary>
		public void EndUpdate()
		{
			UpdateActive = false;
			previousState = currentState;
		}

		/// <summary>
		/// Gets the input state of a given key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public abstract InputState GetInputState(K key);

		/// <summary>
		/// Checks if a given key is in the given input state
		/// </summary>
		/// <param name="button"></param>
		/// <param name="state"></param>
		/// <returns></returns>
		public bool CheckButton(K button, InputState state) => GetInputState(button) == state;

		/// <summary>
		/// Checks if any of the given keys are in the given input state
		/// </summary>
		/// <param name="keys"></param>
		/// <param name="state"></param>
		/// <returns></returns>
		public bool CheckAnyButton(K[] keys, InputState state)
		{
			bool res = false;

			foreach (var key in keys)
			{
				if (CheckButton(key, state)) res = true;
			}

			return res;
		}

		/// <summary>
		/// Checks if all of the given keys are in the given input state
		/// </summary>
		/// <param name="keys"></param>
		/// <param name="state"></param>
		/// <returns></returns>
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