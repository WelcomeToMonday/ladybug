using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.ECS.Components;

namespace Ladybug.ECS
{
	/// <summary>
	/// A Component that can be registered and managed
	/// by an <see cref="Ladybug.ECS.ECS"/> instance
	/// </summary>
	public abstract class Component
	{
		private int _drawPriority = 0;

		/// <summary>
		/// Name of the Component instance
		/// </summary>
		/// <value></value>
		public string Name { get; set; }

		/// <summary>
		/// <see cref="Ladybug.ECS.Entity"/> that contains this Component
		/// </summary>
		/// <value></value>
		public Entity Entity { get; internal set; }

		/// <summary>
		/// <see cref="Ladybug.ECS.ECS"/> that is managing this Component
		/// </summary>
		/// <value></value>
		public ECS ECS { get => Entity.ECS; }

		/// <summary>
		/// <see cref="Ladybug.Scene"/> that contains the <see cref="Ladybug.ECS.ECS"/> that
		/// manages this Component
		/// </summary>
		/// <value></value>
		public Scene Scene { get => ECS.Scene; }

		/// <summary>
		/// <see cref="Ladybug.Game"/> instance that is managing the
		/// <see cref="Ladybug.Scene"/> that contains the 
		/// <see cref="Ladybug.ECS.Entity"/> managing this Component
		/// </summary>
		/// <value></value>
		public Game Game { get => Scene.Game; }

		/// <summary>
		/// Whether the Component is active
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// By default, Active is used to
		/// determine whether to update this
		/// Component. This behavior can be changed
		/// by overriding <see cref="CheckRunUpdate()"/>
		/// </remarks>
		public bool Active { get; set; } = true;

		/// <summary>
		/// Whether the Component is visible
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// By default, Visible is used to
		/// determine whether to draw this
		/// Component. This behavior can be changed
		/// by overriding <see cref="CheckRunDraw()"/>
		/// </remarks>
		public bool Visible { get; set; } = true;

		/// <summary>
		/// The Draw Priority of this Component
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// Components with a smaller value in DrawPriority
		/// are drawn earlier than those with a larger value
		/// </remarks>
		public int DrawPriority
		{
			get => _drawPriority;
			set
			{
				if (_drawPriority != value)
				{
					_drawPriority = value;
					ECS.RequestDrawSort(this);
				}
			}
		}
		
		/// <summary>
		/// Whether this Component has been initialized
		/// </summary>
		/// <value></value>
		public bool Initialized { get; private set; } = false;

		/// <summary>
		/// Begin inline composition of a Component
		/// </summary>
		/// <returns></returns>
		public static ComposedComponent Compose() => new ComposedComponent();

		/// <summary>
		/// Determines whether to Update this component each frame
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// By default, checks that Active and Entity.Active are true
		/// </remarks>
		protected virtual bool CheckRunUpdate() => Entity.Active && Active;

		/// <summary>
		/// Determines whether to Draw this component each frame
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// By default, checks that <see cref="Visible"/> and <see cref="Entity.Visible"/> are true
		/// </remarks>
		protected virtual bool CheckRunDraw() => Entity.Visible && Visible;

		/// <summary>
		/// Run when Component is initialized
		/// </summary>
		protected virtual void Initialize() { }
		internal void _Initialize()
		{
			if (!Initialized)
			{
				Initialize();
			}
		}

		/// <summary>
		/// Run immediately before Update
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void PreUpdate(GameTime gameTime) { }
		internal void _PreUpdate(GameTime gameTime)
		{
			if (CheckRunUpdate())
			{
				PreUpdate(gameTime);
			}
		}

		/// <summary>
		/// Run when this component is Updated by the ECS
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void Update(GameTime gameTime) { }
		internal void _Update(GameTime gameTime)
		{
			if (CheckRunUpdate())
			{
				Update(gameTime);
			}
		}

		/// <summary>
		/// Run immediately after Update
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void PostUpdate(GameTime gameTime) { }
		internal void _PostUpdate(GameTime gameTime)
		{
			if (CheckRunUpdate())
			{
				PostUpdate(gameTime);
			}
		}

		/// <summary>
		/// Run immediately before Draw
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		protected virtual void PreDraw(GameTime gameTime, SpriteBatch spriteBatch) { }
		internal void _PreDraw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (CheckRunUpdate())
			{
				PreDraw(gameTime, spriteBatch);
			}
		}

		/// <summary>
		/// Run when the ECS Draws this Component
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		protected virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
		internal void _Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (CheckRunUpdate())
			{
				Draw(gameTime, spriteBatch);
			}
		}

		/// <summary>
		/// Run immediately after Draw
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		protected virtual void PostDraw(GameTime gameTime, SpriteBatch spriteBatch) { }
		internal void _PostDraw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (CheckRunUpdate())
			{
				PostDraw(gameTime, spriteBatch);
			}
		}
	}
}