using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Ladybug.Graphics
{
	public enum ColorFormat { HEX, NAME, RGB }
	public static class ColorExtensions
	{
		public static string GetHexString(this Color c)
		{
			return $"{c.R:X2}{c.G:X2}{c.B:X2}";
		}

		public static Color GetColorFromHex(string hex)
		{
			var value = hex.TrimStart('#');

			var hexr = Convert.ToInt32(value.Substring(0, 2), 16);
			var hexg = Convert.ToInt32(value.Substring(2, 2), 16);
			var hexb = Convert.ToInt32(value.Substring(4, 2), 16);

			return new Color(hexr, hexg, hexb);
		}

		public static string GetHexFromColor(Color color)
		{
			return color.GetHexString();
		}

		public static Color ParseColor(string color, Dictionary<string, string> pallete)
		{
			var res = Color.Black;
			
			if (color.Contains("$"))
			{
				var sColor = color.TrimStart('$').Trim();
				if (pallete.ContainsKey(sColor))
				{
					res = ParseColor(pallete[sColor]);
				}
			}
			else
			{
				res = ParseColor(color);
			}

			return res;
		}

		public static Color ParseColor(string color)
		{
			var res = Color.Black;
			var cType = ColorFormat.NAME;

			if (color.Contains("#")) cType = ColorFormat.HEX;
			if (color.Contains(",")) cType = ColorFormat.RGB;

			switch (cType)
			{
				default:
				case ColorFormat.NAME:
					object s = null;
					s = res.GetType().GetProperty(color).GetValue(s, null);
					res = (Color)s;
					break;

				case ColorFormat.HEX:
					res = ColorExtensions.GetColorFromHex(color);
					break;

				case ColorFormat.RGB:
					var values = color.Split(',');

					var rgbr = int.Parse(values[0]);
					var rgbg = int.Parse(values[1]);
					var rgbb = int.Parse(values[2]);

					res = new Color(rgbr, rgbg, rgbb);
					break;
			}
			return res;
		}
	}
}