#pragma warning disable 1591 // Hide XMLdoc warnings.

namespace Ladybug.UI
{
	/// <summary>
	/// Type of texture, Map or Basic
	/// </summary>
	public enum TextureType
	{
		/// <summary>
		/// Standard texture
		/// </summary>
		Basic,
		/// <summary>
		/// 9-slice texture map
		/// </summary>
		Map
	};

	public enum VerticalAlignment
	{
		Top,
		Center,
		Bottom,
	}

	public enum HorizontalAlignment
	{
		Left,
		Center,
		Right
	}

	public enum Orientation
	{
		Vertical,
		Horizontal
	}
}
#pragma warning restore 1591