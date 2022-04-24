using System.Xml;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Tiles
{
	/// <summary>
	/// Represents a layer of tilemap tiles
	/// </summary>
	public class TileLayer
	{
		/// <summary>
		/// Create a TileLayer
		/// </summary>
		public TileLayer(XmlElement node, TileMap tileMap)
		{
			TileMap = tileMap;
			XmlElement = node;

			foreach (XmlAttribute att in node.Attributes)
			{
				if (Properties.TryAdd(att.Name, att.Value))
				{
					Properties[att.Name] = att.Value;
				}
			}

			var propNode = node.SelectSingleNode("./properties");
			if (propNode != null)
			{
				foreach (XmlElement prop in propNode.SelectNodes("./property"))
				{
					Properties.TryAdd(prop.Attributes["name"].Value, prop.Attributes["value"].Value);
					BuildCustomProperty(prop);
				}
			}

			Initialize();
		}

		/// <summary>
		/// TileMap containing this TileLayer
		/// </summary>
		public TileMap TileMap { get; private set; }

		/// <summary>
		/// The XmlElement describing this TileLayer
		/// </summary>
		/// <value></value>
		public XmlElement XmlElement { get; private set; }

		/// <summary>
		/// Tile data
		/// </summary>
		//public int[,] Tiles { get; private set; }
		public Tile[,] Tiles { get; private set; }

		/// <summary>
		/// TileLayer properties
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, string> Properties = new Dictionary<string, string>();

		/// <summary>
		/// Initialize the TileLayer
		/// </summary>
		protected virtual void Initialize()
		{
			int.TryParse(XmlElement.Attributes["width"].Value, out int width);
			int.TryParse(XmlElement.Attributes["height"].Value, out int height);

			//Tiles = new int[width, height];
			Tiles = new Tile[width, height];
			var dataNode = XmlElement.SelectSingleNode("./data") as XmlElement;
			var tileList = dataNode.InnerText.Trim('\n').Split(',');
			var tileIndex = 0;
			for (var row = 0; row < height; row++)
			{
				for (var col = 0; col < width; col++)
				{
					//int.TryParse(tileList[tileIndex], out Tiles[col, row]);
					int.TryParse(tileList[tileIndex], out int tileId);
					tileId--; //correct for tiled adding 1 to tile IDs
					if (tileId >= 0)
					{
						var tileSet = TileMap.FindTileSet(tileId);
						Tiles[col, row] = tileSet[tileId - tileSet.FirstGID];
					}
					else
					{
						Tiles[col, row] = null;
					}

					tileIndex++;
				}
			}
		}

		/// <summary>
		/// Handle the processing of the TileLayer's custom properties
		/// </summary>
		/// <param name="node"></param>
		protected virtual void BuildCustomProperty(XmlElement node)
		{
			// To be overridden by derived classes
		}

		/// <summary>
		/// Renders the TileLayer using the provided SpriteBatch
		/// </summary>
		/// <param name="spriteBatch"></param>
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			for (var row = 0; row < TileMap.Height; row++)
			{
				for (var col = 0; col < TileMap.Width; col++)
				{
					if (Tiles[col, row] != null)
					{
						var tile = Tiles[col, row];
						var pos = GetTilePosition(col, row);
						spriteBatch.Draw(
							tile.Sprite.Texture,
							new Rectangle(
								(int)pos.X,
								(int)pos.Y,
								tile.Width,
								tile.Height
							),
							tile.Sprite.Frame,
							Color.White
						);
					}
					// Tiled adds +1 to Tile IDs so 0 can represent empty fields.
					// Since we're using the ID to determine position on the tileset
					// sprite atlas, we're going to subtract one so the math checks out.
					/*
					var tileID = Tiles[col, row] - 1;
					if (tileID >= 0)
					{
						var tileSet = TileMap.FindTileSet(tileID);
						var tile = tileSet[tileID - (tileSet.FirstGID)];
						var pos = GetTilePosition(col, row);

						spriteBatch.Draw(
							tile.Sprite.Texture,
							new Rectangle(
								(int)pos.X,
								(int)pos.Y,
								tileSet.TileWidth,
								tileSet.TileHeight
							),
							tile.Sprite.Frame,
							Color.White
						);
					}
					*/
				}
			}
		}

		private Vector2 GetTilePosition(int col, int row)
		{
			var res = Vector2.Zero;

			switch (TileMap.Orientation)
			{
				default:
				case Orientation.Orthogonal:
					res = new Vector2(
						TileMap.TileWidth * col,
						TileMap.TileHeight * row
					);
					break;
				case Orientation.Isometric:
					res = new Vector2(TileMap.Height * (TileMap.TileWidth / 2), 0) - new Vector2(TileMap.TileWidth / 2, 0) //origin point
					+ new Vector2(-row * (TileMap.TileWidth/2), row * (TileMap.TileHeight/2)) // row offset
					+ new Vector2(col * TileMap.TileWidth / 2, col * TileMap.TileHeight / 2); // col offset
					break;
			}

			return res;
		}
	}
}