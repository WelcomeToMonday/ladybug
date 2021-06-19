using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.ECS
{
	public interface IDrawableComponent
	{
		int DrawPriority {get;}

		bool Visible {get;
		}
		void Draw(GameTime gameTime, SpriteBatch spriteBatch);
	}
}