// Based on articles found at http://www.redblobgames.com/grids/hexagons/
using System;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	public class HexTransform
	{
		public HexTransform(Rectangle outerBounds, HexOrientation orientation = Hex.DEFAULT_ORIENTATION)
		{
			OuterBounds = outerBounds;
			Orientation = orientation;
		}

		public HexTransform(Vector2 size, HexOrientation orientation = Hex.DEFAULT_ORIENTATION)
		{
			OuterBounds = new Rectangle();
			Orientation = orientation;
			SetSize(size);
		}

		public HexOrientation Orientation { get; set; } = Hex.DEFAULT_ORIENTATION;

		public Vector2 Location => OuterBounds.Location.ToVector2();

		public Rectangle OuterBounds { get; set; } // outer rectangular bounds

		public Vector2 Size { get; set; }

		public Polygon InnerBounds { get; } // inner hexagonal bounds // todo

		public void SetPosition(int x, int y) => SetPosition(new Vector2(x, y));

		public void SetPosition(Vector2 pos)
		{
			OuterBounds = new Rectangle((int)pos.X, (int)pos.Y, OuterBounds.Width, OuterBounds.Height);
		}

		public void SetSize(int x, int y) => SetSize(new Vector2(x, y));

		public void SetSize(Vector2 size)
		{
			double width = 0;
			double height = 0;

			switch (Orientation)
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

			OuterBounds = new Rectangle(OuterBounds.Location.X, OuterBounds.Location.Y, (int)width, (int)height);
		}
	}
}