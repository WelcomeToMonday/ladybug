using Microsoft.Xna.Framework;

namespace Ladybug.FSM
{
	public interface IState
	{
		void Enter();
		void Update(GameTime gameTime);
		void Exit();
	}
}