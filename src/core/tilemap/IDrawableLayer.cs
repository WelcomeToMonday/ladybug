using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Core.TileMap
{
	public interface IDrawableLayer
	{
		void Draw(TileMap tileMap, SpriteBatch spriteBatch);
	}
}