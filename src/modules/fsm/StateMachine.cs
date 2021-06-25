using System;
using Microsoft.Xna.Framework;

namespace Ladybug.FSM
{
	/// <summary>
	/// Ladybug Finite State Machine
	/// </summary>
	public class StateMachine
	{
		/// <summary>
		/// The current <see cref="Ladybug.FSM.State"/> has changed
		/// </summary>
		public event EventHandler StateChanged;

		private State _currentState = new State();

		/// <summary>
		/// Creates a new StateMachine instance
		/// </summary>
		public StateMachine()
		{

		}

		/// <summary>
		/// Creates a new Statemachine Instance
		/// </summary>
		/// <param name="initialState">Initial <see cref="Ladybug.FSM.State"/></param>
		public StateMachine(State initialState)
		{
			ChangeState(initialState);
		}

		/// <summary>
		/// Changes the active <see cref="Ladybug.FSM.State"/>
		/// </summary>
		/// <param name="newState">New <see cref="Ladybug.FSM.State"/></param>
		public void ChangeState(State newState)
		{
			if (_currentState != null)
			{
				_currentState._Exit();
			}

			_currentState = newState;
			_currentState._Enter();
			StateChanged?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Updates the StateMachine
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			_currentState?._Update(gameTime);
		}
	}
}