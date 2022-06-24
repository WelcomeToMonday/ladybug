// References:
// Point-in-Polygon detection: https://www.geeksforgeeks.org/how-to-check-if-a-given-point-lies-inside-a-polygon/

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
		/// The mathematical center of the polygon, constructed from the average position of all of its points
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
		/// Bounding rectangle which contains the entirety of the polygon
		/// </summary>
		/// <value></value>
		public Rectangle Bounds
		{
			get
			{
				if (m_Bounds.IsEmpty)
				{
					m_Bounds = GetBounds();
				}
				return m_Bounds;
			}
		}
		private Rectangle m_Bounds;

		/// <summary>
		/// Returns <c>true</c> if the given vector lies within the bounds of the polygon
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public virtual bool Contains(Vector2 vector)
		{
			// https://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon
			var polygon = Points;
			var testPoint = vector;
			bool result = false;
			int j = polygon.Count() - 1;
			for (int i = 0; i < polygon.Count(); i++)
			{
				if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
				{
					if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
					{
						result = !result;
					}
				}
				j = i;
			}
			return result;
			/*
			if (!Bounds.Contains(vector, true))
			{
				return false;
			}

			var extreme = new Vector2(Bounds.Right + 10, vector.Y);

			int count = 0;
			int i = 0;
			var n = Points.Length;
			do
			{
				int next = (i + 1) % n;

				if (Line.Intersects(Points[i], Points[next], vector, extreme))
				{
					if (Line.GetSectionOrientation(Points[i], vector, Points[next]) == 0)
					{
						return Line.Contains(vector, Points[i], Points[next]);
					}

					count++;
				}
				i = next;
			} 
			while (i != 0);

			return count % 2 == 1;
			*/
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

		private Rectangle GetBounds()
		{
			var p = Points[0];

			var minX = p.X;
			var maxX = p.X;

			var minY = p.Y;
			var maxY = p.Y;

			foreach (var point in Points)
			{
				if (minX > point.X)
				{
					minX = point.X;
				}

				if (minY > point.Y)
				{
					minY = point.Y;
				}

				if (maxX < point.X)
				{
					maxX = point.X;
				}

				if (maxY < point.Y)
				{
					maxY = point.Y;
				}
			}

			return new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
		}
	}
}