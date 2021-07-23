using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.ECS;

namespace Ladybug.Tests.ECS
{
	public class TestComponentSystem : ComponentSystem<TestComponent> { }
	public class TestComponent : Component
	{
		public bool InitializeCalled { get; private set; } = false;

		public bool PreUpdateCalled { get; private set; } = false;
		public bool UpdateCalled { get; private set; } = false;
		public bool PostUpdateCalled { get; private set; } = false;
		
		public bool PreDrawCalled { get; private set; } = false;
		public bool DrawCalled { get; private set; } = false;
		public bool PostDrawCalled { get; private set; } = false;

		protected override void Initialize()
		{
			InitializeCalled = true;
		}

		protected override void PreUpdate(GameTime gameTime) => PreUpdateCalled = true;		
		protected override void Update(GameTime gameTime) => UpdateCalled = true;
		protected override void PostUpdate(GameTime gameTime) => PostUpdateCalled = true;

		protected override void PreDraw(GameTime gameTime, SpriteBatch spriteBatch) => PreDrawCalled = true;
		protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch) => DrawCalled = true;
		protected override void PostDraw(GameTime gameTime, SpriteBatch spriteBatch) => PostDrawCalled = true;
		
	}
}