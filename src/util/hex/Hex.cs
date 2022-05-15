// Based on articles found at http://www.redblobgames.com/grids/hexagons/
using System;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	/// <summary>
	/// Represents the orientation of a hexagon
	/// </summary>
	public enum HexOrientation
	{
		/// <summary>
		/// Pointed-top hexagon orientation
		/// </summary>
		Point,
		/// <summary>
		/// Flat-top hexagon orientation
		/// </summary>
		Flat
	}

	/// <summary>
	/// Represents which rows or columns are offset within a hexagonal grid
	/// </summary>
	public enum HexOffset
	{
		/// <summary>
		/// Odd rows/columns are offset
		/// </summary>
		Odd = -1,
		/// <summary>
		/// Even rows/columns are offset
		/// </summary>
		Even = 1,
	}

	/// <summary>
	/// Hex Coordinate
	/// </summary>
	public struct Hex
	{
		/// <summary>
		/// Default hexagon orientation
		/// </summary>
		public const HexOrientation DEFAULT_ORIENTATION = HexOrientation.Point;

		/// <summary>
		/// Default grid offset method
		/// </summary>
		public const HexOffset DEFAULT_OFFSET = HexOffset.Odd;

		/// <summary>
		/// Directional coordinates
		/// </summary>
		public static readonly Hex[] Directions;

		/// <summary>
		/// Diagonal coordinates
		/// </summary>
		public static readonly Hex[] Diagonals;

		/// <summary>
		/// Convert an offset coordinate to a Hex coordinate value
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="offset"></param>
		/// <param name="orientation"></param>
		/// <returns></returns>
		public static Hex FromOffset(Point pos, HexOffset offset = DEFAULT_OFFSET, HexOrientation orientation = DEFAULT_ORIENTATION)
		=> FromOffset(pos.X, pos.Y, offset, orientation);

		/// <summary>
		/// Convert an offset coordinate to a Hex coordinate value
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="offset"></param>
		/// <param name="orientation"></param>
		/// <returns></returns>
		public static Hex FromOffset(Vector2 pos, HexOffset offset = DEFAULT_OFFSET, HexOrientation orientation = DEFAULT_ORIENTATION)
		=> FromOffset((int)pos.X, (int)pos.Y, offset, orientation);

		/// <summary>
		/// Convert an offset coordinate value to a Hex coordinate value
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="offset"></param>
		/// <param name="orientation"></param>
		/// <returns></returns>
		public static Hex FromOffset(int x, int y, HexOffset offset = DEFAULT_OFFSET, HexOrientation orientation = DEFAULT_ORIENTATION)
		{
			if (orientation == HexOrientation.Flat)
			{
				var q = x;
				var r = y - (int)((x + (int)offset * Math.Abs(x % 2)) / 2);
				var s = -q - r;

				return new Hex(q, r, s);
			}
			else
			{
				var q = x - (int)((y + (int)offset * Math.Abs(y % 2)) / 2);
				var r = y;
				var s = -q - r;

				return new Hex(q, r, s);
			}
		}

		static Hex()
		{
			Directions = new[]
			{
				new Hex(1, 0, -1),
				new Hex(1, -1, 0),
				new Hex(0, -1, 1),
				new Hex(-1, 0, 1),
				new Hex(-1, 1, 0),
				new Hex(0, 1, -1)
			};

			Diagonals = new[]
			{
				new Hex(2, -1, -1),
				new Hex(1, -2, 1),
				new Hex(-1, -1, 2),
				new Hex(-2, 1, 1),
				new Hex(-1, 2, -1),
				new Hex(1, 1, -2)
			};
		}

		/// <summary>
		/// Create a Hex coordinate
		/// </summary>
		/// <param name="q"></param>
		/// <param name="r"></param>
		/// <param name="s"></param>
		public Hex(int q, int r, int s)
		{
			Q = q;
			R = r;
			S = s;
		}

		/// <summary>
		/// Q axis coordinate value
		/// </summary>
		/// <value></value>
		public readonly int Q { get; }

		/// <summary>
		/// R axis coordinate value
		/// </summary>
		/// <value></value>
		public readonly int R { get; }

		/// <summary>
		/// S axis coordinate value
		/// </summary>
		/// <value></value>
		public readonly int S { get; }

		/// <summary>
		/// Length of the Hex coordinate
		/// </summary>
		/// <returns></returns>
		public int Length => (int)((Math.Abs(Q) + Math.Abs(R) + Math.Abs(S)) / 2);

		/// <summary>
		/// Sum Hex coordinates
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Hex operator +(Hex a, Hex b) => a.Add(b);

		/// <summary>
		/// Subtract Hex coordinates
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Hex operator -(Hex a, Hex b) => a.Subtract(b);

		/// <summary>
		/// Sum two Hex coordinates
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public Hex Add(Hex other) => new Hex(Q + other.Q, R + other.R, S + other.S);

		/// <summary>
		/// Subtract two Hex coordinates
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public Hex Subtract(Hex other) => new Hex(Q - other.Q, R - other.R, S - other.S);

		/// <summary>
		/// Scale the Hex coordinate
		/// </summary>
		/// <param name="k"></param>
		/// <returns></returns>
		public Hex Scale(int k) => new Hex(Q * k, R * k, S * k);

		/// <summary>
		/// Rotate Hex coordinate values counter-clockwise
		/// </summary>
		/// <returns></returns>
		public Hex RotateLeft() => new Hex(-S, -Q, -R);

		/// <summary>
		/// Rotate Hex coordinate values clockwise
		/// </summary>
		/// <returns></returns>
		public Hex RotateRight() => new Hex(-R, -S, -Q);

		/// <summary>
		/// Get the distance between Hex coordinates
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		public int Distance(Hex hex) => Subtract(hex).Length;

		/// <summary>
		/// Convert Hex coordinate to offset coordinate
		/// </summary>
		/// <param name="orientation"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		public Vector2 ToOffset(HexOrientation orientation = DEFAULT_ORIENTATION, HexOffset offset = DEFAULT_OFFSET)
		{
			if (orientation == HexOrientation.Flat)
			{
				int col = Q;
				int row = R + (int)((Q + (int)offset * Math.Abs(Q % 2)) / 2);

				return new Vector2(col, row);
			}
			else
			{
				int col = Q + (int)((R + (int)offset * Math.Abs(R % 2)) / 2);
				int row = R;

				return new Vector2(col, row);
			}
		}
	}
}