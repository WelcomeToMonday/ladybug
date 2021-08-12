using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Ladybug.Graphics
{
	/// <summary>
	/// Enum containing available color formats
	/// </summary>
	public enum ColorFormat
	{
		/// <summary>
		/// Hexadecimal color format
		/// </summary>
		HEX,
		/// <summary>
		/// Name color format
		/// </summary>
		/// <remarks>
		/// Valid names as defined as static
		/// properties of Microsoft.Xna.Framework.Color
		/// </remarks>
		NAME,
		/// <summary>
		/// RGB color format
		/// </summary>
		RGB
	}

	/// <summary>
	/// Static class containing Color static helper and extension methods
	/// </summary>
	public static class ColorExtensions
	{
		/// <summary>
		/// Returns a hexadecimal string representation
		/// of the given Color
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static string GetHexString(this Color c)
		{
			return $"{c.R:X2}{c.G:X2}{c.B:X2}";
		}

		/// <summary>
		/// Returns a Color representation of the given
		/// hexadecimal color strings
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		public static Color GetColorFromHex(string hex)
		{
			var value = hex.TrimStart('#');

			var hexr = Convert.ToInt32(value.Substring(0, 2), 16);
			var hexg = Convert.ToInt32(value.Substring(2, 2), 16);
			var hexb = Convert.ToInt32(value.Substring(4, 2), 16);

			return new Color(hexr, hexg, hexb);
		}

		/// <summary>
		/// Returns a hexadecimal string representation
		/// of the given Color
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static string GetHexFromColor(Color color)
		{
			return color.GetHexString();
		}

		/// <summary>
		/// Attempts to return a color representation of a string
		/// </summary>
		/// <param name="color"></param>
		/// <param name="result"></param>
		/// <param name="pallete"></param>
		/// <returns></returns>
		public static bool TryParseColor(string color, out Color result, Dictionary<string, string> pallete)
		{
			var success = false;
			result = default(Color);

			if (color.Contains("$"))
			{
				var sColor = color.TrimStart('$').Trim();
				if (pallete.ContainsKey(sColor) && TryParseColor(pallete[sColor], out result))
				{
					success = true;
				}
			}
			else
			{
				if (TryParseColor(color, out result))
				{
					success = true;
				}
			}

			return success;
		}

		/// <summary>
		/// Attempts to return the color representation of a string
		/// </summary>
		/// <param name="color"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public static bool TryParseColor(string color, out Color result)
		{
			var success = false;
			result = default(Color);
			
			var cType = ColorFormat.NAME;
			if (color.Contains("#")) cType = ColorFormat.HEX;
			if (color.Contains(",")) cType = ColorFormat.RGB;

			switch (cType)
			{
				default:
				case ColorFormat.NAME:
					object s = null;
					s = typeof(Color).GetProperty(color).GetValue(s, null);
					result = (Color)s;
					success = true;
					break;

				case ColorFormat.HEX:
					result = ColorExtensions.GetColorFromHex(color);
					success = true;
					break;

				case ColorFormat.RGB:
					var values = color.Split(',');

					var rgbr = int.Parse(values[0]);
					var rgbg = int.Parse(values[1]);
					var rgbb = int.Parse(values[2]);

					result = new Color(rgbr, rgbg, rgbb);
					success = true;
					break;
			}
			return success;
		}
	}
}