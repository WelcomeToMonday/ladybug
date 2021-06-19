using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.TileMap
{
	public interface IDrawableLayer
	{
		void Draw(TileMap tileMap, SpriteBatch spriteBatch);
	}
}