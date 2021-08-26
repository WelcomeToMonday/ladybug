using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.UI
{
	/// <summary>
	/// UI Control containing a simple texture
	/// </summary>
	public class TextureFrame : Control
	{
		/// <summary>
		/// Image this TextureFrame contains
		/// </summary>
		/// <value></value>
		public Texture2D Texture { get; set; }

		/// <summary>
		/// Called when this control is drawn
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (Texture != null)
			{
				spriteBatch.Draw(
					Texture,
					Bounds,
					null,
					Color.White
				);
			}
		}
	}
}