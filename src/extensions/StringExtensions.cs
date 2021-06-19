using System;

namespace Ladybug
{
	public static class StringExtensions
	{
		public static bool ToBool(this System.String value, bool defaultValue = false)
		{
			bool val = false;
			switch (value.ToLower())
			{
				case "t":
				case "true":
				case "1":
					val = true;
					break;
				case "f":
				case "false":
				case "0":
				case "":
					val = false;
					break;
				default:
					val = defaultValue;
					break;
			}
			return val;
		}
	}
}