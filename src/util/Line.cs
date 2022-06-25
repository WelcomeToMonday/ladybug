// References:
// Point-in-Polygon detection: https://www.geeksforgeeks.org/how-to-check-if-a-given-point-lies-inside-a-polygon/

using System;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	/// <summary>
	/// Line segment between two points
	/// </summary>
	public struct Line
	{
		/// <summary>
		/// Returns true if point v lies on line segment pq
		/// </summary>
		/// <param name="v">Point to check</param>
		/// <param name="p">Start of line segment qr</param>
		/// <param name="q">End of line segment qr</param>
		public static bool Contains(Vector2 v, Vector2 p, Vector2 q)
		{
			return new Line(p, q).Contains(v);
		}

		/// <summary>
		/// Returns true if lines l1 and l2 intersect
		/// </summary>
		/// <param name="l1"></param>
		/// <param name="l2"></param>
		/// <returns></returns>
		public static bool Intersects(Line l1, Line l2)
		=> Intersects(l1.P, l1.Q, l2.P, l2.Q);

		// todo: audit this.
		// see: https://stackoverflow.com/questions/3838329/how-can-i-check-if-two-segments-intersect
		/// <summary>
		/// Returns true of lines p1q1 and p2q2 intersect
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="q1"></param>
		/// <param name="p2"></param>
		/// <param name="q2"></param>
		/// <returns></returns>
		public static bool Intersects(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2)
		{
			// Handle edge case for passing through vertices
			if (p1.Y > q1.Y)
			{
				q1 = new Vector2(q1.X, q1.Y + 0.01f);
			}
			else if (p1.Y < q1.Y)
			{
				q1 = new Vector2(q1.X, q1.Y - 0.01f);
			}

			var o1 = GetSectionOrientation(p1, q1, p2);
			var o2 = GetSectionOrientation(p1, q1, q2);
			var o3 = GetSectionOrientation(p2, q2, p1);
			var o4 = GetSectionOrientation(p2, q2, q1);

			if (o1 != o2 && o3 != o4)
			{
				return true;
			}

			//if (o1 == 0 && Contains(p1, p2, q1))
			if (o1 == 0 && Contains(p2, p1, q1))
			{
				return true;
			}

			//if (o2 == 0 && Contains(p1, q2, q1))
			if (o2 == 0 && Contains(q2, p1, q1))
			{
				return true;
			}

			//if (o3 == 0 && Contains(p2, p1, q2)) //check this
			if (o3 == 0 && Contains(p1, p2, q2))
			{
				return true;
			}

			//if (o4 == 0 && Contains(p2, q1, q2))
			if (o4 == 0 && Contains(q1, p2, q2))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Utility method. Finds orientation of ordered triplet of vectors.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="q"></param>
		/// <param name="r"></param>
		/// <remarks>Used in segment intersection detection</remarks>
		/// <returns>0 if points are colinear. 1 if clockwise. 2 if counterclockwise</returns>
		public static int GetSectionOrientation(Vector2 p, Vector2 q, Vector2 r)
		{
			int val = (int)((q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y));

			if (val == 0)
			{
				return 0; // colinear
			}

			return (val > 0) ? 1 : 2; // clockwise or counterclock wise
		}

		/// <summary>
		/// Create a Line segment
		/// </summary>
		/// <param name="p">Start point of the Line</param>
		/// <param name="q">End point of the Line</param>
		public Line(Vector2 p, Vector2 q)
		{
			P = p;
			Q = q;
		}

		/// <summary>
		/// Start point of the line
		/// </summary>
		/// <value></value>
		public Vector2 P { get; private set; }

		/// <summary>
		/// End point of the line
		/// </summary>
		/// <value></value>
		public Vector2 Q { get; private set; }

		/// <summary>
		/// Returns true if point v lies on the line
		/// </summary>
		/// <param name="v">Point to check</param>
		/// <returns></returns>
		public bool Contains(Vector2 v)
		{
			var dxc = v.X - P.X;
			var dyc = v.Y - P.Y;

			var dxl = Q.X - P.X;
			var dyl = Q.Y - P.Y;

			if ((dxc * dyl - dyc * dxl) != 0)
			{
				return false;
			}

			if (MathF.Abs(dxl) >= MathF.Abs(dyl))
			{
				return dxl > 0 ?
					P.X <= v.X && v.X <= Q.X :
	 				Q.X <= v.X && v.X <= P.X;
			}
			else
			{
				return dyl > 0 ?
					P.Y <= v.Y && v.Y <= Q.Y :
					Q.Y <= v.Y && v.Y <= P.Y;
			}
		}

		/// <summary>
		/// Returns true if this line intersects with line pq
		/// </summary>
		/// <param name="p"></param>
		/// <param name="q"></param>
		/// <returns></returns>
		public bool Intersects(Vector2 p, Vector2 q) => Intersects(this.P, this.Q, p, q);

		/// <summary>
		/// Returns true if this line intersects with line l
		/// </summary>
		/// <param name="l"></param>
		/// <returns></returns>
		public bool Intersects(Line l) => Intersects(this, l);

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return (P, Q).GetHashCode();
		}
	}
}