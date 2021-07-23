#pragma warning disable 1591 // Hide XMLdoc warnings.

using System;

using Microsoft.Xna.Framework;

namespace Ladybug.FSM
{
	/// <summary>
	/// A State that can be composed inline
	/// </summary>
	public sealed class ComposedState : State
	{
		private Action _onEnter = () => { };
		private Action<GameTime> _onUpdate = (GameTime gameTime) => { };
		private Action _onExit = () => { };
		internal ComposedState() { }

		/// <summary>
		/// Sets the behavior of this State when
		/// this becomes the active
		/// state of a <see cref="StateMachine"/>
		/// </summary>
		/// <param name="action">Action defining OnEnter behavior</param>
		/// <returns></returns>
		public ComposedState OnEnter(Action action)
		{
			_onEnter = action;
			return this;
		}
		protected override void Enter()
		{
			_onEnter();
		}

		/// <summary>
		/// Sets the behavior of this State when
		/// its <see cref="StateMachine"/> is updated
		/// </summary>
		/// <param name="action">Action defining OnUpdate behavior</param>
		/// <returns></returns>
		public ComposedState OnUpdate(Action<GameTime> action)
		{
			_onUpdate = action;
			return this;
		}
		protected override void Update(GameTime gameTime)
		{
			_onUpdate(gameTime);
		}

		/// <summary>
		/// Sets the behavior of this State when
		/// a <see cref="StateMachine"/> replaces this State with
		/// another State
		/// </summary>
		/// <param name="action">Action defining OnExit behavior</param>
		/// <returns></returns>
		public ComposedState OnExit(Action action)
		{
			_onExit = action;
			return this;
		}
		protected override void Exit()
		{
			_onExit();
		}
	}
}
#pragma warning restore 1591