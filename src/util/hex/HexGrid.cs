// Based on articles found at http://www.redblobgames.com/grids/hexagons/
using System;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	/// <summary>
	/// A two-dimensional grid of HexTransforms
	/// </summary>
	public class HexGrid
	{
		private HexMatrix _matrix;

		/// <summary>
		/// Create a HexGrid
		/// </summary>
		/// <param name="orientation"></param>
		/// <param name="offset"></param>
		public HexGrid(HexOrientation orientation = Hex.DEFAULT_ORIENTATION, HexOffset offset = Hex.DEFAULT_OFFSET)
		{
			Orientation = orientation;
			Offset = offset;
			_matrix = HexMatrix.GetMatrix(orientation);
		}

		/// <summary>
		/// The positional origin of the grid
		/// </summary>
		/// <value></value>
		public Vector2 Origin { get; private set; } = Vector2.Zero;

		/// <summary>
		/// The dimensions of the grid represented by row/column count
		/// </summary>
		/// <value></value>
		public Vector2 Dimensions { get; private set; } = Vector2.Zero;

		/// <summary>
		/// The size of the HexTransforms contained in this grid
		/// </summary>
		/// <remarks>A hexagon's size value represents the distance from the center to one of its corners</remarks>
		public Vector2 HexSize { get; private set; } = Vector2.Zero;

		/// <summary>
		/// Contents of the HexGrid
		/// </summary>
		/// <value></value>
		public HexTransform[,] Grid { get; private set; }

		/// <summary>
		/// Orientation of the HexTransforms within the HexGrid
		/// </summary>
		/// <value></value>
		public HexOrientation Orientation { get; private set; } = Hex.DEFAULT_ORIENTATION;

		/// <summary>
		/// The alternating-row offset method of the HexGrid
		/// </summary>
		/// <value></value>
		public HexOffset Offset { get; private set; } = Hex.DEFAULT_OFFSET;

		/// <summary>
		/// Set the dimensions of this HexGrid by row/column count
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public HexGrid WithDimensions(int x, int y) => WithDimensions(new Vector2(x, y));

		/// <summary>
		/// Set the dimensions of this HexGrid by row/column count
		/// </summary>
		/// <param name="dimensions"></param>
		/// <returns></returns>
		public HexGrid WithDimensions(Vector2 dimensions)
		{
			Dimensions = dimensions;
			return this;
		}

		/// <summary>
		/// Set the origin of this HexGrid
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public HexGrid WithOrigin(int x, int y) => WithOrigin(new Vector2(x, y));

		/// <summary>
		/// Set the origin of this HexGrid
		/// </summary>
		/// <param name="origin"></param>
		/// <returns></returns>
		public HexGrid WithOrigin(Vector2 origin)
		{
			Origin = origin;
			return this;
		}

		/// <summary>
		/// Set the size of the HexTransforms within this HexGrid
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <remarks>A hexagon's size value represents the distance from the center to one of its corners</remarks>
		public HexGrid WithHexSize(int x, int y) => WithHexSize(new Vector2(x, y));

		/// <summary>
		/// Set the size of the HexTransforms within this HexGrid
		/// </summary>
		/// <param name="size"></param>
		/// <remarks>A hexagon's size value represents the distance from the center to one of its corners</remarks>
		public HexGrid WithHexSize(Vector2 size)
		{
			HexSize = size;
			return this;
		}

		/// <summary>
		/// Set the bounds of the HexTransforms within this HexGrid
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public HexGrid WithHexBounds(int x, int y) => WithHexBounds(new Vector2(x, y));

		/// <summary>
		/// Set the bounds of the HexTransforms within this HexGrid
		/// </summary>
		/// <param name="bounds"></param>
		/// <returns></returns>
		public HexGrid WithHexBounds(Vector2 bounds)
		{
			HexSize = HexTransform.BoundsToSize(bounds, Orientation);
			return this;
		}

		/// <summary>
		/// Build the HexGrid
		/// </summary>
		/// <returns></returns>
		public HexGrid Generate()
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

					
					var tb = new Rectangle(loc.ToPoint(), HexTransform.SizeToBounds(HexSize, Orientation).ToPoint());
					var t = new HexTransform(tb, Orientation);
					Grid[col, row] = t;
				}
			}

			return this;
		}

		/// <summary>
		/// Get the pixel position of the given hex within this grid
		/// </summary>
		/// <param name="hx"></param>
		/// <returns></returns>
		public Vector2 HexToPixel(Hex hx)
		{
			var m = _matrix;
			double x = (m.f0 * hx.Q + m.f1 * hx.R) * HexSize.X;
			double y = (m.f2 * hx.Q + m.f3 * hx.R) * HexSize.Y;

			return new Vector2((float)(x + Origin.X), (float)(y + Origin.Y));
		}

		/// <summary>
		/// Get the hex within this grid which lies at the given position
		/// </summary>
		/// <param name="px"></param>
		/// <returns></returns>
		public Hex PixelToHex(Vector2 px)
		{
			var m = _matrix;
			Vector2 pt = new Vector2((px.X - Origin.X) / HexSize.X, (px.Y - Origin.Y) / HexSize.Y);
			int q = (int)(m.b0 * pt.X + m.b1 * pt.Y);
			int r = (int)(m.b2 * pt.X + m.b3 * pt.Y);

			return new Hex(q, r, -q - r);
		}
	}
}