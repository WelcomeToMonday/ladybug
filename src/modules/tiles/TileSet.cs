using System.Collections.Generic;
using System.IO;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

using IPath = System.IO.Path;

namespace Ladybug.Tiles
{
	/// <summary>
	/// Represents a collection of tiles
	/// </summary>
	public class TileSet
	{
		/// <summary>
		/// Create a TileSet
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="tileMap"></param>
		public TileSet(string filePath, TileMap tileMap)
		{
			Path = filePath;
			var xml = tileMap.Content.Load<XmlDocument>(Path);
			XmlDocument = xml;
			TileMap = tileMap;
			var root = xml.SelectSingleNode("/tileset") as XmlElement;
			BuildTileSet(root);
		}

		/// <summary>
		/// Access this TileSet's SpriteAtlas content by index
		/// </summary>
		public Sprite this[int i] => SpriteAtlas[i];

		/// <summary>
		/// Access this TileSet's SpriteAtlas content by coordinate
		/// </summary>
		public Sprite this[int i, int j] => SpriteAtlas[i, j];

		/// <summary>
		/// The ID of the first Tile in this TileSet
		/// </summary>
		/// <remarks>
		/// This is used by the TileMap renderer to find which tileset
		/// a particular tile belongs to.
		/// </remarks>
		public int FirstGID { get; set; }

		/// <summary>
		/// Range of Tile IDs within this TileSet
		/// </summary>
		/// <returns></returns>
		public Vector2 TileRange => new Vector2(FirstGID, (TileCount + FirstGID) - 1);

		/// <summary>
		/// The Content path to this TileSet
		/// </summary>
		/// <value></value>
		public string Path { get; private set; }

		/// <summary>
		/// The XmlDocument which describes this TileSet
		/// </summary>
		/// <value></value>
		public XmlDocument XmlDocument { get; private set; }

		/// <summary>
		/// The TileMap which is manaing this TileSet
		/// </summary>
		/// <value></value>
		public TileMap TileMap { get; private set; }

		/// <summary>
		/// The SpriteAtlas which contains this TileSet's
		/// texture assets
		/// </summary>
		/// <value></value>
		public SpriteAtlas SpriteAtlas { get; private set; }

		/// <summary>
		/// This TileSet's properites
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, string> Properties { get; private set; } = new Dictionary<string, string>();

		/// <summary>
		/// The width of this TileSet's individual tiles, in pixels
		/// </summary>
		/// <returns></returns>
		public int TileWidth => int.Parse(Properties["tilewidth"]);

		/// <summary>
		/// The height of this TileSet's individual tiles, in pixels
		/// </summary>
		/// <returns></returns>
		public int TileHeight => int.Parse(Properties["tileheight"]);

		/// <summary>
		/// How many tiles this TileSet contains
		/// </summary>
		/// <returns></returns>
		public int TileCount => int.Parse(Properties["tilecount"]);

		/// <summary>
		/// How many columns of tiles are in this TileSet
		/// </summary>
		/// <returns></returns>
		public int ColumnCount => int.Parse(Properties["columns"]);

		/// <summary>
		/// How many rows of tiles are in this TileSet
		/// </summary>
		public int RowCount => TileCount / ColumnCount;

		/// <summary>
		/// The spacing between each tile in this TileSet, in pixels
		/// </summary>
		/// <value></value>
		public int Spacing
		{
			get
			{
				var res = 0;
				if (Properties.TryGetValue("spacing", out string value))
				{
					if (int.TryParse(value, out int vres))
					{
						res = vres;
					}
				}
				return res;
			}
		}

		private void BuildTileSet(XmlElement node)
		{
			foreach (XmlAttribute att in node.Attributes)
			{
				if (!Properties.TryAdd(att.Name, att.Value))
				{
					Properties[att.Name] = att.Value;
				}
			}

			// todo: Check if tilesets can have multiple images.
			// If so, we'll need to adjust this to accommodate.
			var image = node.SelectSingleNode("./image") as XmlElement;
			var imagePath = IPath.ChangeExtension(
				IPath.Combine(
					IPath.GetDirectoryName(Path),
					image.Attributes["source"].Value
					),
				null
			);

			BuildAtlas(imagePath);
		}

		private void BuildAtlas(string imagePath)
		{
			var tex = TileMap.Content.Load<Texture2D>(imagePath);

			// todo: atlases don't have spacing support yet!
			// tilesets with spacing set won't look right until
			// we get this addressed.
			SpriteAtlas = new SpriteAtlas(tex, ColumnCount, RowCount);
		}
	}
}