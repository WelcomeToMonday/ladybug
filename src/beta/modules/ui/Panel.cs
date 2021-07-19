using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.Beta.UI
{
	public class Panel : Control
	{
		public Texture2D BackgroundTexture
		{
			get => m_BackgroundTexture;
			set
			{
				if (m_BackgroundTexture != value)
				{
					m_BackgroundTexture = value;
					BuildBackground();
				}
			}
		}
		
		private Texture2D m_BackgroundTexture;

		private Texture2D _background;

		private void BuildBackground()
		{
			ThreadManager.QueueAction(() => {
				_background = Sprite.GetTextureFromMap(BackgroundTexture, new Vector2(Bounds.Width, Bounds.Height), Game.GraphicsDevice);
			});
		}
	}
}