using System.Xml;
using System.Collections.Generic;
using Ladybug.Graphics;

namespace Ladybug.Tiles
{
	/// <summary>
	/// A single tilemap tile
	/// </summary>
	public class Tile
	{
		internal Tile()
		{

		}

		/// <summary>
		/// Tile's custom properties
		/// </summary>
		/// <value></value>
		public Dictionary<string, string> Properties { get; internal set; }

		/// <summary>
		/// Tileset containing this tile
		/// </summary>
		/// <value></value>
		public TileSet TileSet { get; internal set; }

		/// <summary>
		/// Tile's ID within the containing tileset
		/// </summary>
		/// <value></value>
		public int ID { get; internal set; }

		/// <summary>
		/// Tile's width in pixels
		/// </summary>
		/// <value></value>
		public int Width { get; internal set; }

		/// <summary>
		/// Tile's height in pixels
		/// </summary>
		/// <value></value>
		public int Height { get; internal set; }

		/// <summary>
		/// Tile's sprite to be rendered upon drawing
		/// </summary>
		/// <value></value>
		public Sprite Sprite { get; internal set; }

		/// <summary>
		/// XmlElement containing any custom properties and sub-objects of the tile
		/// </summary>
		/// <remarks>
		/// Null if no custom tile properties have been defined for this tile
		/// </remarks>
		public XmlElement XmlElement { get; internal set; }
	}
}