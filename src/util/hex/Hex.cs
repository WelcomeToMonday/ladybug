// Based on articles found at http://www.redblobgames.com/grids/hexagons/
using System;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	public enum HexOrientation
	{
		Point,
		Flat
	}

	public enum HexOffset
	{
		Odd = -1,
		Even = 1,
	}

	/// <summary>
	/// Hex Coordinate
	/// </summary>
	public struct Hex
	{
		public const HexOrientation DEFAULT_ORIENTATION = HexOrientation.Point;
		public const HexOffset DEFAULT_OFFSET = HexOffset.Odd;

		public static readonly Hex[] Directions;
		public static readonly Hex[] Diagonals;

		public static Hex FromOffset(Point pos, HexOffset offset = DEFAULT_OFFSET, HexOrientation orientation = DEFAULT_ORIENTATION)
		=> FromOffset(pos.X, pos.Y, offset, orientation);

		public static Hex FromOffset(Vector2 pos, HexOffset offset = DEFAULT_OFFSET, HexOrientation orientation = DEFAULT_ORIENTATION)
		=> FromOffset((int)pos.X, (int)pos.Y, offset, orientation);

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

		public Hex(int q, int r, int s)
		{
			Q = q;
			R = r;
			S = s;
		}

		public readonly int Q { get; }
		public readonly int R { get; }
		public readonly int S { get; }

		public int Length => (int)((Math.Abs(Q) + Math.Abs(R) + Math.Abs(S)) / 2);

		public static Hex operator +(Hex a, Hex b) => a.Add(b);
		public static Hex operator -(Hex a, Hex b) => a.Subtract(b);

		public Hex Add(Hex other) => new Hex(Q + other.Q, R + other.R, S + other.S);

		public Hex Subtract(Hex other) => new Hex(Q - other.Q, R - other.R, S - other.S);

		public Hex Scale(int k) => new Hex(Q * k, R * k, S * k);

		public Hex RotateLeft() => new Hex(-S, -Q, -R);

		public Hex RotateRight() => new Hex(-R, -S, -Q);

		public int Distance(Hex hex) => Subtract(hex).Length;

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