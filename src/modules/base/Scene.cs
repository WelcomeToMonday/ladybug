using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Ladybug.UserInput;

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
	public abstract class Scene
	{
		#region Events
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
		#endregion // Events

		private Dictionary<string, Action<InputActionEventArgs>> _inputActions = new Dictionary<string, Action<InputActionEventArgs>>();

		/// <summary>
		/// Begins inline composition of a new Scene
		/// </summary>
		/// <returns></returns>
		public static ComposedScene Compose() => new ComposedScene();

		/// <summary>
		/// Begins inline composition of a new Scene
		/// </summary>
		/// <param name="game"></param>
		/// <returns></returns>
		public static ComposedScene Compose(Game game) => new ComposedScene(game);

		/// <summary>
		/// Creates a new Scene
		/// </summary>
		/// <param name="game"><see cref="Ladybug.Game"/> instance that will be managing this Scene</param>
		/// <returns></returns>
		public Scene(Game game) : this() => SetGame(game);

		/// <summary>
		/// Creates a new Scene
		/// </summary>
		public Scene()
		{
			Input.Action += _InputAction;
		}

		/// <summary>
		/// Contains services registered with this Scene
		/// </summary>
		/// <value></value>
		public GameServiceContainer Services { get; private set; }

		/// <summary>
		/// The Scene's default SpriteBatch
		/// </summary>
		public SpriteBatch SpriteBatch { get; private set; }

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
			Services = new GameServiceContainer();
			Services.AddService(ResourceCatalog);
		}

		/// <summary>
		/// Run when content is loaded asynchronously
		/// </summary>
		protected virtual void LoadContentAsync() { }
		internal async void _LoadContentAsync()
		{
			LoadContentAsyncStart?.Invoke(this, new EventArgs());
			await Task.Run(() => LoadContentAsync());
			ContentLoadedAsync = true;
			LoadContentAsyncComplete?.Invoke(this, new EventArgs());
			ThreadManager.QueueAction(() => _LoadContent());
		}

		/// <summary>
		/// Run when the Scene is initialized asynchronously
		/// </summary>
		protected virtual void InitializeAsync() { }
		internal async void _InitializeAsync()
		{
			InitializeAsyncStart?.Invoke(this, new EventArgs());
			await Task.Run(() => InitializeAsync());
			InitializedAsync = true;
			InitializeAsyncComplete?.Invoke(this, new EventArgs());
			ThreadManager.QueueAction(() => _Initialize());
		}

		/// <summary>
		/// Run when content is loaded into the Scene
		/// </summary>
		protected virtual void LoadContent() { }
		internal void _LoadContent()
		{
			LoadContent();
			ContentLoaded = true;
			LoadContentComplete?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Run when the Scene is Initialized
		/// </summary>
		protected virtual void Initialize() { }
		internal void _Initialize()
		{
			Initialize();
			Initialized = true;
			InitializeComplete?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Run each frame when the Scene is updated
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void Update(GameTime gameTime) { }
		internal void _Update(GameTime gameTime)
		{
			Update(gameTime);
		}

		/// <summary>
		/// Sets the action to be performed upon a specific input action
		/// </summary>
		/// <param name="name"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		protected void SetInputAction(string name, Action<InputActionEventArgs> action)
		{
			if (!_inputActions.TryAdd(name, action))
			{
				_inputActions[name] = action;
			}
		}
		private void _InputAction(object sender, InputActionEventArgs e)
		{
			if (_inputActions.ContainsKey(e.Name))
			{
				_inputActions[e.Name](e);
			}
		}

		/// <summary>
		/// Run when the Scene is drawn
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void Draw(GameTime gameTime) { }
		internal void _Draw(GameTime gameTime)
		{
			Draw(gameTime);
		}

		/// <summary>
		/// Unloads the Scene, removing it from the <see cref="Ladybug.Game"/> instance that is managing it
		/// </summary>
		public void UnloadScene()
		{
			Unload();
			Game.UnloadScene(this);
			Unloaded?.Invoke(this, new EventArgs());
		}
		/// <summary>
		/// Run when the Scene is unloaded
		/// </summary>
		/// <remarks>
		/// To unload the scene, use <see cref="UnloadScene"/> instead
		/// </remarks>
		protected virtual void Unload() { }

		/// <summary>
		/// Pauses the Scene
		/// </summary>
		/// <remarks>
		/// A paused Scene will not execute Update actions,
		/// but will still execute Draw actions
		/// </remarks>
		public void PauseScene()
		{
			State = SceneState.PAUSED;
			Paused?.Invoke(this, new EventArgs());
			Pause();
			Stopped?.Invoke(this, new EventArgs());
			Stop();
		}
		/// <summary>
		/// Run when the Scene is paused
		/// </summary>
		/// <remarks>
		/// To pause the scene, use <see cref="PauseScene"/> instead
		/// </remarks>
		protected virtual void Pause() { }

		/// <summary>
		/// Unpauses the Scene
		/// </summary>
		/// <remarks>
		/// Returns the Scene to ACTIVE state,
		/// only if state was previously PAUSED
		/// </remarks>
		public void UnpauseScene()
		{
			if (State == SceneState.PAUSED)
			{
				State = SceneState.ACTIVE;
				Unpaused?.Invoke(this, new EventArgs());
				Unpause();
				Resumed?.Invoke(this, new EventArgs());
				Resume();
			}
		}
		/// <summary>
		/// Run when the Scene is unpaused
		/// </summary>
		/// <remarks>
		/// To unpause the scene, use <see cref="UnpauseScene"/> instead
		/// </remarks>
		protected virtual void Unpause() { }

		/// <summary>
		/// Suspends the Scene
		/// </summary>
		/// <remarks>
		/// A suspended Scene will not execute
		/// Update or Draw actions
		/// </remarks>
		public void SuspendScene()
		{
			State = SceneState.SUSPENDED;
			Suspended?.Invoke(this, new EventArgs());
			Suspend();
			Stopped?.Invoke(this, new EventArgs());
			Stop();
		}
		/// <summary>
		/// Run when the scene is suspended
		/// </summary>
		/// <remarks>
		/// To suspend the scene, use <see cref="SuspendScene"/> instead
		/// </remarks>
		protected virtual void Suspend() { }

		/// <summary>
		/// Unsuspends the Scene
		/// </summary>
		/// <remarks>
		/// Returns the Scene to ACTIVE state,
		/// only if state was previously SUSPENDED
		/// </remarks>
		public void UnsuspendScene()
		{
			if (State == SceneState.SUSPENDED)
			{
				State = SceneState.ACTIVE;
				Unsuspended?.Invoke(this, new EventArgs());
				Unsuspend();
				Resumed?.Invoke(this, new EventArgs());
				Resume();
			}
		}
		/// <summary>
		/// Run when the scene is unsuspended
		/// </summary>
		/// <remarks>
		/// To unsuspend the scene, use <see cref="UnsuspendScene"/> instead
		/// </remarks>
		protected virtual void Unsuspend() { }

		/// <summary>
		/// Run when the scene is paused or suspended
		/// </summary>
		/// <remarks>
		/// To stop the scene, use <see cref="PauseScene"/> or <see cref="SuspendScene"/> instead
		/// </remarks>
		protected virtual void Stop() { }

		/// <summary>
		/// Run when the scene is unpaused or unsuspended
		/// </summary>
		/// <remarks>
		/// To resum the scene, use <see cref="UnpauseScene"/> or <see cref="UnsuspendScene"/> instead
		/// </remarks>
		protected virtual void Resume() { }
	}
}