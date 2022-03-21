using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

using Microsoft.Xna.Framework;

namespace Ladybug
{
	/// <summary>
	/// Static class containing common Ladybug static helper and extension methods
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Returns an XmlReader for the given XmlDocument
		/// </summary>
		/// <param name="document"></param>
		/// <returns></returns>
		public static XmlReader GetReader(this XmlDocument document) => new XmlNodeReader(document);

		/// <summary>
		/// Creates a new <see cref="Ladybug.Feed{T}"/> from a List
		/// </summary>
		/// <param name="list">Source List for <see cref="Ladybug.Feed{T}"/> </param>
		/// <param name="MaxLines">Maximum number of lines in <see cref="Ladybug.Feed{T}"/></param>
		/// <param name="startOffset">Start line offset for <see cref="Ladybug.Feed{T}"/></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static Feed<T> GetFeed<T>(this List<T> list, int MaxLines, int startOffset = 0) => new Feed<T>(list, MaxLines, startOffset);

		/// <summary>
		/// Gets an angle representation of a Vector2
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static float ToAngle(this Vector2 vector)
		{
			return (float)Math.Atan2(vector.Y, vector.X);
		}

		/// <summary>
		/// Gets a Vector2 representation of an angle
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static Vector2 ToVector2(this float angle)
		{
			return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}

		/// <summary>
		/// Gets a Point representation of a Vector2
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static Point ToPoint(this Vector2 vector)
		{
			return new Point((int)vector.X, (int)vector.Y);
		}

		/// <summary>
		/// Gets the angle between two Vector2s
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		public static float AngleTo(this Vector2 p1, Vector2 p2)
		{
			float xdiff = p2.X - p1.X;
			float ydiff = p2.Y - p1.Y;
			return (float)Math.Atan2(ydiff, xdiff);
		}

		/// <summary>
		/// Gets the distance between two Vector2s
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		public static float DistanceTo(this Vector2 p1, Vector2 p2)
		{
			return (float)Math.Sqrt((Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2)));
		}

		/// <summary>
		/// Linear Interpolation
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="by"></param>
		/// <returns></returns>
		public static float Lerp(float p1, float p2, float by)
		{
			return p1 + (p2 - p1) * by;
		}

		// see: https://www.icode.com/c-function-for-a-bezier-curve/
		/// <summary>
		/// Interpolate a Bezier curve
		/// </summary>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="p3"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public static Vector2 Bezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
		{
			float cx = 3 * (p1.X - p0.X);
			float cy = 3 * (p1.Y - p0.Y);
			float bx = 3 * (p2.X - p1.X) - cx;
			float by = 3 * (p2.Y - p1.Y) - cy;
			float ax = p3.X - p0.X - cx - bx;
			float ay = p3.Y - p0.Y - cy - by;
			float Cube = t * t * t;
			float Square = t * t;

			float resX = (ax * Cube) + (bx * Square) + (cx * t) + p0.X;
			float resY = (ay * Cube) + (by * Square) + (cy * t) + p0.Y;

			return new Vector2(resX, resY);
		}
	}
}