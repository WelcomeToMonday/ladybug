// Based on articles found at http://www.redblobgames.com/grids/hexagons/
using System;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	public class HexGrid
	{
		private HexMatrix _matrix;

		public HexGrid(Vector2 origin, Vector2 dimensions, Vector2 hexSize, HexOffset offset = Hex.DEFAULT_OFFSET, HexOrientation orientation = Hex.DEFAULT_ORIENTATION)
		{
			Origin = origin;
			Dimensions = dimensions;
			HexSize = hexSize;
			Orientation = orientation;
			Offset = offset;
			_matrix = GetMatrix();
			Generate();
		}

		public Vector2 Origin { get; private set; }
		public Vector2 Dimensions { get; private set; }
		public Vector2 HexSize { get; private set; }

		public HexTransform[,] Grid { get; private set; }

		public HexOrientation Orientation { get; private set; } = Hex.DEFAULT_ORIENTATION;

		public HexOffset Offset { get; private set; } = Hex.DEFAULT_OFFSET;

		private void Generate()
		{
			var cols = (int)Dimensions.X;
			var rows = (int)Dimensions.Y;
			Grid = new HexTransform[cols, rows];

			for (var col = 0; col < cols; col++)
			{
				for (var row = 0; row < rows; row++)
				{
					var h = Hex.FromOffset(col, row, Offset, Orientation);
					var loc = HexToPixel(h);
					//var bounds = new Rectangle(loc.ToPoint(), HexSize.ToPoint());
					//var t = new HexTransform(bounds);
					var t = new HexTransform(HexSize, Orientation);
					t.SetPosition(loc);
					Grid[col, row] = t;
				}
			}
		}

		public Vector2 HexToPixel(Hex hx)
		{
			var m = _matrix;
			double x = (m.f0 * hx.Q + m.f1 * hx.R) * HexSize.X;
			double y = (m.f2 * hx.Q + m.f3 * hx.R) * HexSize.Y;

			return new Vector2((float)(x + Origin.X), (float)(y + Origin.Y));
		}

		public Hex PixelToHex(Vector2 px)
		{
			var m = _matrix;
			Vector2 pt = new Vector2((px.X - Origin.X) / HexSize.X, (px.Y - Origin.Y) / HexSize.Y);
			int q = (int)(m.b0 * pt.X + m.b1 * pt.Y);
			int r = (int)(m.b2 * pt.X + m.b3 * pt.Y);

			return new Hex(q, r, -q - r);
		}

		private HexMatrix GetMatrix()
		{
			HexMatrix m;
			switch (Orientation)
			{
				default:
				case HexOrientation.Flat:
					m = new HexMatrix(3.0 / 2.0, 0.0, Math.Sqrt(3.0) / 2.0, Math.Sqrt(3.0), 2.0 / 3.0, 0.0, -1.0 / 3.0, Math.Sqrt(3.0) / 3.0, 0.0);
					break;
				case HexOrientation.Point:
					m = new HexMatrix(Math.Sqrt(3.0), Math.Sqrt(3.0) / 2.0, 0.0, 3.0 / 2.0, Math.Sqrt(3.0) / 3.0, -1.0 / 3.0, 0.0, 2.0 / 3.0, 0.5);
					break;
			}
			return m;
		}

		private struct HexMatrix
		{
			public HexMatrix(double f0, double f1, double f2, double f3, double b0, double b1, double b2, double b3, double start_angle)
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
	}
}