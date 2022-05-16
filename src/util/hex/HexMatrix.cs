#pragma warning disable 1591 // Hide XMLdoc warnings.
using System;

namespace Ladybug
{
	/// <summary>
	/// Matrix used in mathematical hex operations
	/// </summary>
	public struct HexMatrix
	{
		public static readonly HexMatrix Flat;
		public static readonly HexMatrix Point;

		static HexMatrix()
		{
			Flat = new HexMatrix(3.0 / 2.0, 0.0, Math.Sqrt(3.0) / 2.0, Math.Sqrt(3.0), 2.0 / 3.0, 0.0, -1.0 / 3.0, Math.Sqrt(3.0) / 3.0, 0.0);
			Point = new HexMatrix(Math.Sqrt(3.0), Math.Sqrt(3.0) / 2.0, 0.0, 3.0 / 2.0, Math.Sqrt(3.0) / 3.0, -1.0 / 3.0, 0.0, 2.0 / 3.0, 0.5);
		}

		public static HexMatrix GetMatrix(HexOrientation orientation)
		{
			HexMatrix m;
			switch (orientation)
			{
				default:
				case HexOrientation.Point:
					m = Point;
					break;
				case HexOrientation.Flat:
					m = Flat;
					break;
			}
			return m;
		}

		/// <summary>
		/// Create a HexMatrix
		/// </summary>
		/// <param name="f0"></param>
		/// <param name="f1"></param>
		/// <param name="f2"></param>
		/// <param name="f3"></param>
		/// <param name="b0"></param>
		/// <param name="b1"></param>
		/// <param name="b2"></param>
		/// <param name="b3"></param>
		/// <param name="start_angle"></param>
		public HexMatrix(double f0, double f1, double f2, double f3, double b0, double b1, double b2, double b3, double start_angle)
		{
			this.f0 = f0;
			this.f1 = f1;
			this.f2 = f2;
			this.f3 = f3;
			this.b0 = b0;
			this.b1 = b1;
			this.b2 = b2;
			this.b3 = b3;
			this.start_angle = start_angle;
		}

		public readonly double f0;
		public readonly double f1;
		public readonly double f2;
		public readonly double f3;
		public readonly double b0;
		public readonly double b1;
		public readonly double b2;
		public readonly double b3;
		public readonly double start_angle;
	}
}
#pragma warning restore 1591