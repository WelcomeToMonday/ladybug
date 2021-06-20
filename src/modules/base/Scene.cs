using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Ladybug.ECS;

namespace Ladybug
{
	public enum SceneState
	{
		/// <summary>Active Scenes will have both <c>Update()</c> and <c>Draw</c> called each frame.</summary>
		ACTIVE,
		/// <summary>Paused Scenes will only have <c>Draw()</c> called each frame</summary>
		PAUSED,
		/// <summary>Suspended Scenes will have neither <c>Update()</c> nor <c>Draw()</c> called each frame.</summary>
		SUSPENDED
	}

	/// <summary>
	/// A Scene represents a single game Update/Render loop.
	/// </summary>
	public class Scene
	{

		public event EventHandler LoadContentAsyncStart;
		public event EventHandler LoadContentAsyncComplete;

		public event EventHandler InitializeAsyncStart;
		public event EventHandler InitializeAsyncComplete;

		public event EventHandler LoadContentComplete;
		public event EventHandler InitializeComplete;

		public event EventHandler Paused;
		public event EventHandler Unpaused;

		public event EventHandler Suspended;
		public event EventHandler Unsuspended;

		public event EventHandler Unloaded;

		/// <summary>
		/// The Scene has been Paused or Suspended
		/// </summary>
		public event EventHandler Stopped;
		/// <summary>
		/// The Scene has been Unpaused or Unsuspended
		/// </summary>
		public event EventHandler Resumed;

		/// <summary>
		/// The Scene's default SpriteBatch
		/// </summary>
		protected SpriteBatch SpriteBatch;

		private bool _firstUpdateComplete = false;
		private bool _firstDrawComplete = false;

		public Scene(Game game)
		{
			Game = game;
			Content = new ContentManager(Game.Content.ServiceProvider);
			Content.RootDirectory = "Content";
			ResourceCatalog = new ResourceCatalog(Content);
			SpriteBatch = new SpriteBatch(game.GraphicsDevice);
		}

		public bool InitializedAsync { get; private set; } = false;
		public bool ContentLoadedAsync { get; private set; } = false;

		public bool Initialized { get; private set; } = false;
		public bool ContentLoaded { get; private set; } = false;

		public EntitySystem EntitySystem {get; private set;}

		/// <summary>
		/// Scene-local ResourceCatalog
		/// </summary>
		/// <value></value>
		public ResourceCatalog ResourceCatalog {get; private set;}

		/// <summary>
		/// Reference to the Scene's ContentManager
		/// 
		/// Each Scene contains its own separate ContentManager so its assets can be
		/// individually loaded and unloaded along with the Scene
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// This ContentManager is intended to be accessed primarily through the Scene's 
		/// ResourceCatalog.
		/// </remarks>
		public ContentManager Content { get; private set; }

		/// <summary>
		/// Reference to the Game instance that is handling this scene
		/// </summary>
		public Game Game { get; protected set; }

		public SceneState State { get; set; } = SceneState.ACTIVE;

		public async virtual void LoadContentAsync()
		{
			ContentLoadedAsync = true;
			LoadContentAsyncComplete?.Invoke(this, new EventArgs());
			ThreadManager.QueueAction(() => LoadContent());
		}

		public async virtual void InitializeAsync()
		{
			InitializedAsync = true;
			InitializeAsyncComplete?.Invoke(this, new EventArgs());
			ThreadManager.QueueAction(() => Initialize());
		}

		public virtual void LoadContent()
		{
			ContentLoaded = true;
			LoadContentComplete?.Invoke(this, new EventArgs());
		}

		public virtual void Initialize()
		{
			Initialized = true;
			InitializeComplete?.Invoke(this, new EventArgs());
		}

		public virtual void Update(GameTime gameTime)
		{
			if (!_firstUpdateComplete)
			{
				FirstUpdate();
				_firstUpdateComplete = true;
			}
			if (EntitySystem != null)
			{
				EntitySystem.PreUpdate(gameTime);
				EntitySystem.Update(gameTime);
				EntitySystem.PostUpdate(gameTime);
			}
		}

		public virtual void Draw(GameTime gameTime)
		{
			if (!_firstDrawComplete)
			{
				FirstDraw();
				_firstDrawComplete = true;
			}
			if (EntitySystem != null)
			{
				EntitySystem.Draw(gameTime, SpriteBatch);
			}
		}

		/// <summary>
		/// Called before this scene's first Update.
		/// Useful for last-second initialization.
		/// </summary>
		public virtual void FirstUpdate()
		{

		}

		/// <summary>
		/// Called before this scene's first Draw.
		/// Useful for last-second initialization
		/// </summary>
		public virtual void FirstDraw()
		{

		}

		/// <summary>
		/// Unloads the Scene
		/// </summary>
		public void Unload()
		{
			Unloaded?.Invoke(this, new EventArgs());
			OnUnload();
		}

		/// <summary>
		/// Called when the Scene has been unloaded
		/// </summary>
		public virtual void OnUnload() { }

		/// <summary>
		/// Pauses the Scene
		/// </summary>
		/// <remarks>
		/// A paused Scene will not execute Update(),
		/// but will still execute Draw()
		/// </remarks>
		public void Pause()
		{
			State = SceneState.PAUSED;
			Paused?.Invoke(this, new EventArgs());
			OnPause();
			Stopped?.Invoke(this, new EventArgs());
			OnStop();
		}

		/// <summary>
		/// Called when the Scene has been paused
		/// </summary>
		public virtual void OnPause() { }

		/// <summary>
		/// Unpauses the Scene
		/// </summary>
		/// <remarks>
		/// Returns the Scene to ACTIVE state,
		/// only if state was previously PAUSED
		/// </remarks>
		public void Unpause()
		{
			if (State == SceneState.PAUSED) 
			{
				State = SceneState.ACTIVE;
				Unpaused?.Invoke(this, new EventArgs());
				OnUnpause();
				Resumed?.Invoke(this, new EventArgs());
				OnResume();
			}
		}

		/// <summary>
		/// Called when the Scene is Unpaused
		/// </summary>
		public virtual void OnUnpause() {}

		/// <summary>
		/// Suspends the Scene
		/// </summary>
		/// <remarks>
		/// A suspended Scene will not execute
		/// Update() or Draw()
		/// </remarks>
		public void Suspend()
		{
			State = SceneState.SUSPENDED;
			Suspended?.Invoke(this, new EventArgs());
			OnSuspend();
			Stopped?.Invoke(this, new EventArgs());
			OnStop();
		}

		/// <summary>
		/// Called when the Scene is suspended
		/// </summary>
		public virtual void OnSuspend() {}

		/// <summary>
		/// Unsuspends the Scene
		/// </summary>
		/// <remarks>
		/// Returns the Scene to ACTIVE state,
		/// only if state was previously SUSPENDED
		/// </remarks>
		public void Unsuspend()
		{
			if (State == SceneState.SUSPENDED) 
			{
				State = SceneState.ACTIVE;
				Unsuspended?.Invoke(this, new EventArgs());
				OnUnsuspend();
				Resumed?.Invoke(this, new EventArgs());
				OnResume();
			}
		}

		/// <summary>
		/// Called when the Scene is Unsuspended
		/// </summary>
		public virtual void OnUnsuspend() {}

		/// <summary>
		/// Called when the Scene is Paused or Suspended
		/// </summary>
		public virtual void OnStop() {}

		/// <summary>
		/// Called when the Scene is Unpaused or Unsuspended
		/// </summary>
		public virtual void OnResume() {}

	}
}