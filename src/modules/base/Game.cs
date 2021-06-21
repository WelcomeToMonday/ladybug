using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Ladybug
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		/// <summary>
		/// The list of Scenes being managed by this SceneManager
		/// </summary>
		/// <typeparam name="Scene"></typeparam>
		/// <remarks>
		/// Use <c>LoadScene()</c> and <c>UnloadScene()</c> to add and remove Scenes from the list
		/// </remarks>
		protected List<Scene> SceneList = new List<Scene>();

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
		public ResourceCatalog ResourceCatalog {get; private set;}

		/// <summary>
		/// The SceneManager's resident GraphicsDeviceManager, used by managed scenes for rendering.
		/// </summary>
		public GraphicsDeviceManager GraphicsDeviceManager {get; private set;}

		public ThreadManager ThreadManager { get; private set; }

		/// <summary>
		/// Loads a Scene asynchronously, calling its LoadContentAsync and InitializeAsync before
		/// adding the scene to the SceneManager via LoadScene
		/// </summary>
		/// <param name="scene">Scene to be loaded asynchronously</param>
		/// <remarks>
		/// Calls LoadScene, meaning the scene's LoadContent and LoadScene methods are called
		/// after their async counterparts.
		/// </remarks>
		public async virtual void LoadSceneAsync(Scene scene)
		{
			if (!scene.ContentLoadedAsync) await Task.Run(() => scene.LoadContentAsync());
			if (!scene.InitializedAsync) await Task.Run(() => scene.InitializeAsync());
			ThreadManager.QueueAction(() => LoadScene(scene));
		}

		/// <summary>
		/// Loads a scene into the SceneManager, which will update the Scene every game loop.
		/// </summary>
		/// <param name="scene">Scene to be loaded</param>
		public virtual void LoadScene(Scene scene)
		{
			SceneList.Add(scene);
			if (!scene.ContentLoaded) scene.LoadContent();
			if (!scene.Initialized) scene.Initialize();
		}

		/// <summary>
		/// Removes a scene from the SceneManager, unloading its assets and removing it from the
		/// scene execution loop.
		/// </summary>
		/// <param name="scene">Scene to be unloaded</param>
		public virtual void UnloadScene(Scene scene)
		{
			if (SceneList.Contains(scene))
			{
				scene.Unload();
				SceneList.Remove(scene);
			}
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			foreach (var scene in SceneList)
			{
				if (!scene.ContentLoaded) scene.LoadContent();
			}
		}

		/// <summary>
		/// Updates all Scenes managed by this SceneManager which are neither Paused nor Suspended.
		/// </summary>
		/// <param name="gameTime">Time passed since previous Update</param>
		protected override void Update(GameTime gameTime)
		{
			ThreadManager.Update();
			for (var i = 0; i < SceneList.Count; i++)
			{
				if (SceneList[i].State == SceneState.ACTIVE)
				{
					SceneList[i].Update(gameTime);
				}
			}
		}

		/// <summary>
		/// Renders all Scenes managed by this SceneManager which are not Suspended.
		/// </summary>
		/// <param name="gameTime">Time passed since previous Draw</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			foreach (var scene in SceneList)
			{
				if (scene.State != SceneState.SUSPENDED)
				{
					scene.Draw(gameTime);
				}
			}
			base.Draw(gameTime);
		}
	}
}