// Based on articles found at http://www.redblobgames.com/grids/hexagons/
using System;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	public class HexTransform
	{
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

		public HexTransform()
		{
			Bounds = Rectangle.Empty;
			Orientation = Hex.DEFAULT_ORIENTATION;
		}

		public HexTransform(Rectangle bounds, HexOrientation orientation = Hex.DEFAULT_ORIENTATION)
		{
			Bounds = bounds;
			Orientation = orientation;
		}

		public HexOrientation Orientation { get; set; } = Hex.DEFAULT_ORIENTATION;

		public Vector2 Location
		{
			get => Bounds.Location.ToVector2();
			set
			{
				Bounds = new Rectangle((int)value.X, (int)value.Y, Bounds.Width, Bounds.Height);
			}
		}

		public Rectangle Bounds { get; set; } // outer rectangular bounds

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