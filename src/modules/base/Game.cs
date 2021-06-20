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

		/// <summary>
		/// The SceneManager's resident GraphicsDeviceManager, used by managed scenes for rendering.
		/// </summary>
		public GraphicsDeviceManager graphics;

		public Game() : base()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			graphics.ApplyChanges(); //Sets the GraphicsDevice
			ThreadManager = new ThreadManager();
		}

		public ThreadManager ThreadManager { get; private set; }

		/// <summary>
		/// Loads a Scene asynchronously, calling its LoadContentAsync and InitializeAsync before
		/// adding the scene to the SceneManager via LoadScene
		/// </summary>
		/// <param name="scene"></param>
		/// <remarks>
		/// Calls LoadScene, meaning the scene's LoadContent and LoadScene methods are called
		/// after their async counterparts.
		/// </remarks>
		public async virtual void LoadSceneAsync(Scene scene)
		{
			if (!scene.ContentLoadedAsync) await Task.Run(() => scene.LoadContentAsync());
			if (!scene.InitializedAsync) await Task.Run(() => scene.InitializeAsync());
			ThreadManager.QueueAction(() => LoadScene(scene));
			//LoadScene(scene);
		}

		/// <summary>
		/// Loads a scene into the SceneManager, which will update the Scene every game loop.
		/// </summary>
		/// <param name="scene"></param>
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
		/// <param name="scene"></param>
		public virtual void UnloadScene(Scene scene)
		{
			if (SceneList.Contains(scene))
			{
				scene.Unload();
				SceneList.Remove(scene);
			}
		}

		/// <summary>
		/// Pauses a Scene, skipping its <c>Update()</c>, but continuing to run its <c>Draw()</c>
		/// </summary>
		/// <param name="scene"></param>
		public virtual void PauseScene(Scene scene)
		{
			scene.Pause();
			//scene.State = SceneState.PAUSED;
		}

		/// <summary>
		/// Unpauses a Scene, causing it to resume its <c>Update()</c>
		/// </summary>
		/// <param name="scene"></param>
		public virtual void UnpauseScene(Scene scene)
		{
			scene.Unpause();
			//scene.State = SceneState.ACTIVE;
		}

		/// <summary>
		/// Suspends a scene, causing both its <c>Update()</c> and <c>Draw()</c> to be skipped
		/// </summary>
		/// <param name="scene"></param>
		public virtual void SuspendScene(Scene scene)
		{
			scene.Suspend();
			//scene.State = SceneState.SUSPENDED;
		}

		/// <summary>
		/// Unsuspends a scene, resuming both its <c>Update()</c> and <c>Draw()</c>.
		/// </summary>
		/// <param name="scene"></param>
		public virtual void UnsuspendScene(Scene scene)
		{
			scene.Unsuspend();
			//scene.State = SceneState.ACTIVE;
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
		/// <param name="gameTime"></param>
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
		/// <param name="gameTime"></param>
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