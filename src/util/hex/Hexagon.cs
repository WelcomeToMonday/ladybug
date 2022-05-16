using System;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	/// <summary>
	/// Six-sided convex polygon
	/// </summary>
	public class Hexagon : Polygon
	{
		private HexMatrix _matrix;

		/// <summary>
		/// Create a Hexagon
		/// </summary>
		/// <param name="location"></param>
		/// <param name="size"></param>
		/// <param name="orientation"></param>
		public Hexagon(Vector2 location, Vector2 size, HexOrientation orientation = Hex.DEFAULT_ORIENTATION)
		{
			Location = location;
			Size = size;
			Orientation = orientation;
			_matrix = HexMatrix.GetMatrix(orientation);
			Points = GetCorners();
		}

		/// <summary>
		/// Size of the hexagon, measured from the corners to the center
		/// </summary>
		/// <value></value>
		public Vector2 Size
		{
			get => m_Size;
			set
			{
				if (m_Size != value)
				{
					m_Size = value;
					Points = GetCorners();
				}
			}
		}
		private Vector2 m_Size = Vector2.Zero;

		/// <summary>
		/// Location of the hexagon
		/// </summary>
		/// <value></value>
		public Vector2 Location
		{
			get => m_Location;
			set
			{
				if (m_Location != value)
				{
					m_Location = value;
					Points = GetCorners();
				}
			}
		}
		private Vector2 m_Location = Vector2.Zero;

		/// <summary>
		/// Orientation of the hexagon
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
					_matrix = HexMatrix.GetMatrix(m_Orientation);
					Points = GetCorners();
				}
			}
		}
		private HexOrientation m_Orientation = Hex.DEFAULT_ORIENTATION;

		/// <summary>
		/// Retrieve the positions of the corners of the hexagon
		/// </summary>
		/// <returns></returns>
		public Vector2[] GetCorners()
		{
			var res = new Vector2[6];
			for (int i = 0; i < 6; i++)
			{
				res[i] = GetCornerOffset(i) + Location;
			}
			return res;
		}

		private Vector2 GetCornerOffset(int corner)
		{
			double angle = 2.0 * Math.PI * (_matrix.start_angle - corner) / 6.0;
			return new Vector2((float)(Size.X * Math.Cos(angle)), (float)(Size.Y * Math.Sin(angle)));
		}
	}
}