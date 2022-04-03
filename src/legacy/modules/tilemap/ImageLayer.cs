using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Legacy.TileMap
{
	/// <summary>
	/// Represents an image that can be drawn onto a tilemap as a layer
	/// </summary>
	[Obsolete("Tilemap module is obsolete since v2.1.0. Please migrate to the Tiles module.")]
	public class ImageLayer : IDrawableLayer
	{
		/// <summary>
		/// Create an ImageLayer
		/// </summary>
		public ImageLayer()
		{

		}

		/// <summary>
		/// Set the layer's image data
		/// </summary>
		/// <param name="source"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="xOffset"></param>
		/// <param name="yOffset"></param>
		public void SetData(string source, int width, int height, int xOffset = 0, int yOffset = 0)
		{
			Source = source;
			Dimensions = new Vector2(width, height);
			Offset = new Vector2(xOffset, yOffset);
		}

		/// <summary>
		/// The layer's source image key within the TileMap's content manager
		/// </summary>
		/// <value></value>
		public string Source { get; private set; }

		/// <summary>
		/// The dimensions of the image layer
		/// </summary>
		/// <value></value>
		public Vector2 Dimensions { get; private set; }

		/// <summary>
		/// The image layer's offset
		/// </summary>
		/// <value></value>
		public Vector2 Offset {get; private set;}

		/// <summary>
		/// Draw the image layer onto a tilemap
		/// </summary>
		/// <param name="tileMap"></param>
		/// <param name="spriteBatch"></param>
		public void Draw(TileMap tileMap, SpriteBatch spriteBatch)
		{
			var tex = tileMap.Content.Load<Texture2D>(Source);

			spriteBatch.Draw(
				tex,
				new Rectangle(
					(int)Offset.X,
					(int)Offset.Y,
					(int)Dimensions.X,
					(int)Dimensions.Y
				),
				null,
				Color.White
			);
		}
	}
}