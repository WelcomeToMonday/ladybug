using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.UI
{
	/// <summary>
	/// UI Control containing a simple <see cref="Ladybug.Graphics.Sprite"/>
	/// </summary>
	public class SpriteFrame : Control
	{
		/// <summary>
		/// Sprite contained by this SpriteFrame
		/// </summary>
		/// <value></value>
		public Sprite Sprite { get; set; }

		/// <summary>
		/// Called when this Control is drawn
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (Sprite != null && Sprite.Texture != null)
			{
				spriteBatch.Draw(
					Sprite.Texture,
					Bounds,
					Sprite.Frame,
					Color.White
				);
			}
		}
	}
}