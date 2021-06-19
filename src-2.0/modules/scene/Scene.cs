using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Ladybug.Scene
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

		protected SpriteBatch spriteBatch;

		public bool InitializedAsync { get; private set; } = false;
		public bool ContentLoadedAsync { get; private set; } = false;

		public bool Initialized { get; private set; } = false;
		public bool ContentLoaded { get; private set; } = false;

		private bool _firstUpdateComplete = false;
		private bool _firstDrawComplete = false;

		public Scene(SceneManager sceneManager) // Is there any way to make this not have to consume a sceneManager to work?
		{
			SceneManager = sceneManager;
			Content = new ContentManager(sceneManager.Content.ServiceProvider);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Reference to the Scene's ContentManager
		/// 
		/// Each Scene contains its own separate ContentManager so its assets can be
		/// individually loaded and unloaded along with the Scene
		/// </summary>
		/// <value></value>
		public ContentManager Content { get; private set; }

		/// <summary>
		/// Reference to the SceneManager that is handling this scene
		/// </summary>
		public SceneManager SceneManager { get; protected set; }

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
		}

		public virtual void Draw(GameTime gameTime)
		{
			if (!_firstDrawComplete)
			{
				FirstDraw();
				_firstDrawComplete = true;
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

		public virtual void Unload()
		{
			Unloaded?.Invoke(this, new EventArgs());
		}

		public virtual void Pause()
		{
			State = SceneState.PAUSED;
			Paused?.Invoke(this, new EventArgs());
			Stopped?.Invoke(this, new EventArgs());
		}

		public virtual void Unpause()
		{
			if (State == SceneState.PAUSED) State = SceneState.ACTIVE;
			Unpaused?.Invoke(this, new EventArgs());
			Resumed?.Invoke(this, new EventArgs());
		}

		public virtual void Suspend()
		{
			State = SceneState.SUSPENDED;
			Suspended?.Invoke(this, new EventArgs());
			Stopped?.Invoke(this, new EventArgs());
		}

		public virtual void Unsuspend()
		{
			if (State == SceneState.SUSPENDED) State = SceneState.ACTIVE;
			Unsuspended?.Invoke(this, new EventArgs());
			Resumed?.Invoke(this, new EventArgs());
		}
	}
}