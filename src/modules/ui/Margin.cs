namespace Ladybug.UI
{
	/// <summary>
	/// Ladybug UI Margin
	/// </summary>
	/// <remarks>
	/// Used by some Ladybug UI controls to determine
	/// margin values
	/// </remarks>
	public struct Margin
	{
		/// <summary>
		/// A margin with a zero value for all sides
		/// </summary>
		public static readonly Margin Zero;

		/// <summary>
		/// Top margin value
		/// </summary>
		public int Top;
		/// <summary>
		/// Right margin value
		/// </summary>
		public int Right;
		/// <summary>
		/// Bottom margin value
		/// </summary>
		public int Bottom;
		/// <summary>
		/// Left margin value
		/// </summary>
		public int Left;

		static Margin()
		{
			Zero = new Margin(0, 0, 0, 0);
		}

		/// <summary>
		/// Create new Margin
		/// </summary>
		/// <param name="top"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		/// <param name="left"></param>
		public Margin(int top, int right, int bottom, int left)
		{
			Top = top;
			Right = right;
			Bottom = bottom;
			Left = left;
		}

		/// <summary>
		/// Create new Margin
		/// </summary>
		/// <param name="top"></param>
		/// <param name="sides"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
		public Margin(int top, int sides, int bottom) : this(top, sides, bottom, sides) { }

		/// <summary>
		/// Create new Margin
		/// </summary>
		/// <param name="topBottom"></param>
		/// <param name="sides"></param>
		/// <returns></returns>
		public Margin(int topBottom, int sides) : this(topBottom, sides, topBottom, sides) { }

		/// <summary>
		/// Create new Margin
		/// </summary>
		/// <param name="margin"></param>
		/// <returns></returns>
		public Margin(int margin) : this(margin, margin, margin, margin) { }

		/// <summary>
		/// Checks if object is equivalent to this Margin
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is Margin)
			{
				var m = (Margin)obj;
				return
					m.Top == Top &&
					m.Right == Right &&
					m.Bottom == Bottom &&
					m.Left == Left;
			}
			else
			{
				return false;
			}
		}

		public static bool operator ==(Margin lhs, Margin rhs) => lhs.Equals(rhs);
		public static bool operator !=(Margin lhs, Margin rhs) => !lhs.Equals(rhs);
	}
}