using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ladybug.Pathfinding
{
	public class NodePath // : IEnumerable ?
	{
		public NodePath(List<Node> nodeList)
		{
			Nodes = nodeList;
		}

		public NodePath(){}

		public List<Node> Nodes { get; private set; } = new List<Node>();

		public bool Empty { get => (Nodes == null || Nodes.Count < 1); }

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

		public Node this[int i]
		{
			get => Nodes[i];
		}

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

		public bool ContainsNode(IPathable nodeObject)
		{
			var res = Nodes.FirstOrDefault(n => n.NodeObject == nodeObject);
			return !(res == null);
		}

		public bool ContainsNode(Node node)
		{
			return ContainsNode(node.NodeObject);
		}
	}
}