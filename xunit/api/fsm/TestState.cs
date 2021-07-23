using Microsoft.Xna.Framework;

using Ladybug.FSM;

namespace Ladybug.Tests.FSM
{
	public class TestState : State
	{
		public bool EnterCalled { get; private set; } = false;
		public bool UpdateCalled { get; private set; } = false;
		public bool ExitCalled { get; private set; } = false;

		protected override void Enter()
		{
			EnterCalled = true;
		}

		protected override void Update(GameTime gameTime)
		{
			UpdateCalled = true;
		}

		protected override void Exit()
		{
			ExitCalled = true;
		}
	}
}