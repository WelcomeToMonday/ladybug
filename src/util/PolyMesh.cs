using System.Collections.Generic;

namespace Ladybug
{
	/// <summary>
	/// Represents a mesh of one or more polygons
	/// </summary>
	public class PolyMesh
	{
		/// <summary>
		/// The mesh containing one or more polygons
		/// </summary>
		public List<Polygon> Mesh;

		/// <summary>
		/// Create a new PolyMesh
		/// </summary>
		public PolyMesh()
		{
			Mesh = new List<Polygon>();
		}
	}
}