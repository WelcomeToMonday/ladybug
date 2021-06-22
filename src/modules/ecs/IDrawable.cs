using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.ECS
{
	[Obsolete("Ladybug.ECS is being deprecated upon 2.0 release. Use Ladybug.Entities instead.", false)]
	public interface IDrawableComponent
	{
		int DrawPriority {get;}

		bool Visible {get;
		}
		void Draw(GameTime gameTime, SpriteBatch spriteBatch);
	}
}