
using System;
using Xunit;

using Microsoft.Xna.Framework;

namespace Ladybug.Tests.Documentation.Articles
{
	// /articles/setting-up-the-game.html
	[Collection("Unique: Game Instantiation")]
	/// <summary>
	/// Asserts that all code samples in the Getting Started
	/// documentation and tutorials remains valid.
	/// </summary>
	public class GettingStarted
	{
		private int _testInt;

		[Fact]
		public void CreateGame()
		{
			using (var game = new Game())
			{
				game.RunOneFrame();
			}
			Assert.True(true);
		}

		[Fact]
		public void CreateSceneInGame()
		{
			var testInit = 0;
			using (var game = new Game())
			{
				var scene = Scene.Compose(game)
					.OnLoadContent(()=>{})
					.OnInitialize(()=>{testInit++;})
					.OnUpdate((GameTime gameTime)=>{})
					.OnDraw((GameTime GameTime)=>{});

				game.LoadScene(scene);
				game.RunOneFrame();

				Assert.Equal(1, testInit);
			}
		}

		[Fact]
		public void CreateSceneFromMethod()
		{
			using (var game = new Game())
			{
				var mainScene = CreateMainScene(game);

				game.LoadScene(mainScene);

				game.RunOneFrame();
			}
			Assert.Equal(1, _testInt);
		}

		private Scene CreateMainScene(Game game)
		{
			var mainScene = Scene.Compose(game)
				.OnUpdate((GameTime gameTime) =>
				{
					_testInt++;
				});
			return mainScene;
		}
	}
}