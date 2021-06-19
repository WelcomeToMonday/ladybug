using Microsoft.Xna.Framework;

namespace Ladybug
{	
	public enum BoxHandle {TOPLEFT, TOPRIGHT, BOTTOMLEFT, BOTTOMRIGHT, CENTER}
	
	public static class RectangleExtensions
	{
		public static Rectangle CopyAtPosition(this Rectangle r, Vector2 position, BoxHandle handle = BoxHandle.TOPLEFT)
		{
			int x = 0;
			int y = 0;

			switch (handle)
			{
				default:
				case BoxHandle.TOPLEFT:
					x = (int)position.X;
					y = (int)position.Y;
					break;
				case BoxHandle.TOPRIGHT:
					x = (int)(position.X - r.Width);
					y = (int)position.Y;
					break;
				case BoxHandle.BOTTOMLEFT:
					x = (int)(position.X);
					y = (int)(position.Y - r.Height);
					break;
				case BoxHandle.BOTTOMRIGHT:
					x = (int)(position.X - r.Width);
					y = (int)(position.Y - r.Height);
					break;
				case BoxHandle.CENTER:
					x = (int)(position.X) - (r.Width / 2);
					y = (int)(position.Y) - (r.Height / 2);
					break;
			}
			
			r = new Rectangle(x, y, r.Width, r.Height);
			return r;
		}

		public static Rectangle CopyAtPosition(this Rectangle r, int x, int y, BoxHandle handle = BoxHandle.TOPLEFT) => r.CopyAtPosition(new Vector2(x,y),handle);

		public static Rectangle CopyAtOffset(this Rectangle r, Vector2 newPosition)
		{
			var res = r.CopyAtPosition(new Vector2(
				(int)(r.X + newPosition.X),
				(int)(r.Y + newPosition.Y)
				)
			);
			return res;
		}

		public static Rectangle CopyAtOffset(this Rectangle r, int x, int y) => r.CopyAtOffset(new Vector2(x,y));

		public static bool PointInBounds(this Rectangle r, Vector2 position)
		{
			return (
				position.X >= r.Left &&
				position.X <= r.Right &&
				position.Y >= r.Top &&
				position.Y <= r.Bottom
				);
		}

		public static Vector2 GetHandlePosition(this Rectangle r, BoxHandle handle)
		{
			Vector2 res = Vector2.Zero;

			switch (handle)
			{
				default:
				case BoxHandle.TOPLEFT:
					res = new Vector2(
						(int)r.X,
						(int)r.Y
						);
					break;
				case BoxHandle.TOPRIGHT:
					res = new Vector2(
						(int)r.X + r.Width,
						(int)r.Y
					);
					break;
				case BoxHandle.BOTTOMLEFT:
					res = new Vector2(
						(int)r.X,
						(int)r.Y + r.Height
					);
					break;
				case BoxHandle.BOTTOMRIGHT:
					res = new Vector2(
						(int)r.X + r.Width,
						(int)r.Y + r.Height
					);
					break;
				case BoxHandle.CENTER:
					res = new Vector2(
						(int)(r.X + (r.Width / 2)),
						(int)(r.Y + (r.Height / 2))
					);
					break;
			}
			return res;
		}
	}
}