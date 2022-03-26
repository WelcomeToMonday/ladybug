using System;
using Microsoft.Xna.Framework;

namespace Ladybug.FSM
{
	/// <summary>
	/// Ladybug Finite State Machine
	/// </summary>
	public class StateMachine<T> where T : State
	{
		/// <summary>
		/// The current <see cref="Ladybug.FSM.State"/> has changed
		/// </summary>
		public event EventHandler StateChanged;

		/// <summary>
		/// The FSM's current State
		/// </summary>
		/// <value></value>
		protected T CurrentState { get; private set; }

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
		public StateMachine(T initialState)
		{
			ChangeState(initialState);
		}

		/// <summary>
		/// Changes the active <see cref="Ladybug.FSM.State"/>
		/// </summary>
		/// <param name="newState">New <see cref="Ladybug.FSM.State"/></param>
		public void ChangeState(T newState)
		{
			if (CurrentState != null)
			{
				CurrentState._Exit();
			}

			CurrentState = newState;
			CurrentState._Enter();
			StateChanged?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Updates the StateMachine
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			CurrentState?._Update(gameTime);
		}
	}

	/// <summary>
	/// Ladybug Finite State Machine
	/// </summary>
	public class StateMachine : StateMachine<State>
	{
		/// <summary>
		/// Creates a new StateMachine instance
		/// </summary>
		public StateMachine() : base()
		{
			ChangeState(State.Compose());
		}
	}
}