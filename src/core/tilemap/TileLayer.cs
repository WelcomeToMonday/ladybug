using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Core.TileMap
{
	public class TileLayer : IDrawableLayer
	{
		public TileLayer()
		{

		}
		public TileLayer(string data, int width, int height)
		{
			SetData(data, width, height);
		}
		
		public int[,] Data { get; private set; }

		public void SetData(string data, int width, int height)
		{
			Data = new int[width, height];
			var dataList = data.Trim('\n').Split(',');
			int dataIndex = 0;
			for (var row = 0; row < height; row++)
			{
				for (var col = 0; col < width; col++)
				{
					int.TryParse(dataList[dataIndex], out Data[col, row]);
					dataIndex++;
				}
			}
		}

		public void Draw(TileMap tileMap, SpriteBatch spriteBatch)
		{
			for (var row = 0; row < tileMap.Height; row++)
			{
				for (var col = 0; col < tileMap.Width; col++)
				{
						// Tiled adds +1 to Tile IDs so 0 can represent empty fields.
						// Since we're using the ID to determine position on the image file,
						// We're going to subtract one so the math checks out.
						var tileID = Data[col, row] - 1;
						if (tileID >= 0)
						{
							var tileSet = tileMap.FindTileSet(tileID);
							var tile = tileSet[tileID - (tileSet.FirstGID)];
							spriteBatch.Draw
							(
								tile.Texture,
								new Rectangle(
									tileMap.TileWidth * col,
									tileMap.TileHeight * row,
									tileMap.TileWidth,
									tileMap.TileHeight
									),
								tile.Frame,
								Color.White
							);
						}
				}
			}
		}
	}
}