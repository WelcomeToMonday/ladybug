using Xunit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Tests.Base
{
	[Collection("Unique: Game Instantiation")]
	public class ResourceCatalogTests
	{
		[Fact]
		public void TestLoadContent()
		{
			var scene = new Scene();
			Texture2D texture = null;
			var drawCalled = false;
			var loadContentCalled = false;
			var initializeCalled = false;

			scene.OnLoadContent(() =>
			{
				loadContentCalled = true;
				scene.ResourceCatalog.LoadResource<Texture2D>("ladybug icon", "image/ladybug-icon");
			})
			.OnInitialize(() =>
			{
				initializeCalled = true;
				texture = scene.ResourceCatalog.GetResource<Texture2D>("ladybug icon");
			})
			.OnDraw((GameTime gameTime) =>
			{
				drawCalled = true;
				scene.SpriteBatch.Begin();
				scene.SpriteBatch.Draw(texture, Vector2.Zero, Color.White);
				scene.SpriteBatch.End();
			});

			using (var game = new Game())
			{
				game.LoadScene(scene);
				game.RunOneFrame();
			}

			Assert.True(drawCalled, "Draw was never called");
			Assert.True(loadContentCalled, "LoadContent was never called");
			Assert.True(initializeCalled, "Initialize was never called");
			Assert.True(texture != null, "Texture was not set");
		}
	}
}