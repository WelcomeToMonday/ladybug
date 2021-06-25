using System;

namespace Ladybug
{
	/// <summary>
	/// Static class containing String static helper and extension methods
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Gets bool representation of a string
		/// </summary>
		/// <param name="value"></param>
		/// <param name="throwOnFail">
		/// Whether to throw an exception upon failed conversion
		/// </param>
		/// <returns></returns>
		/// <remarks>
		/// If method is unable to determine the given
		/// string's value, it will return false
		/// </remarks>
		public static bool ToBool(this string value, bool throwOnFail = true)
		{
			if (value == null)
			{
				return false;
			}

			bool res = false;

			switch (value.ToLower())
			{
				case "t":
				case "true":
				case "1":
					res = true;
					break;
				case "f":
				case "false":
				case "0":
				case "":
					res = false;
					break;
				default:
					if (throwOnFail)
					{
						throw new InvalidOperationException($"String value {value} could not be converted to bool");
					}
					res = false;
					break;
			}
			return res;
		}
	}
}