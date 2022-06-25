using System;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	/// <summary>
	/// Contains extension methods for Polygons
	/// </summary>
	public static class PolygonExtensions
	{
		/// <summary>
		/// Convert this Polygon to a rectangle using its outer bounds.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="useBoundsAsFallback"></param>
		/// <remarks>
		/// If polygon is not already rectangular, will return the smallest possible rectangle containing
		/// the polygon
		/// </remarks>
		public static Rectangle ToRectangle(this Polygon p, bool useBoundsAsFallback = false)
		=> p.Bounds;
	}
}