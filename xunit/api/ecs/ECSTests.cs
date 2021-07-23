using Xunit;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.ECS;
using ECSystem = Ladybug.ECS.ECS;

namespace Ladybug.Tests.ECS
{
	[Collection("Unique: Game Instantiation")]
	public class ECSTests
	{
		[Fact]
		public void TestInlineComponent()
		{
			bool initialized = false;

			bool preUpdated = false;
			bool updated = false;
			bool postUpdated = false;

			bool preDrawn = false;
			bool drawn = false;
			bool postDrawn = false;

			ECSystem system = null;

			var scene = Scene.Compose();
			scene
				.OnInitialize(() =>
				{
					system = new ECSystem(scene);

					var inlineComponent = Component.Compose()
						.OnInitialize(() => initialized = true)
						
						.OnPreUpdate((GameTime gameTime) => preUpdated = true)
						.OnUpdate((GameTime gameTime) => updated = true)
						.OnPostUpdate((GameTime gameTime) => postUpdated = true)
						
						.OnPreDraw((GameTime gameTime, SpriteBatch spriteBatch) => preDrawn = true)
						.OnDraw((GameTime gameTime, SpriteBatch spriteBatch) => drawn = true)
						.OnPostDraw((GameTime gameTime, SpriteBatch spriteBatch) => postDrawn = true);

					var entity = system.CreateEntity()
						.AddComponent(inlineComponent);

					system.Initialize();
				})
				.OnUpdate((GameTime gameTime) =>
				{
					system.Update(gameTime);
				})
				.OnDraw((GameTime gameTime) =>
				{
					system.Draw(gameTime, scene.SpriteBatch);
				});

			using (var game = new Game())
			{
				game.LoadScene(scene);
				game.RunOneFrame();
			}

			Assert.True(initialized);

			Assert.True(preUpdated);
			Assert.True(updated);
			Assert.True(postUpdated);

			Assert.True(preDrawn);
			Assert.True(drawn);
			Assert.True(postDrawn);
		}

		[Fact]
		public void TestDerivedComponent()
		{
			ECSystem system = null;
			TestComponent testComponent = null;

			var scene = Scene.Compose();
			scene
				.OnInitialize(() =>
				{
					system = new ECSystem(scene);
					system.RegisterComponentSystem<TestComponent, TestComponentSystem>();

					var entity = system.CreateEntity()
						.AddComponent<TestComponent>(out testComponent);

					system.Initialize();
				})
				.OnUpdate((GameTime gameTime) =>
				{
					system.Update(gameTime);
				})
				.OnDraw((GameTime gameTime) =>
				{
					system.Draw(gameTime, scene.SpriteBatch);
				});

			using (var game = new Game())
			{
				game.LoadScene(scene);
				game.RunOneFrame();
			}

			Assert.True(testComponent.InitializeCalled);

			Assert.True(testComponent.PreUpdateCalled);
			Assert.True(testComponent.UpdateCalled);
			Assert.True(testComponent.PostUpdateCalled);
			
			Assert.True(testComponent.PreDrawCalled);
			Assert.True(testComponent.DrawCalled);
			Assert.True(testComponent.PostDrawCalled);
		}
	}
}