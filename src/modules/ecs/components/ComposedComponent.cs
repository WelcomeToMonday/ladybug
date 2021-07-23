#pragma warning disable 1591 // Hide XMLdoc warnings.

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.ECS.Components
{
	/// <summary>
	/// ComponentSystem responsible for processing <see cref="Ladybug.ECS.Components.ComposedComponent"/> Components
	/// </summary>
	public class ComposedComponentSystem : ComponentSystem<ComposedComponent> { }

	/// <summary>
	/// Inline-composable ECS Component
	/// </summary>
	public sealed class ComposedComponent : Component
	{
		private Action _onInitialize = () => { };

		private Action<GameTime> _onPreUpdate = (GameTime gameTime) => { };
		private Action<GameTime> _onUpdate = (GameTime gameTime) => { };
		private Action<GameTime> _onPostUpdate = (GameTime gameTime) => { };

		private Action<GameTime, SpriteBatch> _onPreDraw = (GameTime gameTime, SpriteBatch spriteBatch) => { };
		private Action<GameTime, SpriteBatch> _onDraw = (GameTime gameTime, SpriteBatch spriteBatch) => { };
		private Action<GameTime, SpriteBatch> _onPostDraw = (GameTime gameTime, SpriteBatch spriteBatch) => { };

		internal ComposedComponent() { }

		/// <summary>
		/// Sets this Component's Initialize behavior
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedComponent OnInitialize(Action action)
		{
			_onInitialize = action;
			return this;
		}
		protected override void Initialize()
		{
			_onInitialize();
		}

		/// <summary>
		/// Sets this Component's PreUpdate behavior
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedComponent OnPreUpdate(Action<GameTime> action)
		{
			_onPreUpdate = action;
			return this;
		}
		protected override void PreUpdate(GameTime gameTime)
		{
			_onPreUpdate(gameTime);
		}

		/// <summary>
		/// Sets this Component's Update behavior
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedComponent OnUpdate(Action<GameTime> action)
		{
			_onUpdate = action;
			return this;
		}
		protected override void Update(GameTime gameTime)
		{
			_onUpdate(gameTime);
		}

		/// <summary>
		/// Sets this Component's PostUpdate behavior
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedComponent OnPostUpdate(Action<GameTime> action)
		{
			_onPostUpdate = action;
			return this;
		}
		protected override void PostUpdate(GameTime gameTime)
		{
			_onPostUpdate(gameTime);
		}

		/// <summary>
		/// Sets this Component's PreDraw behavior
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedComponent OnPreDraw(Action<GameTime, SpriteBatch> action)
		{
			_onPreDraw = action;
			return this;
		}
		protected override void PreDraw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			_onPreDraw(gameTime, spriteBatch);
		}

		/// <summary>
		/// Sets this Component's Draw behavior
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedComponent OnDraw(Action<GameTime, SpriteBatch> action)
		{
			_onDraw = action;
			return this;
		}
		protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			_onDraw(gameTime, spriteBatch);
		}

		/// <summary>
		/// Sets this Component's PostDraw behavior
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public ComposedComponent OnPostDraw(Action<GameTime, SpriteBatch> action)
		{
			_onPostDraw = action;
			return this;
		}
		protected override void PostDraw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			_onPostDraw(gameTime, spriteBatch);
		}
	}
}
#pragma warning restore 1591