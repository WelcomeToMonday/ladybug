using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug
{
	/// <summary>
	/// Ladybug Camera
	/// </summary>
	public class Camera
	{
		private float _zoomMin = 0.35f;
		private float _zoomMax = 10.0f;
		private bool _restrictedToArea = false;
		
		/// <summary>
		/// Creates a new Camera
		/// </summary>
		/// <param name="bounds"></param>
		public Camera(Rectangle bounds)
		{
			Bounds = bounds;
		}
		
		/// <summary>
		/// Creates a new Camera
		/// </summary>
		/// <param name="viewport"></param>
		public Camera(Viewport viewport) : this(viewport.Bounds)
		{

		}

		/// <summary>
		/// World-relative position of the Camera
		/// </summary>
		/// <value></value>
		public Vector2 Position { get; protected set; }

		/// <summary>
		/// Bounds of the Camera
		/// </summary>
		/// <value></value>
		public Rectangle Bounds { get; protected set; }

		/// <summary>
		/// World-relative bounds the Camera is restricted to
		/// </summary>
		/// <value></value>
		public Rectangle AllowedArea { get; protected set; }

		/// <summary>
		/// Bounds representing what is contained within the camera's view
		/// </summary>
		/// <value></value>
		public Rectangle View { get; protected set; }

		/// <summary>
		/// Matrix representing the  Camera's size, scale, and rotation
		/// </summary>
		/// <value></value>
		public Matrix TransformMatrix { get; protected set; }

		/// <summary>
		/// Zoom level
		/// </summary>
		/// <value></value>
		public float Zoom { get; protected set; } = 1.0f;

		/// <summary>
		/// Adjust the zoom level relative to the
		/// current zoom level
		/// </summary>
		/// <param name="z"></param>
		public void ZoomBy(float z)
		{
			Zoom += z;
			if (Zoom < _zoomMin) Zoom = _zoomMin;
			if (Zoom > _zoomMax) Zoom = _zoomMax;
		}

		/// <summary>
		/// Set the zoom level
		/// </summary>
		/// <param name="z"></param>
		public void SetZoom(float z)
		{
			Zoom = z;
			if (Zoom < _zoomMin) Zoom = _zoomMin;
			if (Zoom > _zoomMax) Zoom = _zoomMax;
		}

		/// <summary>
		/// Move the Camera relative to its current
		/// position
		/// </summary>
		/// <param name="newPosition"></param>
		public void Move(Vector2 newPosition) => SetPosition(Position + newPosition);
		
		/// <summary>
		/// Adjust the Camera's position via linear interpolation
		/// </summary>
		/// <param name="newPosition"></param>
		/// <param name="by"></param>
		public void LerpTo(Vector2 newPosition, float by)
		{
			float rx = Extensions.Lerp(Position.X, newPosition.X, by);
			float ry = Extensions.Lerp(Position.Y, newPosition.Y, by);
			SetPosition(new Vector2(rx, ry));
		}

		/// <summary>
		/// Set the bounds the Camera must remain
		/// within
		/// </summary>
		/// <param name="r"></param>
		public void SetAllowedArea(Rectangle r)
		{
			_restrictedToArea = true;
			AllowedArea = r;
		}

		/// <summary>
		/// Remove bounds restrictions
		/// </summary>
		public void ClearAllowedArea()
		{
			_restrictedToArea = false;
		}

		/// <summary>
		/// Sets the Camera's position
		/// </summary>
		/// <param name="newPosition"></param>
		public void SetPosition(Vector2 newPosition)
		{
			var posX = newPosition.X;
			var posY = newPosition.Y;
			
			if (_restrictedToArea)
			{

				var cameraMax = new Vector2(
					AllowedArea.Width - (Bounds.Width / Zoom / 2),
					AllowedArea.Height - (Bounds.Height / Zoom / 2)
				);

				var clampPos = Vector2.Clamp(
					newPosition,
					new Vector2(Bounds.Width / Zoom / 2, Bounds.Height / Zoom / 2),
					cameraMax
				);
				posX = (int) clampPos.X;
				posY = (int) clampPos.Y;
			}

			Position = new Vector2(posX, posY);
		}

		/// <summary>
		/// Converts a location in screen position context to a location
		/// in world position context
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public Vector2 ScreenToWorldSpace(Vector2 source)
		{
			return Vector2.Transform(source, Matrix.Invert(TransformMatrix));
		}

		/// <summary>
		/// Updates the Camera
		/// </summary>
		public void Update()
		{
			RefreshMatrix();
		}

		private void RefreshMatrix()
		{
			TransformMatrix =
				Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
				Matrix.CreateScale(Zoom) *
				Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));

			RefreshView();
		}

		/// <summary>
		/// Updates visible area.
		/// <remarks>Used for culling and draw-optimization</remarks>
		/// </summary>
		private void RefreshView()
		{
			var inverseView = Matrix.Invert(TransformMatrix);

			var topLeft = Vector2.Transform(Vector2.Zero, inverseView);
			var topRight = Vector2.Transform(new Vector2(Bounds.X, 0), inverseView);
			var bottomLeft = Vector2.Transform(new Vector2(0, Bounds.Y), inverseView);
			var bottomRight = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseView);

			var min = new Vector2(
				MathHelper.Min(topLeft.X, MathHelper.Min(topRight.X, MathHelper.Min(bottomLeft.X, bottomRight.X))),
				MathHelper.Min(topLeft.Y, MathHelper.Min(topRight.Y, MathHelper.Min(bottomLeft.Y, bottomRight.Y))));
			var max = new Vector2(
				MathHelper.Max(topLeft.X, MathHelper.Max(topRight.X, MathHelper.Max(bottomLeft.X, bottomRight.X))),
				MathHelper.Max(topLeft.Y, MathHelper.Max(topRight.Y, MathHelper.Max(bottomLeft.Y, bottomRight.Y))));

			View = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
		}
	}
}