using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Ladybug.Core;

namespace Ladybug.Core.TileMap
{
	public class LoadZone : MapObject, ICollision
	{
		public LoadZone(string name, Rectangle bounds, Dictionary<string, string> properties) : base(name, "loadzone", bounds, properties)
		{
			var s = name.Split('.');
			MapName = s[0];
			SpawnPointName = s[1];
		}
		public Rectangle CollisionBounds { get => Bounds; }
		public string MapName {get;}
		public string SpawnPointName {get;}
	}
}