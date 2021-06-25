using System;

using Microsoft.Xna.Framework;

namespace Ladybug.FSM
{
	/// <summary>
	/// A State that can be processed by a
	/// <see cref="Ladybug.FSM.StateMachine"/>
	/// </summary>
	public class State
	{
		/// <summary>
		/// State has become the StateMachine's
		/// active state
		/// </summary>
		public event EventHandler Entered;

		/// <summary>
		/// State is no longer the StateMachine's
		/// active state
		/// </summary>
		public event EventHandler Exited;

		private Action _onEnter = () => {};
		private Action<GameTime> _onUpdate = (GameTime gameTime) => {};
		private Action _onExit = () => {};

		/// <summary>
		/// Creates a new State
		/// </summary>
		public State()
		{

		}

		/// <summary>
		/// Crates a new State,
		/// copying the behavior of the provided
		/// state
		/// </summary>
		/// <param name="template">Template State</param>
		public State(State template)
		{
			_onEnter = template._onEnter;
			_onUpdate = template._onUpdate;
			_onExit = template._onExit;
		}

		internal void _Enter()
		{
			_onEnter();
			Entered?.Invoke(this, new EventArgs());
		}

		internal void _Update(GameTime gameTime)
		{
			_onUpdate(gameTime);
		}
		
		internal void _Exit()
		{
			_onExit();
			Exited?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Sets the behavior of this State when
		/// this becomes the active
		/// state of a <see cref="StateMachine"/>
		/// </summary>
		/// <param name="action">Action defining OnEnter behavior</param>
		/// <returns></returns>
		public State OnEnter(Action action)
		{
			_onEnter = action;
			return this;
		}

		/// <summary>
		/// Sets the behavior of this State when
		/// its <see cref="StateMachine"/> is updated
		/// </summary>
		/// <param name="action">Action defining OnUpdate behavior</param>
		/// <returns></returns>
		public State OnUpdate(Action<GameTime> action)
		{
			_onUpdate = action;
			return this;
		}

		/// <summary>
		/// Sets the behavior of this State when
		/// a <see cref="StateMachine"/> replaces this State with
		/// another State
		/// </summary>
		/// <param name="action">Action defining OnExit behavior</param>
		/// <returns></returns>
		public State OnExit(Action action)
		{
			_onExit = action;
			return this;
		}
	}
}