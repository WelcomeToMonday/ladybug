using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Ladybug.Core
{
	public class Polygon
	{
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

		public Vector2[] Points { get; private set; }

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