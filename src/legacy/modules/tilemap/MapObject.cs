using System;

using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Ladybug.Legacy.TileMap
{
	/// <summary>
	/// Represents a generic TileMap object
	/// </summary>
	[Obsolete("Tilemap module is obsolete since v2.1.0. Please migrate to the Tiles module.")]
	public class MapObject
	{
		/// <summary>
		/// Create a MapObject
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="bounds"></param>
		/// <param name="properties"></param>
		public MapObject(string name, string type, Rectangle bounds, Dictionary<string, string> properties)
		{
			Name = name;
			Type = type;
			Bounds = bounds;
			Properties = properties;
		}

		/// <summary>
		/// The MapObject's name
		/// </summary>
		/// <value></value>
		public string Name { get; set; }

		/// <summary>
		/// The MapObject's type
		/// </summary>
		/// <value></value>
		public string Type { get; set; }

		/// <summary>
		/// The MapObject's bounds
		/// </summary>
		/// <value></value>
		public Rectangle Bounds { get; set; }

		/// <summary>
		/// A Collection of the MapObject's properties
		/// </summary>
		/// <value></value>
		public Dictionary<string, string> Properties { get; private set; }

		/// <summary>
		/// Set a MapObject property value
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void SetProperty(string name, string value)
		{
			if (Properties == null)
			{
				Properties = new Dictionary<string, string>();
			}

			Properties[name] = value;
		}
	}
}