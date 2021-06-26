using Microsoft.Xna.Framework;

namespace Ladybug
{
	/// <summary>
	/// Ladybug Transform instance
	/// </summary>
	/// <remarks>
	/// Represents basic location, size, scale, and rotation information
	/// </remarks>
	public class Transform
	{
		/// <summary>
		/// Default scale value
		/// </summary>
		/// <returns></returns>
		public static Vector2 DefaultScale { get; } = new Vector2(1.0f, 1.0f);

		/// <summary>
		/// The outer bounds of the Transform
		/// </summary>
		public Rectangle Bounds { get; private set; } = new Rectangle();

		/// <summary>
		/// The rotation value of the Transform, in degrees
		/// </summary>
		public float Rotation { get; private set; } = 0.0f;

		/// <summary>
		/// The scale of the Transform
		/// </summary>
		public Vector2 Scale { get; private set; } = new Vector2(1, 1);

		/// <summary>
		/// Read-only location value of the Transform
		/// </summary>
		/// <remarks>
		/// To move the Transform, use <see cref="Move(int, int)"/>,
		/// <see cref="SetPosition(int, int, BoxHandle)"/>, or their
		/// overloads.
		/// </remarks>
		public Vector2 Location
		{
			get
			{
				if (m_LocationP != Bounds.Location)
				{
					m_LocationP = Bounds.Location;
					m_LocationV = Bounds.Location.ToVector2();
				}
				return m_LocationV;
			}
		}

		private Vector2 m_LocationV = Vector2.Zero;
		private Point m_LocationP = Point.Zero;

		/// <summary>
		/// Moves the Transform, updating its position
		/// </summary>
		/// <param name="newPos">Vector2 value to move the Transform by</param>
		public void Move(Vector2 newPos)
		{
			SetPosition(new Vector2((int)(newPos.X + Bounds.X), (int)(newPos.Y + Bounds.Y)));
		}

		/// <summary>
		/// Moves the Transform, updating its position
		/// </summary>
		/// <param name="xMove">Distance to move the Transform by horizontally, in pixels</param>
		/// <param name="yMove">Distance to move the Transform by vertically, in pixels</param>
		public void Move(int xMove, int yMove)
		{
			Move(new Vector2(xMove, yMove));
		}

		/// <summary>
		/// Sets the Transform's bounds
		/// </summary>
		/// <param name="rectangle">New Bounds for the Transform</param>
		public void SetBounds(Rectangle rectangle)
		{
			Bounds = rectangle;
		}

		/// <summary>
		/// Sets the position of the Transform
		/// </summary>
		/// <param name="x">New x coordinate of the Transform</param>
		/// <param name="y">New y coordinate of the Transform</param>
		/// <param name="handle">The handle of the Transform's Bounds used to place it in its new Position</param>
		public void SetPosition(int x, int y, BoxHandle handle = BoxHandle.TopLeft) => SetPosition(new Vector2(x, y), handle);

		/// <summary>
		/// Sets the position of the Transform
		/// </summary>
		/// <param name="newPos">New location of the Transform</param>
		/// <param name="handle">The handle of the Transform's Bounds used to place it in its new Position</param>
		public void SetPosition(Vector2 newPos, BoxHandle handle = BoxHandle.TopLeft)
		{
			int x, y;
			switch (handle)
			{
				default:
				case BoxHandle.TopLeft:
					x = (int)(newPos.X);
					y = (int)(newPos.Y);
					break;
				case BoxHandle.TopRight:
					x = (int)(newPos.X + Bounds.Width);
					y = (int)(newPos.Y);
					break;
				case BoxHandle.BottomLeft:
					x = (int)(newPos.X);
					y = (int)(newPos.Y + Bounds.Height);
					break;
				case BoxHandle.BottomRight:
					x = (int)(newPos.X + Bounds.Width);
					y = (int)(newPos.Y + Bounds.Height);
					break;
				case BoxHandle.Center:
					x = (int)(newPos.X + (Bounds.Width / 2));
					y = (int)(newPos.Y + (Bounds.Height / 2));
					break;
			}
			Bounds = new Rectangle(x, y, Bounds.Width, Bounds.Height);
		}

		/// <summary>
		/// Rotates the Transform
		/// </summary>
		/// <param name="newRot">Value to rotate the Transform by, in degrees</param>
		/// <remarks>
		/// This method rotates the Transform relative to its current rotation.
		/// To set the Transform's rotation to an absolute value, use <see cref="Transform.SetRotation(float)"/>
		/// </remarks>
		public void RotateBy(float newRot)
		{
			Rotation += newRot;
		}

		/// <summary>
		/// Sets the Transform's rotation
		/// </summary>
		/// <param name="newRot">Transform's new rotation value, in degrees</param>
		public void SetRotation(float newRot)
		{
			Rotation = newRot;
		}

		/// <summary>
		/// Scales the Transform
		/// </summary>
		/// <param name="newScale">Value to scale the transform by</param>
		/// <remarks>
		/// This method scales the Transform relative to its current scale.
		/// To set the Transform's scale to an absolute value, use <see cref="Transform.SetScale(float)"/>
		/// </remarks>
		public void ScaleBy(float newScale) => ScaleBy(new Vector2(newScale, newScale));

		/// <summary>
		/// Scales the Transform
		/// </summary>
		/// <param name="newScale">Value to scale the transform by</param>
		/// <remarks>
		/// This method scales the Transform relative to its current scale.
		/// To set the Transform's scale to an absolute value, use <see cref="Transform.SetScale(float)"/>
		/// </remarks>
		public void ScaleBy(Vector2 newScale)
		{
			Scale += newScale;
		}

		/// <summary>
		/// Sets the Transform's Scale
		/// </summary>
		/// <param name="newScale">Transform's new scale value</param>
		public void SetScale(float newScale) => SetScale(new Vector2(newScale, newScale));

		/// <summary>
		/// Sets the Transform's Scale
		/// </summary>
		/// <param name="newScale">Transform's new scale value</param>
		public void SetScale(Vector2 newScale) => Scale = newScale;
	}
}
