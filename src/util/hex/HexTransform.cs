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

		/// <summary>
		/// Create a HexTransform with default bounds and orientation
		/// </summary>
		public HexTransform()
		{
			Bounds = Rectangle.Empty;
			Orientation = Hex.DEFAULT_ORIENTATION;
		}

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
		public HexOrientation Orientation { get; set; } = Hex.DEFAULT_ORIENTATION;

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
		public Rectangle Bounds { get; set; } // outer rectangular bounds

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
	}
}