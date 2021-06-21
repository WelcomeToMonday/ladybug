using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Ladybug;
using Microsoft.Xna.Framework;

namespace Ladybug.Test
{
	public class GameSceneTests
	{
		[Fact]
		public void CreateGameAndAddScene()
		{
			var testInit = 0;
			var testLoad = 0;
			var testUpdate = 0;
			var testDraw = 0;

			using (var game = new Game())
			{
				var scene = new Scene(game)
					.OnInitialize(() => { testInit = 1; })
					.OnLoadContent(() => { testLoad = 1; })
					.OnUpdate((GameTime gameTime) => { testUpdate = 1; })
					.OnDraw((GameTime gameTime) => { testDraw = 1; });

				game.LoadScene(scene);
				game.RunOneFrame();
			}

			Assert.True(testInit == 1, $"Scene Initialization failed (Expected 1, got {testInit})");
			Assert.True(testLoad == 1, $"Scene Initialization failed (Expected 1, got {testLoad})");
			Assert.True(testUpdate == 1, $"Scene Initialization failed (Expected 1, got {testUpdate})");
			Assert.True(testDraw == 1, $"Scene Initialization failed (Expected 1, got {testDraw})");
		}
	}
}
