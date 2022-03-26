using System.Collections.Generic;
using System.Linq;

namespace Ladybug.Pathfinding
{
	/// <summary>
	/// Represents a collection of pathfinding graph nodes
	/// </summary>
	public class NodeMap
	{
		/// <summary>
		/// List of pathable node objects within this map
		/// </summary>
		/// <returns></returns>
		public List<IPathable> Nodes { get; private set; } = new List<IPathable>();

		// note: Not used by library. Necessary?
		/// <summary>
		/// A collection of saved paths within this map
		/// </summary>
		/// <value></value>
		public Dictionary<string, NodePath> SavedPaths { get; private set; }

		/// <summary>
		/// Add one or more nodes to the map
		/// </summary>
		/// <param name="nodes"></param>
		public void AddNodes(params IPathable[] nodes)
		{
			foreach (var n in nodes)
			{
				Nodes.Add(n);
			}
		}

		/// <summary>
		/// Add one or more nodes to the map
		/// </summary>
		/// <param name="nodes"></param>
		public void AddNodes(List<IPathable> nodes)
		{
			foreach (var n in nodes)
			{
				Nodes.Add(n);
			}
		}

		/// <summary>
		/// Find a path between two nones
		/// </summary>
		/// <param name="startNode"></param>
		/// <param name="endNode"></param>
		/// <returns></returns>
		public NodePath Find(IPathable startNode, IPathable endNode)
		{
			NodePath path = null;
			List<Node> openList = new List<Node>();
			List<Node> closedList = new List<Node>();
			Node currentNode = null;
			int currentGScore = 0;

			openList.Add(new Node(currentGScore,0,startNode));

			while (openList.Count > 0)
			{
				var lowest = openList.Min(n => n.F);
				currentNode = openList.First(n => n.F == lowest);

				closedList.Add(currentNode);
				openList.Remove(currentNode);
				

				if (closedList.FirstOrDefault(n => n.NodeObject == endNode) != null)
				{
					break; // Target is in closed list; Path has been found.
				}

				var neighbors = currentNode.NodeObject.GetNeighbors();
				currentGScore++;

				foreach (var neighbor in neighbors)
				{
					if (closedList.FirstOrDefault(n => n.NodeObject == neighbor) != null)
					{
						continue; // Neighbor is in closed list, so ignore.
					}

					if (!neighbor.IsPathable)
					{
						continue;
					}

					var newNode = new Node(
						currentGScore,
						neighbor.GetDistanceEstimate(endNode),
						neighbor,
						currentNode
						);

					if (openList.FirstOrDefault(n => n.NodeObject == newNode.NodeObject) == null)
					{
						// Neighbor is not in open list, so compute scores and add.
						openList.Insert(0,newNode);
					}
					else
					{
						if (currentGScore + newNode.G < newNode.F)
						{
							// If current path is better than previous,
							// update to new path
							newNode.G = currentGScore;
							newNode.ParentNode = currentNode;
						}
					}
				}
			}

			if (currentNode.GetPath().ContainsNode(endNode))
			{
				path = currentNode.GetPath();
			}
			
			return path;
		}
	}
}