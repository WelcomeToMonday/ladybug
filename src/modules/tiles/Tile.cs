using System.Xml;

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