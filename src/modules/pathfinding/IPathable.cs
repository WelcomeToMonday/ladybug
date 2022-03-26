using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Ladybug.Pathfinding
{
	/// <summary>
	/// Describes an object that can be used as a graph vertex node for pathfinding
	/// </summary>
	public interface IPathable
	{
		/// <summary>
		/// Retrieve a list of this node's neighbor nodes
		/// </summary>
		/// <returns></returns>
		List<IPathable> GetNeighbors();

		// note: LocalCoordinates isn't used by the library. Should it be here?
		/// <summary>
		/// The node's coordinates local to the graph
		/// </summary>
		/// <remarks>
		/// Used in generating a distance estimate between nodes
		/// </remarks>
		Vector2 LocalCoordinates {get;}

		/// <summary>
		/// Whether this node is currently traversable in the pathfinding graph
		/// </summary>
		/// <value></value>
		bool IsPathable {get;}

		/// <summary>
		/// The estimated distance value between this node and the given node
		/// </summary>
		/// <param name="otherObject"></param>
		/// <returns></returns>
		int GetDistanceEstimate(IPathable otherObject);
	}
}