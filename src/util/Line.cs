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
			return (
				P.X <= MathF.Max(v.X, Q.X) &&
				P.X >= MathF.Min(v.X, Q.X) &&
				P.Y <= MathF.Max(v.Y, Q.Y) &&
				P.Y >= MathF.Min(v.Y, Q.Y)
				);
		}
	}
}