namespace Ladybug.Pathfinding
{
	public class Node
	{
		public Node(int gScore, int hScore, IPathable nodeObject, Node parentNode = null)
		{
			G = gScore;
			H = hScore;
			NodeObject = nodeObject;
			ParentNode = parentNode;
		}

		public int H { get; set; }

		public int G { get; set; }

		public int F { get => G + H; }

		public IPathable NodeObject { get; }

		public Node ParentNode{ get; set; } = null;

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