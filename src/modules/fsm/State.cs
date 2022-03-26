using System;

using Microsoft.Xna.Framework;

namespace Ladybug.FSM
{
	/// <summary>
	/// A State that can be processed by a
	/// <see cref="Ladybug.FSM.StateMachine"/>
	/// </summary>
	public abstract class State
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

		/// <summary>
		/// Begin inline composing of a new State
		/// </summary>
		/// <returns></returns>
		public static ComposedState Compose() => new ComposedState();

		/// <summary>
		/// Method called when a StateMachine enters this State
		/// </summary>
		protected virtual void Enter() { }
		internal void _Enter()
		{
			Enter();
			Entered?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Method called each frame a StateMachine is in this State
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void Update(GameTime gameTime) { }
		internal void _Update(GameTime gameTime)
		{
			Update(gameTime);
		}
		
		/// <summary>
		/// Method called when a StateMachine exits this State
		/// </summary>
		protected virtual void Exit() { }
		internal void _Exit()
		{
			Exit();
			Exited?.Invoke(this, new EventArgs());
		}
	}
}