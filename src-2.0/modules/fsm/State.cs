using System;

using Microsoft.Xna.Framework;

namespace Ladybug.FSM
{
	public class State : IState
	{
		public event EventHandler StateEntered;
		public event EventHandler StateExited;

		private Action _enter = () => {};
		private Action<GameTime> _update = (GameTime gameTime) => {};
		private Action _exit = () => {};

		public State()
		{

		}

		public State(State template)
		{
			_enter = template._enter;
			_update = template._update;
			_exit = template._exit;
		}

		public void Enter()
		{
			_enter();
			StateEntered?.Invoke(this, new EventArgs());
		}

		public void Update(GameTime gameTime) => _update(gameTime);
		
		public void Exit()
		{
			_exit();
			StateExited?.Invoke(this, new EventArgs());
		}

		public State OnEnter(Action action)
		{
			_enter = action;
			return this;
		}

		public State OnUpdate(Action<GameTime> action)
		{
			_update = action;
			return this;
		}

		public State OnExit(Action action)
		{
			_exit = action;
			return this;
		}
	}
}