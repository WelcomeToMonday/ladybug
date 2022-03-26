namespace Ladybug.Pathfinding
{
	/// <summary>
	/// Represents an abstract node used to generate paths
	/// </summary>
	public class Node
	{
		/// <summary>
		/// Create a node
		/// </summary>
		/// <param name="gScore"></param>
		/// <param name="hScore"></param>
		/// <param name="nodeObject"></param>
		/// <param name="parentNode"></param>
		public Node(int gScore, int hScore, IPathable nodeObject, Node parentNode = null)
		{
			G = gScore;
			H = hScore;
			NodeObject = nodeObject;
			ParentNode = parentNode;
		}

		/// <summary>
		/// The Node's A* "H" Score
		/// </summary>
		/// <value></value>
		public int H { get; set; }

		/// <summary>
		/// The Node's A* "G" Score
		/// </summary>
		/// <value></value>
		public int G { get; set; }

		/// <summary>
		/// The Node's A* "F" Score
		/// </summary>
		/// <value></value>
		public int F { get => G + H; }

		/// <summary>
		/// The pathable object this node represents
		/// </summary>
		/// <value></value>
		public IPathable NodeObject { get; }

		/// <summary>
		/// The Node's parent Node
		/// </summary>
		/// <value></value>
		public Node ParentNode{ get; set; } = null;

		/// <summary>
		/// Generate a NodePath including this Node and its ancestor nodes, if present
		/// </summary>
		/// <returns></returns>
		public NodePath GetPath()
		{
			var path = new NodePath();
			var current = this;
			var parent = ParentNode;

			path.AddNode(current);

			while (parent != null)
			{
				path.AddNode(parent);
				current = parent;
				parent = current.ParentNode;
			} 

			return path;
		}
	}
}