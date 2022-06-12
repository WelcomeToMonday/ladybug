using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Ladybug
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
				throw new ArgumentException("A polygon must consist of at least three vectors");
			}
		}

		/// <summary>
		/// The vertices of the polygon
		/// </summary>
		/// <value></value>
		public Vector2[] Points { get; protected set; }

		/// <summary>
		/// The mathematical center of the polygon, considered the average position of all of its points
		/// </summary>
		public Vector2 Centroid
		{
			get
			{
				float x = 0.0f;
				float y = 0.0f;

				foreach (var p in Points)
				{
					x += p.X;
					y += p.Y;
				}

				x = x / Points.Length;
				y = y / Points.Length;

				return new Vector2(x, y);
			}
		}

		/// <summary>
		/// Returns <c>true</c> if the given vector lies within the bounds of the polygon
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public virtual bool ContainsVector(Vector2 vector)
		{
			// todo: replace w/ raycast algo
			// - may want to save this somewhere as it is handy for any convex poly
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

		/// <summary>
		/// Sorts the Polygon's vertices clockwise around its centroid.
		/// </summary>
		/// <remarks>
		/// As this uses the centroid of the polygon, the more irregular the polygon, the
		/// less likely sorting it will result in an accurate shape. It is advised to construct highly-irregular
		/// polygons from an already-sorted list of points rather than sorting them afterward.
		/// </remarks>
		public void SortPoints()
		{
			Func<Vector2, Vector2, float> getAngle = (c, v) =>
			{
				return MathF.Atan2(v.Y - c.Y, v.X - c.X);
			};

			Func<Vector2, Vector2, Vector2, int> resolveEqual = (c, x, y) =>
			{
				var vx = new Vector2(
					MathF.Abs(c.X - x.X),
					MathF.Abs(c.Y - x.Y)
					);

				var vy = new Vector2(
					MathF.Abs(c.X - y.X),
					MathF.Abs(c.Y - y.Y)
					);

				var diffX = vx.X + vx.Y;
				var diffY = vy.X + vy.Y;

				if (diffX > diffY)
				{
					return 1;
				}
				else if (diffX < diffY)
				{
					return -1;
				}
				else
				{
					return 0;
				}
			};

			var c = Centroid;
			var p = Points.ToList();

			p.Sort((x, y) =>
				{
					var ax = getAngle(c, x);
					var ay = getAngle(c, y);

					if (ax > ay)
					{
						return 1;
					}
					else if (ax < ay)
					{
						return -1;
					}
					else
					{
						return resolveEqual(c, x, y);
					}
				});

			Points = p.ToArray();
		}
	}
}