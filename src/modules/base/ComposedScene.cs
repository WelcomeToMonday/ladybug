#pragma warning disable 1591 // Hide XMLdoc warnings.
using System;

using Microsoft.Xna.Framework;

using Ladybug.UserInput;

namespace Ladybug
{
	/// <summary>
	/// An inline-composable Scene
	/// </summary>
	public sealed class ComposedScene : Scene
	{
		private Action _onLoadContentAsync = () => { };
		private Action _onInitializeAsync = () => { };

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

		internal ComposedScene(Game game) : base(game) { }
		internal ComposedScene() : base() { }

		/// <summary>
		/// Sets the task that is run asynchronously when the Scene's
		/// content is loaded
		/// </summary>
		/// <param name="action">Asynchronous action run upon loading the Scene's content</param>
		/// <returns>Scene</returns>
		public ComposedScene OnLoadContentAsync(Action action)
		{
			_onLoadContentAsync = action;
			return this;
		}
		protected override void LoadContentAsync() => _onLoadContentAsync();

		/// <summary>
		/// Sets the task that is run when the Scene is initialized
		/// asynchronously
		/// </summary>
		/// <param name="action">Asycronous action run upon initializing the Scene</param>
		/// <returns>Scene</returns>
		public ComposedScene OnInitializeAsync(Action action)
		{
			_onInitializeAsync = action;
			return this;
		}
		protected override void InitializeAsync() => _onInitializeAsync();

		/// <summary>
		/// Sets the action that is run when the Scene's content is
		/// loaded.
		/// </summary>
		/// <param name="action">Action run upon loading the Scene's content</param>
		/// <returns>Scene</returns>
		public ComposedScene OnLoadContent(Action action)
		{
			_onLoadContent = action;
			return this;
		}
		protected override void LoadContent() => _onLoadContent();

		/// <summary>
		/// Sets the action that is run when the Scene is initialized
		/// </summary>
		/// <param name="action">Action run upon initializing the Scene</param>
		/// <returns>Scene</returns>
		public ComposedScene OnInitialize(Action action)
		{
			_onInitialize = action;
			return this;
		}
		protected override void Initialize() => _onInitialize();

		/// <summary>
		/// Sets the action that is run every frame when the Scene
		/// is updated
		/// </summary>
		/// <param name="action">Action run upon updating the Scene</param>
		/// <returns>Scene</returns>
		public ComposedScene OnUpdate(Action<GameTime> action)
		{
			_onUpdate = action;
			return this;
		}
		protected override void Update(GameTime gameTime) => _onUpdate(gameTime);

		/// <summary>
		/// Sets the action that is run upon the given input action
		/// </summary>
		/// <param name="name"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedScene OnInputAction(string name, Action<InputActionEventArgs> action)
		{
			SetInputAction(name, action);
			return this;
		}

		/// <summary>
		/// Sets the action that is run every frame when
		/// the Scene Draws to the screen
		/// </summary>
		/// <param name="action">Action run upon drawing the Scene</param>
		/// <returns>Scene</returns>
		public ComposedScene OnDraw(Action<GameTime> action)
		{
			_onDraw = action;
			return this;
		}
		protected override void Draw(GameTime gameTime) => _onDraw(gameTime);

		/// <summary>
		/// Sets the action that is run when the Scene
		/// is unloaded
		/// </summary>
		/// <param name="action">Action run upon unloading the Scene</param>
		/// <returns>Scene</returns>
		public ComposedScene OnUnload(Action action)
		{
			_onUnload = action;
			return this;
		}
		protected override void Unload() => _onUnload();

		/// <summary>
		/// Sets the action that is run when the Scene is paused
		/// </summary>
		/// <param name="action">Action run upon pausing the Scene</param>
		/// <returns>Scene</returns>
		public ComposedScene OnPause(Action action)
		{
			_onPause = action;
			return this;
		}
		protected override void Pause() => _onPause();

		/// <summary>
		/// Sets the action that is run when the Scene is unpaused
		/// </summary>
		/// <param name="action">Action run upon unpausing the Scene</param>
		/// <returns>Scene</returns>
		public ComposedScene OnUnpause(Action action)
		{
			_onUnpause = action;
			return this;
		}
		protected override void Unpause() => _onUnpause();

		/// <summary>
		/// Sets the action that is run when the scene is suspended
		/// </summary>
		/// <param name="action">Action run upon suspending the Scene</param>
		/// <returns>Scene</returns>
		public ComposedScene OnSuspend(Action action)
		{
			_onSuspend = action;
			return this;
		}
		protected override void Suspend() => _onSuspend();

		/// <summary>
		/// Sets the action that is run when the Scene is
		/// unsuspended
		/// </summary>
		/// <param name="action">Action run upon unsuspending the Scene</param>
		/// <returns>Scene</returns>
		public ComposedScene OnUnsuspend(Action action)
		{
			_onUnsuspend = action;
			return this;
		}
		protected override void Unsuspend() => _onUnsuspend();

		/// <summary>
		/// Sets the action that run when the Scene is
		/// paused or suspended
		/// </summary>
		/// <param name="action">Action run upon pausing or suspending the Scene</param>
		/// <returns>Scene</returns>
		public ComposedScene OnStop(Action action)
		{
			_onStop = action;
			return this;
		}
		protected override void Stop() => _onStop();

				/// <summary>
		/// Sets the action that is run when the Scene is
		/// unpaused or unsuspended
		/// </summary>
		/// <param name="action"></param>
		/// <returns>Scene</returns>
		public ComposedScene OnResume(Action action)
		{
			_onResume = action;
			return this;
		}
		protected override void Resume() => _onResume();
	}
}
#pragma warning restore 1591