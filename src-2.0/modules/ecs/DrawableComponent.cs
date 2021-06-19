using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.ECS
{
	[Obsolete("Deprecated. Instead, derive Component and implement IDrawableComponent", false)]
	public abstract class DrawableComponent : Component, IDrawableComponent
	{	
		public virtual int DrawPriority {get;}

		public virtual bool Visible {get;}
		
		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
	}
}