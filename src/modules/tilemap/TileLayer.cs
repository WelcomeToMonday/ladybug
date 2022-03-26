using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.TileMap
{
	/// <summary>
	/// Represents a layer of tilemap tiles
	/// </summary>
	public class TileLayer : IDrawableLayer
	{
		/// <summary>
		/// Create a TileLayer
		/// </summary>
		public TileLayer()
		{

		}

		/// <summary>
		/// Create a TileLayer
		/// </summary>
		/// <param name="data"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public TileLayer(string data, int width, int height)
		{
			SetData(data, width, height);
		}
		
		/// <summary>
		/// The TileLayer's data, organized as a two-dimensional array of Tile IDs
		/// </summary>
		/// <value></value>
		public int[,] Data { get; private set; }

		/// <summary>
		/// Set the TileLayer's content data
		/// </summary>
		/// <param name="data"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
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

		/// <summary>
		/// Draws the TileLayer onto a TileMap
		/// </summary>
		/// <param name="tileMap"></param>
		/// <param name="spriteBatch"></param>
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
							var pos = GetTilePosition(tileMap, col, row);
							spriteBatch.Draw
							(
								tile.Texture,
								new Rectangle(
									(int)pos.X,
									(int)pos.Y,
									tileSet.TileWidth,
									tileSet.TileHeight
									),
								tile.Frame,
								Color.White
							);
						}
				}
			}
		}

		private Vector2 GetTilePosition(TileMap tileMap, int col, int row)
		{
			var res = Vector2.Zero;

			switch (tileMap.Orientation)
			{
				default:
				case TileMap.TileOrientation.Orthographic:
					res = new Vector2(
						tileMap.TileWidth * col,
						tileMap.TileHeight * row
					);
					break;
				case TileMap.TileOrientation.Isometric:
					res = new Vector2(tileMap.Height * (tileMap.TileWidth / 2), 0) - new Vector2(tileMap.TileWidth / 2, 0) //origin point
					+ new Vector2(-row * (tileMap.TileWidth/2), row * (tileMap.TileHeight/2)) // row offset
					+ new Vector2(col * tileMap.TileWidth / 2, col * tileMap.TileHeight / 2); // col offset
					break;
			}
			
			return res;
		}
	}
}