using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.TileMap
{
	/// <summary>
	/// Describes a layer that can be drawn directly onto a TileMap
	/// </summary>
	public interface IDrawableLayer
	{
		/// <summary>
		/// Draw the layer onto a tilemap
		/// </summary>
		/// <param name="tileMap"></param>
		/// <param name="spriteBatch"></param>
		void Draw(TileMap tileMap, SpriteBatch spriteBatch);
	}
}