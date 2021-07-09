using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.UserInput;

namespace Ladybug
{
	/// <summary>
	/// Ladybug Game instance
	/// </summary>
	public class Game : Microsoft.Xna.Framework.Game
	{
		/// <summary>
		/// The list of Scenes being managed by this Game instance
		/// </summary>
		/// <remarks>
		/// Use <see cref="Game.LoadScene()"/> and <see cref="Game.UnloadScene(Scene)"/> to add and remove Scenes from the list
		/// </remarks>
		protected List<Scene> SceneList = new List<Scene>();

		/// <summary>
		/// Creates a new Ladybug Game instance
		/// </summary>
		/// <returns></returns>
		public Game() : base()
		{
			GraphicsDeviceManager = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			GraphicsDeviceManager.ApplyChanges();
			ThreadManager = new ThreadManager();
		}

		/// <summary>
		/// Game-global Resource Catalog
		/// </summary>
		/// <value></value>
		public ResourceCatalog ResourceCatalog { get; private set; }

		/// <summary>
		/// The Game instance's resident GraphicsDeviceManager, used by managed scenes for rendering.
		/// </summary>
		public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

		/// <summary>
		/// This Game instance's resident <see cref="Ladybug.ThreadManager"/>
		/// </summary>
		/// <value></value>
		public ThreadManager ThreadManager { get; private set; }

		/// <summary>
		/// Creates and Loads a new <see cref="Ladybug.Scene"/> asynchronously
		/// </summary>
		/// <typeparam name="T">Type of <see cref="Ladybug.Scene"/> to create</typeparam>
		public async void LoadSceneAsync<T>() where T : Scene, new()
		{
			var scene = new T();
			scene.SetGame(this);
			await Task.Run(() => LoadSceneAsync(scene));
		}

		/// <summary>
		/// Loads a Scene asynchronously
		/// </summary>
		/// <param name="scene"><see cref="Ladybug.Scene"/> to be loaded asynchronously</param>
		public async void LoadSceneAsync(Scene scene)
		{
			if (scene.Game == null)
			{
				scene.SetGame(this);
			}
			
			if (!scene.ContentLoadedAsync) await Task.Run(() => scene._LoadContentAsync());
			if (!scene.InitializedAsync) await Task.Run(() => scene._InitializeAsync());
			ThreadManager.QueueAction(() => LoadScene(scene));
		}

		/// <summary>
		/// Creates and Loads a new <see cref="Ladybug.Scene"/> into the Game instance
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public void LoadScene<T>() where T : Scene, new()
		{
			var scene = new T();
			scene.SetGame(this);
			LoadScene(scene);
		}

		/// <summary>
		/// Loads a <see cref="Ladybug.Scene"/> into the Game instance
		/// </summary>
		/// <param name="scene">Scene to be loaded</param>
		public void LoadScene(Scene scene)
		{
			if (scene.Game == null)
			{
				scene.SetGame(this);
			}

			SceneList.Add(scene);
			if (!scene.ContentLoaded) scene._LoadContent();
			if (!scene.Initialized) scene._Initialize();
		}

		/// <summary>
		/// Removes a <see cref="Ladybug.Scene"/> from the Game instance, unloading its assets and removing it from the
		/// scene execution loop.
		/// </summary>
		/// <param name="scene">Scene to be unloaded</param>
		internal void UnloadScene(Scene scene)
		{
			if (SceneList.Contains(scene))
			{
				SceneList.Remove(scene);
			}
		}

		/// <summary>
		/// Updates all Scenes managed by this Game instance which are neither Paused nor Suspended.
		/// </summary>
		/// <param name="gameTime">Time passed since previous Update</param>
		protected override void Update(GameTime gameTime)
		{
			ThreadManager.Update();
			Input.Begin();
			for (var i = 0; i < SceneList.Count; i++)
			{
				if (SceneList[i].State == SceneState.ACTIVE)
				{
					SceneList[i]._Update(gameTime);
				}
			}
			Input.End();
		}

		/// <summary>
		/// Renders all Scenes managed by this Game instance which are not Suspended.
		/// </summary>
		/// <param name="gameTime">Time passed since previous Draw</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			foreach (var scene in SceneList)
			{
				if (scene.State != SceneState.SUSPENDED)
				{
					scene._Draw(gameTime);
				}
			}
			base.Draw(gameTime);
		}
	}
}