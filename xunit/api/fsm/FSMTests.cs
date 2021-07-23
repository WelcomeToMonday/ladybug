using Xunit;

using Microsoft.Xna.Framework;

using Ladybug.FSM;

namespace Ladybug.Tests.FSM
{
	public class FSMTests
	{
		[Fact]
		public void TestInlineState()
		{
			bool entered = false;
			bool updated = false;
			bool exited = false;
			bool changed = false;

			var inlineState = State.Compose()
				.OnEnter(() => entered = true)
				.OnUpdate((GameTime gameTime) => updated = true)
				.OnExit(() => exited = true);

			var endState = State.Compose()
				.OnEnter(() => { changed = true; });

			var stateMachine = new StateMachine();

			stateMachine.ChangeState(inlineState);
			stateMachine.Update(null);
			stateMachine.ChangeState(endState);

			Assert.True(entered);
			Assert.True(updated);
			Assert.True(exited);
			Assert.True(changed);
		}

		[Fact]
		public void TestDerivedState()
		{
			var changed = false;

			var state = new TestState();
			var endState = State.Compose()
				.OnEnter(() => changed = true);
			var stateMachine = new StateMachine();

			stateMachine.ChangeState(state);
			stateMachine.Update(null);
			stateMachine.ChangeState(endState);

			Assert.True(changed);
			Assert.True(state.EnterCalled);
			Assert.True(state.UpdateCalled);
			Assert.True(state.ExitCalled);
		}
	}
}