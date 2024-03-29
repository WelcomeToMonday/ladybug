// Based on articles found at http://www.redblobgames.com/grids/hexagons/
using System;

namespace Ladybug
{
	/// <summary>
	/// Fractional Hex Coordinate
	/// </summary>
	public struct FHex
	{
		/// <summary>
		/// Create a fractional hex coordinate
		/// </summary>
		/// <param name="q"></param>
		/// <param name="r"></param>
		/// <param name="s"></param>
		public FHex(double q, double r, double s)
		{
			Q = q;
			R = r;
			S = s;

			if (Math.Round(q + r + s) != 0)
			{
				throw new ArgumentException("q + r + s must be 0");
			}
		}

		/// <summary>
		/// Q axis position
		/// </summary>
		/// <value></value>
		public readonly double Q { get; }

		/// <summary>
		/// R axis position
		/// </summary>
		/// <value></value>
		public readonly double R { get; }

		/// <summary>
		/// S axis position
		/// </summary>
		/// <value></value>
		public readonly double S { get; }

		/// <summary>
		/// Get the closest non-fractional Hex coordinate
		/// </summary>
		/// <returns></returns>
		public Hex Round()
		{
			int qi = (int)(Math.Round(Q));
			int ri = (int)(Math.Round(R));
			int si = (int)(Math.Round(S));

			double q_diff = Math.Abs(qi - Q);
			double r_diff = Math.Abs(ri - R);
			double s_diff = Math.Abs(si - S);

			if (q_diff > r_diff && q_diff > s_diff)
			{
				qi = -ri - si;
			}
			else if (r_diff > s_diff)
			{
				ri = -qi - si;
			}
			else
			{
				si = -qi - ri;
			}

			return new Hex(qi, ri, si);
		}

		/// <summary>
		/// Linearly interpolate to the given FHex
		/// </summary>
		/// <param name="b"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public FHex Lerp(FHex b, double t)
		=> new FHex(Q * (1.0 - t) + b.Q * t, R * (1.0 - t) + b.R * t, S * (1.0 - t) + b.S * t);
	}
}