using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.TileMap
{
	public class ImageLayer : IDrawableLayer
	{
		public ImageLayer()
		{

		}

		public void SetData(string source, int width, int height, int xOffset = 0, int yOffset = 0)
		{
			Source = source;
			Dimensions = new Vector2(width, height);
			Offset = new Vector2(xOffset, yOffset);
		}

		public string Source { get; private set; }

		public Vector2 Dimensions { get; private set; }

		public Vector2 Offset {get; private set;}

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