using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ladybug.Pathfinding
{
	/// <summary>
	/// Represents a path of connected nodes
	/// </summary>
	public class NodePath // : IEnumerable ?
	{
		/// <summary>
		/// Create a NodePath
		/// </summary>
		/// <param name="nodeList"></param>
		public NodePath(List<Node> nodeList)
		{
			Nodes = nodeList;
		}

		/// <summary>
		/// Create a NodePath
		/// </summary>
		public NodePath(){}

		/// <summary>
		/// Collection of nodes within this path
		/// </summary>
		/// <returns></returns>
		public List<Node> Nodes { get; private set; } = new List<Node>();

		/// <summary>
		/// Whether the path contains any nodes
		/// </summary>
		/// <returns></returns>
		public bool Empty { get => (Nodes == null || Nodes.Count < 1); }

		/// <summary>
		/// The total score value of this path
		/// </summary>
		/// <value></value>
		public int Score 
		{
			get
			{
				int s = 0;
				foreach (var n in Nodes)
				{
					s += n.F;
				}
				return s;
			}
		}

		/// <summary>
		/// Retrieves a node in the path by position
		/// </summary>
		/// <value></value>
		public Node this[int i]
		{
			get => Nodes[i];
		}

		/// <summary>
		/// Adds a node to the path
		/// </summary>
		/// <param name="n"></param>
		public void AddNode(Node n)
		{
			Nodes.Add(n);
		}

		/// <summary>
		/// Reverses the NodePath in-place.
		/// </summary>
		public void Reverse()
		{
			Nodes.Reverse();
		}

		/// <summary>
		/// Returns a reversed copy of this NodePath
		/// </summary>
		/// <returns></returns>
		public NodePath GetReverse()
		{
			var newPath = new List<Node>(Nodes);
			newPath.Reverse();
			return new NodePath(newPath);
		}

		/// <summary>
		/// Checks whether the given IPathable object is represented in this path
		/// </summary>
		/// <param name="nodeObject"></param>
		/// <returns></returns>
		public bool ContainsNode(IPathable nodeObject)
		{
			var res = Nodes.FirstOrDefault(n => n.NodeObject == nodeObject);
			return !(res == null);
		}

		/// <summary>
		/// Checks whether the given Node is in this path
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool ContainsNode(Node node)
		{
			return ContainsNode(node.NodeObject);
		}
	}
}