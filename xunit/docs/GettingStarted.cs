
using System;
using Xunit;

using Microsoft.Xna.Framework;

namespace Ladybug.Tests.Documentation
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
				var scene = new Scene(game)
					.OnLoadContent(()=>{})
					.OnInitialize(()=>{testInit++;})
					.OnUpdate((GameTime gameTime)=>{})
					.OnDraw((GameTime GameTime)=>{});

				game.LoadScene(scene);
				game.RunOneFrame();

				Assert.Equal(testInit, 1);
			}
		}

		[Fact]
		public void CreateSceneFromMethod()
		{
			using (var game = new Game())
			{
				var mainScene = CreateMainScene(game);

				game.LoadScene(mainScene);

				game.Run();
			}
			Assert.Equal(_testInt, 1);
		}

		private Scene CreateMainScene(Game game)
		{
			var mainScene = new Scene(game)
				.OnUpdate((GameTime gameTime) =>
				{
					_testInt++;
					game.Exit();
				});
			return mainScene;
		}
	}
}