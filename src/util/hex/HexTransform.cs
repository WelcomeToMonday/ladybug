// Based on articles found at http://www.redblobgames.com/grids/hexagons/
using System;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	/// <summary>
	/// A transform representing the size and position of a hexagon
	/// </summary>
	public class HexTransform
	{
		/// <summary>
		/// Convert a Vector2 representing the outer rectangular width and height of a HexTransform
		/// to a Vector2 representing the inner size value of the HexTransform
		/// </summary>
		/// <param name="bounds"></param>
		/// <param name="orientation"></param>
		/// <remarks>A hexagon's size value represents the distance from the center to one of its corners</remarks>
		public static Vector2 BoundsToSize(Vector2 bounds, HexOrientation orientation)
		{
			double width = 0;
			double height = 0;
			switch (orientation)
			{
				case HexOrientation.Point:
					width = bounds.X / Math.Sqrt(3);
					height = bounds.Y / 2;
					break;
				case HexOrientation.Flat:
					width = bounds.X / 2;
					height = bounds.Y / Math.Sqrt(3);
					break;
			}
			return new Vector2((float)width, (float)height);
		}

		/// <summary>
		/// Convert a Vector2 representing the inner size value of a HexTransform to a
		/// Vector2 representing its outer rectangular width and height
		/// </summary>
		/// <param name="size"></param>
		/// <param name="orientation"></param>
		/// <remarks>A hexagon's size value represents the distance from the center to one of its corners</remarks>
		public static Vector2 SizeToBounds(Vector2 size, HexOrientation orientation)
		{
			double width = 0;
			double height = 0;
			switch (orientation)
			{
				case HexOrientation.Point:
					width = Math.Sqrt(3) * size.X;
					height = 2 * size.Y;
					break;
				case HexOrientation.Flat:
					width = 2 * size.X;
					height = Math.Sqrt(3) * size.Y;
					break;
			}
			return new Vector2((float)width, (float)height);
		}

		private Hexagon _hexagon;

		/// <summary>
		/// Create a HexTransform with given bounds and orientation
		/// </summary>
		/// <param name="bounds"></param>
		/// <param name="orientation"></param>
		public HexTransform(Rectangle bounds, HexOrientation orientation = Hex.DEFAULT_ORIENTATION)
		{
			Bounds = bounds;
			Orientation = orientation;
		}

		/// <summary>
		/// Orientation of the HexTransform
		/// </summary>
		/// <value></value>
		public HexOrientation Orientation
		{
			get => m_Orientation;
			set
			{
				if (m_Orientation != value)
				{
					m_Orientation = value;
					_hexagon = new Hexagon(Location + new Vector2(Size.X, Size.Y), Size, m_Orientation);
				}
			}
		}
		private HexOrientation m_Orientation = Hex.DEFAULT_ORIENTATION;

		/// <summary>
		/// Location of the HexTransform
		/// </summary>
		/// <value></value>
		public Vector2 Location
		{
			get => Bounds.Location.ToVector2();
			set
			{
				Bounds = new Rectangle((int)value.X, (int)value.Y, Bounds.Width, Bounds.Height);
			}
		}

		/// <summary>
		/// Bounds of the HexTransform
		/// </summary>
		/// <value></value>
		public Rectangle Bounds
		{
			get => m_Bounds;
			set
			{
				if (m_Bounds != value)
				{
					m_Bounds = value;
					_hexagon = new Hexagon(Location + new Vector2(Size.X, Size.Y), Size, Orientation);
				}
			}
		}
		private Rectangle m_Bounds = Rectangle.Empty;

		/// <summary>
		/// Size of the HexTransform
		/// </summary>
		/// <remarks>A hexagon's size value represents the distance from the center to one of its corners</remarks>
		public Vector2 Size
		{
			get => BoundsToSize(new Vector2(Bounds.X, Bounds.Y), Orientation);
			set
			{
				var b = SizeToBounds(value, Orientation);
				Bounds = new Rectangle(Bounds.Location, new Point((int)b.X, (int)b.Y));
			}
		}

		/// <summary>
		/// Check if the given point is within this HexTransform's hexagonal bounds
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool Contains(Vector2 point) => _hexagon.ContainsVector(point);
	}
}