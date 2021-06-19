using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Ladybug.Pathfinding
{
	public interface IPathable
	{
		List<IPathable> GetNeighbors();
		Vector2 LocalCoordinates {get;}
		bool IsPathable {get;}
		int GetDistanceEstimate(IPathable otherObject);
	}
}