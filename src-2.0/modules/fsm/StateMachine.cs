using System;
using Microsoft.Xna.Framework;

namespace Ladybug.FSM
{
	public class StateMachine
	{
		public event EventHandler StateChanged;

		private IState _currentState = new State();

		public StateMachine()
		{

		}

		public StateMachine(IState initialState)
		{
			ChangeState(initialState);
		}

		public void ChangeState(IState newState)
		{
			if (_currentState != null)
			{
				_currentState.Exit();
			}

			_currentState = newState;
			_currentState.Enter();
			StateChanged?.Invoke(this, new EventArgs());
		}

		public void Update(GameTime gameTime)
		{
			_currentState?.Update(gameTime);
		}
	}
}