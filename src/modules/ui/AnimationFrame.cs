using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.UI
{
	/// <summary>
	/// UI Control containing an <see cref="Ladybug.Graphics.AnimationSequence"/>
	/// </summary>
	public class AnimationFrame : Control
	{
		/// <summary>
		/// AnimationSequence contained by this control
		/// </summary>
		/// <value></value>
		public AnimationSequence Animation { get; set; }

		/// <summary>
		/// Called when the Control is drawn
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			Animation?.Play();
			Sprite sprite = Animation?.GetCurrentFrame();
			if (sprite != null)
			{
				spriteBatch.Draw(
					sprite.Texture,
					Bounds,
					sprite.Frame,
					Color.White
				);
			}
		}
	}
}