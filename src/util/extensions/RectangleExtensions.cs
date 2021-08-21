using Microsoft.Xna.Framework;

namespace Ladybug
{
	/// <summary>
	/// Enum representing handle positions of a Rectangle
	/// </summary>
	public enum BoxHandle
	{
		/// <summary>
		/// Top-left corner
		/// </summary>
		TopLeft,
		/// <summary>
		/// Center of top edge
		/// </summary>
		TopCenter,
		/// <summary>
		/// Top-right corner
		/// </summary>
		TopRight,
		/// <summary>
		/// Bottom-left corner
		/// </summary>
		BottomLeft,
		/// <summary>
		/// Center of bottom edge
		/// </summary>
		BottomCenter,
		/// <summary>
		/// Bottom-right corner
		/// </summary> 
		BottomRight,
		/// <summary>
		/// Center
		/// </summary>
		Center,
		/// <summary>
		/// Center of left edge
		/// </summary>
		LeftCenter,
		/// <summary>
		/// Center of right edge
		/// </summary>
		RightCenter
	}

	/// <summary>
	/// Static class containing Rectangle static helper and extension methods
	/// </summary>
	public static class RectangleExtensions
	{
		/// <summary>
		/// Get a new copy of Rectangle at given position
		/// </summary>
		/// <param name="r"></param>
		/// <param name="position"></param>
		/// <param name="handle"></param>
		/// <returns>New Rectangle of same dimensions at given position</returns>
		public static Rectangle CopyAtPosition(this Rectangle r, Vector2 position, BoxHandle handle = BoxHandle.TopLeft)
		{
			int x = 0;
			int y = 0;

			switch (handle)
			{
				default:
				case BoxHandle.TopLeft:
					x = (int)position.X;
					y = (int)position.Y;
					break;
				case BoxHandle.TopRight:
					x = (int)(position.X - r.Width);
					y = (int)position.Y;
					break;
				case BoxHandle.TopCenter:
					x = (int)(position.X) - (r.Width / 2);
					y = (int)position.Y;
					break;
				case BoxHandle.BottomLeft:
					x = (int)(position.X);
					y = (int)(position.Y - r.Height);
					break;
				case BoxHandle.BottomRight:
					x = (int)(position.X - r.Width);
					y = (int)(position.Y - r.Height);
					break;
				case BoxHandle.BottomCenter:
					x = (int)(position.X) - (r.Width / 2);
					y = (int)(position.Y - r.Height);
					break;
				case BoxHandle.Center:
					x = (int)(position.X) - (r.Width / 2);
					y = (int)(position.Y) - (r.Height / 2);
					break;
				case BoxHandle.LeftCenter:
					x = (int)(position.X);
					y = (int)(position.Y) - (r.Height / 2);
					break;
				case BoxHandle.RightCenter:
					x = (int)(position.X - r.Width);
					y = (int)(position.Y) - (r.Height / 2);
					break;
			}

			r = new Rectangle(x, y, r.Width, r.Height);
			return r;
		}

		/// <summary>
		/// Get a new copy of Rectangle at given position
		/// </summary>
		/// <param name="r"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="handle"></param>
		/// <returns>New Rectangle of same dimensions at given position</returns>
		public static Rectangle CopyAtPosition(this Rectangle r, int x, int y, BoxHandle handle = BoxHandle.TopLeft) => r.CopyAtPosition(new Vector2(x, y), handle);

		/// <summary>
		/// Get a new copy of Rectangle at given offset
		/// </summary>
		/// <param name="r"></param>
		/// <param name="newPosition"></param>
		/// <returns></returns>
		public static Rectangle CopyAtOffset(this Rectangle r, Vector2 newPosition)
		{
			var res = r.CopyAtPosition(new Vector2(
				(int)(r.X + newPosition.X),
				(int)(r.Y + newPosition.Y)
				)
			);
			return res;
		}

		/// <summary>
		/// Get a new copy of Rectangle at given offset
		/// </summary>
		/// <param name="r"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Rectangle CopyAtOffset(this Rectangle r, int x, int y) => r.CopyAtOffset(new Vector2(x, y));

		/// <summary>
		/// Whether a given Point is within the bounds of this Rectangle
		/// </summary>
		/// <param name="r"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static bool PointInBounds(this Rectangle r, Vector2 position)
		{
			return (
				position.X >= r.Left &&
				position.X <= r.Right &&
				position.Y >= r.Top &&
				position.Y <= r.Bottom
				);
		}

		/// <summary>
		/// Gets the Vector2 position of one of this Rectangle's handles
		/// </summary>
		/// <param name="r"></param>
		/// <param name="handle"></param>
		/// <returns></returns>
		public static Vector2 GetHandlePosition(this Rectangle r, BoxHandle handle)
		{
			Vector2 res = Vector2.Zero;

			switch (handle)
			{
				default:
				case BoxHandle.TopLeft:
					res = new Vector2(
						(int)r.X,
						(int)r.Y
						);
					break;
				case BoxHandle.TopRight:
					res = new Vector2(
						(int)r.X + r.Width,
						(int)r.Y
					);
					break;
				case BoxHandle.TopCenter:
					res = new Vector2(
						(int)(r.X + (r.Width / 2)),
						(int)r.Y
					);
					break;
				case BoxHandle.BottomLeft:
					res = new Vector2(
						(int)r.X,
						(int)r.Y + r.Height
					);
					break;
				case BoxHandle.BottomRight:
					res = new Vector2(
						(int)r.X + r.Width,
						(int)r.Y + r.Height
					);
					break;
				case BoxHandle.BottomCenter:
					res = new Vector2(
						(int)(r.X + (r.Width / 2)),
						(int)r.Y + r.Height
					);
					break;
				case BoxHandle.Center:
					res = new Vector2(
						(int)(r.X + (r.Width / 2)),
						(int)(r.Y + (r.Height / 2))
					);
					break;
				case BoxHandle.LeftCenter:
					res = new Vector2(
						(int)r.X,
						(int)(r.Y + (r.Height / 2))
					);
					break;
				case BoxHandle.RightCenter:
					res = new Vector2(
						(int)(int)r.X + r.Width,
						(int)(r.Y + (r.Height / 2))
					);
					break;
			}
			return res;
		}
	}
}