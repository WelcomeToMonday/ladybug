using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Ladybug.TileMap
{
	public class MapObject
	{
		public MapObject(string name, string type, Rectangle bounds, Dictionary<string, string> properties)
		{
			Name = name;
			Type = type;
			Bounds = bounds;
			Properties = properties;
		}

		public string Name { get; set; }
		public string Type { get; set; }
		public Rectangle Bounds { get; set; }
		public Dictionary<string, string> Properties { get; private set; }

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