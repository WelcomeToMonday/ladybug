using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.Beta.UI
{
	public class Panel : Control
	{
		public static string DefaultBackground = "ladybug_ui_panel_background_default";

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

		public Panel()
		{
			OnInitialize(Initialize);
		}

		private void Initialize() 
		{
			//devnote: this will be messy with subclassing. We'll need to address this
			if (ResourceCatalog.TryGetResource<Texture2D>(DefaultBackground, out Texture2D background))
			{
				BackgroundTexture = background;
			}
		}

		private void BuildBackground()
		{
			ThreadManager.QueueAction(() => {
				_background = Sprite.GetTextureFromMap(BackgroundTexture, new Vector2(Bounds.Width, Bounds.Height), Game.GraphicsDevice);
			});
		}
	}
}