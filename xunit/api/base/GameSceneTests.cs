using System.Threading.Tasks;

using Xunit;
using Microsoft.Xna.Framework;

namespace Ladybug.Tests.Base
{
	[Collection("Unique: Game Instantiation")]
	public class GameSceneTests
	{

		/// <summary>
		/// Asserts that a Game and Scene can be instantiated successfully,
		/// A Game can load a Scene,
		/// and that a Scene's basic Initialization, Content Loading,
		/// Updating, and Drawing are called by the Game instance.
		/// </summary>
		[Fact]
		public void CreateGameAndAddScene()
		{
			var testInit = 0;
			var testLoad = 0;
			var testUpdate = 0;
			var testDraw = 0;

			using (var game = new Game())
			{
				var scene = Scene.Compose(game)
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

		/// <summary>
		/// Asserts that a Scene can be initialized asynchronously
		/// </summary>
		[Fact]
		public void TestInitializeAsync()
		{
			var initStart = false;
			var initComplete = false;

			var scene = Scene.Compose()
				.OnInitializeAsync(() =>
					Task.Run(() =>
					{
						initStart = true;
						Task.Delay(5000);
						initComplete = true;
					}));

			using (var game = new Game())
			{
				game.LoadSceneAsync(scene);
				game.RunOneFrame();
			}

			Assert.True(initStart, "Async init did not start");
			Assert.True(initComplete, "Async init did not complete");
		}

		/// <summary>
		/// Asserts that a Scene can load content asynchronously
		/// </summary>
		[Fact]
		public void TestLoadContentAsync()
		{
			var loadStart = false;
			var loadComplete = false;

			var scene = Scene.Compose()
				.OnLoadContentAsync(() =>
					Task.Run(() =>
					{
						loadStart = true;
						Task.Delay(5000);
						loadComplete = true;
					}));

			using (var game = new Game())
			{
				game.LoadSceneAsync(scene);
				game.RunOneFrame();
			}

			Assert.True(loadStart, "Async init did not start");
			Assert.True(loadComplete, "Async init did not complete");
		}
	}
}
