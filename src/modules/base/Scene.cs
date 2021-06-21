using System;
using System.Threading.Tasks;

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

		private Func<Task> _onLoadContentAsync = new Func<Task>(() => Task.CompletedTask);
		private Func<Task> _onInitializeAsync = new Func<Task>(() => Task.CompletedTask);

		private Action _onLoadContent = () => { };
		private Action _onInitialize = () => { };

		private Action<GameTime> _onUpdate = (GameTime gameTime) => { };
		private Action<GameTime> _onDraw = (GameTime gameTime) => { };

		private Action _onUnload = () => { };

		private Action _onPause = () => { };
		private Action _onUnpause = () => { };
		
		private Action _onSuspend = () => { };
		private Action _onUnsuspend = () => { };

		private Action _onStop = () => { };
		private Action _onResume = () => { };

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

		public EntitySystem EntitySystem { get; protected set; }

		/// <summary>
		/// Scene-local ResourceCatalog
		/// </summary>
		/// <value></value>
		public ResourceCatalog ResourceCatalog { get; protected set; }

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

		public SceneState State { get; private set; } = SceneState.ACTIVE;

		public void OnLoadContentAsync(Func<Task> func) => _onLoadContentAsync = func;

		internal async void LoadContentAsync()
		{
			ContentLoadedAsync = true;
			await _onLoadContentAsync();
			LoadContentAsyncComplete?.Invoke(this, new EventArgs());
			ThreadManager.QueueAction(() => LoadContent());
		}

		public void OnInitializeAsync(Func<Task> func) => _onInitializeAsync = func;

		internal async void InitializeAsync()
		{
			InitializedAsync = true;
			await _onInitializeAsync();
			InitializeAsyncComplete?.Invoke(this, new EventArgs());
			ThreadManager.QueueAction(() => Initialize());
		}

		public void OnLoadContent(Action action) => _onLoadContent = action;

		internal void LoadContent()
		{
			ContentLoaded = true;
			LoadContentComplete?.Invoke(this, new EventArgs());
		}

		public void OnInitialize(Action action) => _onInitialize = action;

		internal void Initialize()
		{
			Initialized = true;
			InitializeComplete?.Invoke(this, new EventArgs());
		}

		public void OnUpdate(Action<GameTime> action) => _onUpdate = action;

		internal void Update(GameTime gameTime)
		{
			if (EntitySystem != null)
			{
				EntitySystem.PreUpdate(gameTime);
				EntitySystem.Update(gameTime);
				EntitySystem.PostUpdate(gameTime);
			}
		}

		public void OnDraw(Action<GameTime> action) => _onDraw = action;

		internal void Draw(GameTime gameTime)
		{
			if (EntitySystem != null)
			{
				EntitySystem.Draw(gameTime, SpriteBatch);
			}
		}

		public void OnUnload(Action action) => _onUnload = action;

		/// <summary>
		/// Unloads the Scene
		/// </summary>
		public void Unload()
		{
			Unloaded?.Invoke(this, new EventArgs());
			_onUnload();
		}

		public void OnPause(Action action) => _onPause = action;

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
			_onPause();
			Stopped?.Invoke(this, new EventArgs());
			_onStop();
		}

		public void OnUnpause(Action action) => _onUnpause = action;

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
				_onUnpause();
				Resumed?.Invoke(this, new EventArgs());
				_onResume();
			}
		}

		public void OnSuspend(Action action) => _onSuspend = action;

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
			_onSuspend();
			Stopped?.Invoke(this, new EventArgs());
			_onStop();
		}

		public void OnUnsuspend(Action action) => _onUnsuspend = action;

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
				_onUnsuspend();
				Resumed?.Invoke(this, new EventArgs());
				_onResume();
			}
		}

		public void OnStop(Action action) => _onStop = action;

		public void OnResume(Action action) => _onResume = action;
	}
}