using System;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Ladybug
{
	/// <summary>
	/// Lists possible states a Scene can be in, which determine its Update and Draw behavior
	/// </summary>
	public enum SceneState
	{
		/// <summary>Active Scenes will have both <c>Update()</c> and <c>Draw()</c> called each frame.</summary>
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
		/// <summary>
		/// Asynchronous content loading has started
		/// </summary>
		public event EventHandler LoadContentAsyncStart;

		/// <summary>
		/// Asynchronous content loading has completed
		/// </summary>
		public event EventHandler LoadContentAsyncComplete;

		/// <summary>
		/// Asynchronous initialization has started
		/// </summary>
		public event EventHandler InitializeAsyncStart;

		/// <summary>
		/// Asynchronous initialization has completed
		/// </summary>
		public event EventHandler InitializeAsyncComplete;

		/// <summary>
		/// Content loading has completed
		/// </summary>
		public event EventHandler LoadContentComplete;

		/// <summary>
		/// Initialization has completed
		/// </summary>
		public event EventHandler InitializeComplete;

		/// <summary>
		/// Scene has been paused
		/// </summary>
		public event EventHandler Paused;

		/// <summary>
		/// Scene has been unpaused
		/// </summary>
		public event EventHandler Unpaused;

		/// <summary>
		/// Scene has been suspended
		/// </summary>
		public event EventHandler Suspended;

		/// <summary>
		/// Scene has been unsuspended
		/// </summary>
		public event EventHandler Unsuspended;

		/// <summary>
		/// Scene has been unloaded
		/// </summary>
		public event EventHandler Unloaded;

		/// <summary>
		/// The Scene has been paused or suspended
		/// </summary>
		public event EventHandler Stopped;

		/// <summary>
		/// The Scene has been unpaused or unsuspended
		/// </summary>
		public event EventHandler Resumed;

		/// <summary>
		/// The Scene's default SpriteBatch
		/// </summary>
		public SpriteBatch SpriteBatch { get; private set; }

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

		/// <summary>
		/// Creates a new Scene
		/// </summary>
		/// <param name="game"><see cref="Ladybug.Game"/> instance that will be managing this Scene</param>
		/// <returns></returns>
		public Scene(Game game) => SetGame(game);

		/// <summary>
		/// Creates a new Scene
		/// </summary>
		public Scene() { }

		/// <summary>
		/// Sets the <see cref="Ladybug.Game"/> object that will be managing this Scene
		/// </summary>
		/// <param name="game">Game object that will be managing this Scene</param>
		public void SetGame(Game game)
		{
			Game = game;
			Content = new ContentManager(Game.Content.ServiceProvider);
			Content.RootDirectory = "Content";
			ResourceCatalog = new ResourceCatalog(Content);
			SpriteBatch = new SpriteBatch(game.GraphicsDevice);
		}

		/// <summary>
		/// Whether this Scene has completed asynchronous initialization
		/// </summary>
		public bool InitializedAsync { get; private set; } = false;
		/// <summary>
		/// Whether this Scene has completed asynchronous content loading
		/// </summary>
		public bool ContentLoadedAsync { get; private set; } = false;

		/// <summary>
		/// Whether this Scene has completed initialization
		/// </summary>
		public bool Initialized { get; private set; } = false;
		/// <summary>
		/// Whether this Scene has completed content loading
		/// </summary>
		public bool ContentLoaded { get; private set; } = false;

		/// <summary>
		/// This Scene's resident <see cref="Ladybug.ResourceCatalog"/>
		/// </summary>
		/// <value></value>
		public ResourceCatalog ResourceCatalog { get; protected set; }

		/// <summary>
		/// Reference to the Scene's ContentManager
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// This ContentManager is intended to be accessed primarily through the Scene's 
		/// ResourceCatalog.
		/// </remarks>
		public ContentManager Content { get; private set; }

		/// <summary>
		/// Reference to the <see cref="Ladybug.Game"/> instance that is handling this scene
		/// </summary>
		public Game Game { get; protected set; }

		/// <summary>
		/// Current <see cref="Ladybug.SceneState"/> of the Scene
		/// </summary>
		public SceneState State { get; private set; } = SceneState.ACTIVE;

		/// <summary>
		/// Sets the task that is run asynchronously when the Scene's
		/// content is loaded
		/// </summary>
		/// <param name="func">Asynchronous task run upon loading the Scene's content</param>
		/// <returns>Scene</returns>
		public Scene OnLoadContentAsync(Func<Task> func)
		{
			_onLoadContentAsync = func;
			return this;
		}

		internal async void _LoadContentAsync()
		{
			ContentLoadedAsync = true;
			LoadContentAsyncStart?.Invoke(this, new EventArgs());
			await _onLoadContentAsync();
			LoadContentAsyncComplete?.Invoke(this, new EventArgs());
			ThreadManager.QueueAction(() => _LoadContent());
		}

		/// <summary>
		/// Sets the task that is run when the Scene is initialized
		/// asynchronously
		/// </summary>
		/// <param name="func">Asycronous task run upon initializing the Scene</param>
		/// <returns>Scene</returns>
		public Scene OnInitializeAsync(Func<Task> func)
		{
			_onInitializeAsync = func;
			return this;
		}

		internal async void _InitializeAsync()
		{
			InitializedAsync = true;
			InitializeAsyncStart?.Invoke(this, new EventArgs());
			await _onInitializeAsync();
			InitializeAsyncComplete?.Invoke(this, new EventArgs());
			ThreadManager.QueueAction(() => _Initialize());
		}

		/// <summary>
		/// Sets the action that is run when the Scene's content is
		/// loaded.
		/// </summary>
		/// <param name="action">Action run upon loading the Scene's content</param>
		/// <returns>Scene</returns>
		public Scene OnLoadContent(Action action)
		{
			_onLoadContent = action;
			return this;
		}

		internal void _LoadContent()
		{
			ContentLoaded = true;
			_onLoadContent();
			LoadContentComplete?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Sets the action that is run when the Scene is initialized
		/// </summary>
		/// <param name="action">Action run upon initializing the Scene</param>
		/// <returns>Scene</returns>
		public Scene OnInitialize(Action action)
		{
			_onInitialize = action;
			return this;
		}

		internal void _Initialize()
		{
			Initialized = true;
			_onInitialize();
			InitializeComplete?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Sets the action that is run every frame when the Scene
		/// is updated
		/// </summary>
		/// <param name="action">Action run upon updating the Scene</param>
		/// <returns>Scene</returns>
		public Scene OnUpdate(Action<GameTime> action)
		{
			_onUpdate = action;
			return this;
		}

		internal void _Update(GameTime gameTime)
		{
			_onUpdate(gameTime);
		}

		/// <summary>
		/// Sets the action that is run every frame when
		/// the Scene Draws to the screen
		/// </summary>
		/// <param name="action">Action run upon drawing the Scene</param>
		/// <returns>Scene</returns>
		public Scene OnDraw(Action<GameTime> action)
		{
			_onDraw = action;
			return this;
		}

		internal void _Draw(GameTime gameTime)
		{
			_onDraw(gameTime);
		}

		/// <summary>
		/// Sets the action that is run when the Scene
		/// is unloaded
		/// </summary>
		/// <param name="action">Action run upon unloading the Scene</param>
		/// <returns>Scene</returns>
		public Scene OnUnload(Action action)
		{
			_onUnload = action;
			return this;
		}

		/// <summary>
		/// Unloads the Scene, removing it from the <see cref="Ladybug.Game"/> instance that is managing it
		/// </summary>
		public void Unload()
		{
			_onUnload();
			Game.UnloadScene(this);
			Unloaded?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Sets the action that is run when the Scene is paused
		/// </summary>
		/// <param name="action">Action run upon pausing the Scene</param>
		/// <returns>Scene</returns>
		public Scene OnPause(Action action)
		{
			_onPause = action;
			return this;
		}

		/// <summary>
		/// Pauses the Scene
		/// </summary>
		/// <remarks>
		/// A paused Scene will not execute Update actions,
		/// but will still execute Draw actions
		/// </remarks>
		public void Pause()
		{
			State = SceneState.PAUSED;
			Paused?.Invoke(this, new EventArgs());
			_onPause();
			Stopped?.Invoke(this, new EventArgs());
			_onStop();
		}

		/// <summary>
		/// Sets the action that is run when the Scene is unpaused
		/// </summary>
		/// <param name="action">Action run upon unpausing the Scene</param>
		/// <returns>Scene</returns>
		public Scene OnUnpause(Action action)
		{
			_onUnpause = action;
			return this;
		}

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

		/// <summary>
		/// Sets the action that is run when the scene is suspended
		/// </summary>
		/// <param name="action">Action run upon suspending the Scene</param>
		/// <returns>Scene</returns>
		public Scene OnSuspend(Action action)
		{
			_onSuspend = action;
			return this;
		}

		/// <summary>
		/// Suspends the Scene
		/// </summary>
		/// <remarks>
		/// A suspended Scene will not execute
		/// Update or Draw actions
		/// </remarks>
		public void Suspend()
		{
			State = SceneState.SUSPENDED;
			Suspended?.Invoke(this, new EventArgs());
			_onSuspend();
			Stopped?.Invoke(this, new EventArgs());
			_onStop();
		}

		/// <summary>
		/// Sets the action that is run when the Scene is
		/// unsuspended
		/// </summary>
		/// <param name="action">Action run upon unsuspending the Scene</param>
		/// <returns>Scene</returns>
		public Scene OnUnsuspend(Action action)
		{
			_onUnsuspend = action;
			return this;
		}

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

		/// <summary>
		/// Sets the action that run when the Scene is
		/// paused or suspended
		/// </summary>
		/// <param name="action">Action run upon pausing or suspending the Scene</param>
		/// <returns>Scene</returns>
		public Scene OnStop(Action action)
		{
			_onStop = action;
			return this;
		}

		/// <summary>
		/// Sets the action that is run when the Scene is
		/// unpaused or unsuspended
		/// </summary>
		/// <param name="action"></param>
		/// <returns>Scene</returns>
		public Scene OnResume(Action action)
		{
			_onResume = action;
			return this;
		}
	}
}