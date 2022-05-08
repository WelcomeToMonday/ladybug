// Based on articles found at http://www.redblobgames.com/grids/hexagons/
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	public struct Hexagon
	{
		public static readonly Hexagon[] Directions;
		public static readonly Hexagon[] Diagonals;

		static Hexagon()
		{
			Directions = new[]
			{
				new Hexagon(1, 0, -1),
				new Hexagon(1, -1, 0),
				new Hexagon(0, -1, 1),
				new Hexagon(-1, 0, 1),
				new Hexagon(-1, 1, 0),
				new Hexagon(0, 1, -1)
			};

			Diagonals = new[]
			{
				new Hexagon(2, -1, -1),
				new Hexagon(1, -2, 1),
				new Hexagon(-1, -1, 2),
				new Hexagon(-2, 1, 1),
				new Hexagon(-1, 2, -1),
				new Hexagon(1, 1, -2)
			};
		}

		public Hexagon(int q, int r, int s)
		{
			Q = q;
			R = r;
			S = s;
		}

		public readonly int Q { get; }
		public readonly int R { get; }
		public readonly int S { get; }

		public int Length => (int)((Math.Abs(Q) + Math.Abs(R) + Math.Abs(S)) / 2);

		public static Hexagon operator +(Hexagon a, Hexagon b) => a.Add(b);
		public static Hexagon operator -(Hexagon a, Hexagon b) => a.Subtract(b);

		public Hexagon Add(Hexagon other)
		{
			return new Hexagon(Q + other.Q, R + other.R, S + other.S);
		}

		public Hexagon Subtract(Hexagon other)
		{
			return new Hexagon(Q - other.Q, R - other.R, S - other.S);
		}

		public Hexagon Scale(int k)
		{
			return new Hexagon(Q * k, R * k, S * k);
		}

		public Hexagon RotateLeft()
		{
			return new Hexagon(-S, -Q, -R);
		}

		public Hexagon RotateRight()
		{
			return new Hexagon(-R, -S, -Q);
		}

		public int Distance(Hexagon hex)
		{
			return Subtract(hex).Length;
		}

		private struct FractionalCoordinate
		{
			public static List<Hexagon> HexLineDraw(Hexagon a, Hexagon b)
			{
				int N = a.Distance(b);
				FractionalCoordinate a_nudge = new FractionalCoordinate(a.Q + 1e-06, a.R + 1e-06, a.S - 2e-06);
				FractionalCoordinate b_nudge = new FractionalCoordinate(b.Q + 1e-06, b.R + 1e-06, b.S - 2e-06);
				List<Hexagon> results = new List<Hexagon>();
				double step = 1.0 / Math.Max(N, 1);
				for (int i = 0; i <= N; i++)
				{
					results.Add(a_nudge.HexLerp(b_nudge, step * i).HexRound());
				}
				return results;
			}

			public FractionalCoordinate(double q, double r, double s)
			{
				this.q = q;
				this.r = r;
				this.s = s;
				if (Math.Round(q + r + s) != 0)
				{
					throw new ArgumentException("q + r + s must be 0");
				}
			}

			public readonly double q;
			public readonly double r;
			public readonly double s;

			public Hexagon HexRound()
			{
				int qi = (int)(Math.Round(q));
				int ri = (int)(Math.Round(r));
				int si = (int)(Math.Round(s));
				double q_diff = Math.Abs(qi - q);
				double r_diff = Math.Abs(ri - r);
				double s_diff = Math.Abs(si - s);
				if (q_diff > r_diff && q_diff > s_diff)
				{
					qi = -ri - si;
				}
				else
						if (r_diff > s_diff)
				{
					ri = -qi - si;
				}
				else
				{
					si = -qi - ri;
				}
				return new Hexagon(qi, ri, si);
			}

			public FractionalCoordinate HexLerp(FractionalCoordinate b, double t)
			{
				return new FractionalCoordinate(q * (1.0 - t) + b.q * t, r * (1.0 - t) + b.r * t, s * (1.0 - t) + b.s * t);
			}
		}

		private struct OffsetCoord
		{
			public OffsetCoord(int col, int row)
			{
				this.col = col;
				this.row = row;
			}
			public readonly int col;
			public readonly int row;
			static public int EVEN = 1;
			static public int ODD = -1;

			public static OffsetCoord QoffsetFromCube(int offset, Hexagon h)
			{
				int col = h.Q;
				int row = h.R + (int)((h.Q + offset * (h.Q & 1)) / 2);
				if (offset != OffsetCoord.EVEN && offset != OffsetCoord.ODD)
				{
					throw new ArgumentException("offset must be EVEN (+1) or ODD (-1)");
				}
				return new OffsetCoord(col, row);
			}

			static public Hexagon QoffsetToCube(int offset, OffsetCoord h)
			{
				int q = h.col;
				int r = h.row - (int)((h.col + offset * (h.col & 1)) / 2);
				int s = -q - r;
				if (offset != OffsetCoord.EVEN && offset != OffsetCoord.ODD)
				{
					throw new ArgumentException("offset must be EVEN (+1) or ODD (-1)");
				}
				return new Hexagon(q, r, s);
			}

			static public OffsetCoord RoffsetFromCube(int offset, Hexagon h)
			{
				int col = h.Q + (int)((h.R + offset * (h.R & 1)) / 2);
				int row = h.R;
				if (offset != OffsetCoord.EVEN && offset != OffsetCoord.ODD)
				{
					throw new ArgumentException("offset must be EVEN (+1) or ODD (-1)");
				}
				return new OffsetCoord(col, row);
			}


			static public Hexagon RoffsetToCube(int offset, OffsetCoord h)
			{
				int q = h.col - (int)((h.row + offset * (h.row & 1)) / 2);
				int r = h.row;
				int s = -q - r;
				if (offset != OffsetCoord.EVEN && offset != OffsetCoord.ODD)
				{
					throw new ArgumentException("offset must be EVEN (+1) or ODD (-1)");
				}
				return new Hexagon(q, r, s);
			}

		}

		struct DoubledCoord
		{
			public DoubledCoord(int col, int row)
			{
				this.col = col;
				this.row = row;
			}

			public readonly int col;
			public readonly int row;

			static public DoubledCoord QdoubledFromCube(Hexagon h)
			{
				int col = h.Q;
				int row = 2 * h.R + h.Q;
				return new DoubledCoord(col, row);
			}

			public Hexagon QdoubledToCube()
			{
				int q = col;
				int r = (int)((row - col) / 2);
				int s = -q - r;
				return new Hexagon(q, r, s);
			}

			static public DoubledCoord RdoubledFromCube(Hexagon h)
			{
				int col = 2 * h.Q + h.R;
				int row = h.R;
				return new DoubledCoord(col, row);
			}

			public Hexagon RdoubledToCube()
			{
				int q = (int)((col - row) / 2);
				int r = row;
				int s = -q - r;
				return new Hexagon(q, r, s);
			}
		}

		private struct Orientation
		{
			public Orientation(double f0, double f1, double f2, double f3, double b0, double b1, double b2, double b3, double start_angle)
			{
				this.f0 = f0;
				this.f1 = f1;
				this.f2 = f2;
				this.f3 = f3;
				this.b0 = b0;
				this.b1 = b1;
				this.b2 = b2;
				this.b3 = b3;
				this.start_angle = start_angle;
			}
			public readonly double f0;
			public readonly double f1;
			public readonly double f2;
			public readonly double f3;
			public readonly double b0;
			public readonly double b1;
			public readonly double b2;
			public readonly double b3;
			public readonly double start_angle;
		}

		struct Layout
		{
			public Layout(Orientation orientation, Point size, Point origin)
			{
				this.orientation = orientation;
				this.size = size;
				this.origin = origin;
			}

			public readonly Orientation orientation;
			public readonly Point size;
			public readonly Point origin;
			public static Orientation pointy = new Orientation(Math.Sqrt(3.0), Math.Sqrt(3.0) / 2.0, 0.0, 3.0 / 2.0, Math.Sqrt(3.0) / 3.0, -1.0 / 3.0, 0.0, 2.0 / 3.0, 0.5);
			public static Orientation flat = new Orientation(3.0 / 2.0, 0.0, Math.Sqrt(3.0) / 2.0, Math.Sqrt(3.0), 2.0 / 3.0, 0.0, -1.0 / 3.0, Math.Sqrt(3.0) / 3.0, 0.0);

			public Vector2 HexToPixel(Hexagon h)
			{
				Orientation M = orientation;
				double x = (M.f0 * h.Q + M.f1 * h.R) * size.X;
				double y = (M.f2 * h.Q + M.f3 * h.R) * size.Y;
				return new Vector2((float)(x + origin.X), (float)(y + origin.Y));
			}

			public Hexagon PixelToHex(Vector2 p)
			{
				Orientation M = orientation;
				Vector2 pt = new Vector2((p.X - origin.X) / size.X, (p.Y - origin.Y) / size.Y);
				int q = (int)(M.b0 * pt.X + M.b1 * pt.Y);
				int r = (int)(M.b2 * pt.X + M.b3 * pt.Y);
				return new Hexagon(q, r, -q - r);
			}

			public Vector2 HexCornerOffset(int corner)
			{
				Orientation M = orientation;
				double angle = 2.0 * Math.PI * (M.start_angle - corner) / 6.0;
				return new Vector2((float)(size.X * Math.Cos(angle)), (float)(size.Y * Math.Sin(angle)));
			}

			public Vector2[] PolygonCorners(Hexagon h)
			{
				Vector2[] corners = new Vector2[6];
				Vector2 center = HexToPixel(h);
				for (int i = 0; i < 6; i++)
				{
					Vector2 offset = HexCornerOffset(i);
					corners[i] = new Vector2(center.X + offset.X, center.Y + offset.Y);
				}
				return corners;
			}
		}
	}
}