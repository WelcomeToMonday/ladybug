using System;

using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Legacy.TileMap
{
	/// <summary>
	/// Describes a layer that can be drawn directly onto a TileMap
	/// </summary>
	[Obsolete("Tilemap module is obsolete since v2.1.0. Please migrate to the Tiles module.")]
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