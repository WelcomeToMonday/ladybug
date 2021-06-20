using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug
{
	public class Camera
	{
		private float _zoomMin = 0.35f;
		private float _zoomMax = 10.0f;
		private bool _restrictedToArea = false;
		
    public Camera(Viewport viewport)
		{
			Bounds = viewport.Bounds;
		}

		public Vector2 Position { get; protected set; }

		public Rectangle Bounds { get; protected set; }

		public Rectangle AllowedArea { get; protected set; }

		public Rectangle View { get; protected set; }

		public Matrix Transform { get; protected set; }

		public float Zoom { get; protected set; } = 1.0f;

		public void ZoomBy(float z)
		{
			Zoom += z;
			if (Zoom < _zoomMin) Zoom = _zoomMin;
			if (Zoom > _zoomMax) Zoom = _zoomMax;
		}

		public void SetZoom(float z)
		{
			Zoom = z;
			if (Zoom < _zoomMin) Zoom = _zoomMin;
			if (Zoom > _zoomMax) Zoom = _zoomMax;
		}

		public void Move(Vector2 newPosition) => SetPosition(Position + newPosition);
		

		public void LerpTo(Vector2 newPosition, float by)
		{
			float rx = Lerp(Position.X, newPosition.X, by);
			float ry = Lerp(Position.Y, newPosition.Y, by);
			SetPosition(new Vector2(rx, ry));
		}

		public void SetAllowedArea(Rectangle r)
		{
			_restrictedToArea = true;
			AllowedArea = r;
		}

		public void ClearAllowedArea()
		{
			_restrictedToArea = false;
		}

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

    // See this for world/screen space notes
		// https://www.dreamincode.net/forums/topic/237979-2d-camera-in-xna/
		public Vector2 ScreenToWorldSpace(Vector2 source)
		{
			return new Vector2
			(
				View.Location.X + (source.X / Zoom),
				View.Location.Y + (source.Y / Zoom)
			);
		}

		public Vector2 WorldToScreenSpace(Vector2 source)
		{
			return new Vector2
			(
				(source.X - View.Location.X) * Zoom,
				(source.Y - View.Location.Y) * Zoom
			);
		}

		public void Update(Viewport vp)
		{
			Bounds = vp.Bounds;
			RefreshMatrix();
		}

		private float Lerp(float p1, float p2, float by)
		{
			return p1 + (p2 - p1) * by;
		}

		private void RefreshMatrix()
		{
			Transform =
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
			var inverseView = Matrix.Invert(Transform);

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