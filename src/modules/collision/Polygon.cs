using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Ladybug.Collision
{
	/// <summary>
	/// Represents an abstract polygon shape with three or more vectors
	/// </summary>
	public class Polygon
	{
		/// <summary>
		/// Creates a new polygon object from three or more vectors
		/// </summary>
		/// <param name="vectors"></param>
		public Polygon(params Vector2[] vectors)
		{
			if (vectors.Length >= 3)
			{
				Points = vectors;
			}
			else
			{
				Points = new[]
				{
					Vector2.Zero,
					Vector2.Zero,
					Vector2.Zero
				};
			}
		}

		/// <summary>
		/// The vertices of the polygon
		/// </summary>
		/// <value></value>
		public Vector2[] Points { get; private set; }

		/// <summary>
		/// Returns <c>true</c> if the given vector lies within the bounds of the polygon
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public bool ContainsVector(Vector2 vector)
		{
			bool res = false;

			int j = Points.Length - 1;
			for (int i = 0; i < Points.Length; i++)
			{
				if (Points[i].Y < vector.Y && Points[j].Y >= vector.Y || Points[j].Y < vector.Y && Points[i].Y >= vector.Y)
				{
					if (Points[i].X + (vector.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) * (Points[j].X - Points[i].X) < vector.X)
					{
						res = !res;
					}
				}
				j = i;
			}
			return res;
		}
	}
}