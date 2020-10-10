using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

using Microsoft.Xna.Framework;

namespace Ladybug.Core
{
	public static class Extensions
	{
		public static XmlReader GetReader(this XmlDocument document) => new XmlNodeReader(document);

		public static Feed<T> GetFeed<T>(this List<T> list, int MaxLines, int startOffset = 0) => new Feed<T>(list, MaxLines, startOffset);

		public static float ToAngle(this Vector2 vector)
		{
			return (float)Math.Atan2(vector.Y, vector.X);
		}

		public static Vector2 ToVector(this float angle)
		{
			return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}

		public static Point ToPoint(this Vector2 vector)
		{
			return new Point((int)vector.X, (int)vector.Y);
		}

		public static float AngleTo(this Vector2 p1, Vector2 p2)
		{
			float xdiff = p2.X - p1.X;
			float ydiff = p2.Y - p1.Y;
			return (float)Math.Atan2(ydiff, xdiff);
		}

		public static float DistanceTo(this Vector2 p1, Vector2 p2)
		{
			return (float)Math.Sqrt((Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2)));
		}
	}
}