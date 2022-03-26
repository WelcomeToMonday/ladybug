using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.UI
{
	/// <summary>
	/// Static class containing resource keys for common Panel resources
	/// </summary>
	public static class PanelResources
	{
		/// <summary>
		/// Resource key for a <see cref="Panel"/>'s default background.
		/// </summary>
		/// <remarks>If this key is assigned in a UI's ResourceCatalog, any Panels attached to that UI will use this resource as its initial background</remarks>
		public static readonly string DefaultBackground = "ladybug_panel_background_default";
	}

	/// <summary>
	/// Ladybug base Panel control
	/// </summary>
	/// <remarks>
	/// Panels specialize in containing child Controls, and update the position of child controls relative to the Panel's bounds.
	/// </remarks>
	public class Panel : ControlContainer
	{
		/// <summary>
		/// Type of background texture used by this Panel
		/// </summary>
		public TextureType BackgroundType
		{
			get => m_BackgroundType;
			set
			{
				if (m_BackgroundType != value)
				{
					m_BackgroundType = value;
					BuildBackground();
				}
			}
		}

		/// <summary>
		/// The texture used by the Panel to construct its background
		/// </summary>
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

		/// <summary>
		/// Source background texture used to build the background map
		/// </summary>
		/// <remarks>
		/// Used directly if BackgroundType is set to Basic
		/// </remarks>
		private Texture2D m_BackgroundTexture;

		private TextureType m_BackgroundType = TextureType.Map;

		/// <summary>
		/// Background generated from map
		/// </summary>
		/// <remarks>
		/// Set by BuildBackground(), used by Background property
		/// </remarks>
		private Texture2D _generatedBackground;

		/// <summary>
		/// Background property used by Draw()
		/// </summary>
		private Texture2D Background => BackgroundType == TextureType.Map ?
			_generatedBackground :
			m_BackgroundTexture;

		private void BuildBackground()
		{
			if (BackgroundTexture == null)
			{
				return;
			}

			ThreadManager.QueueAction(() =>
			{
				_generatedBackground = Sprite.GetTextureFromMap(BackgroundTexture, new Vector2(Bounds.Width, Bounds.Height), Game.GraphicsDevice);
			});
		}

		/// <summary>
		/// Called when the Panel is initialized, after it is attached to a parent control
		/// </summary>
		protected override void Initialize()
		{
			Texture2D texture = null;
			if (ResourceCatalog.TryGetResource<Texture2D>(UIResources.DefaultBackground, out Texture2D uiBackgroundTexture))
			{
				texture = uiBackgroundTexture;
			}

			if (ResourceCatalog.TryGetResource<Texture2D>(PanelResources.DefaultBackground, out Texture2D panelBackgroundTexture))
			{
				texture = panelBackgroundTexture;
			}

			if (texture != null)
			{
				BackgroundTexture = texture;
			}
		}

		/// <summary>
		/// Called when the Panel's bounds are updated
		/// </summary>
		/// <param name="oldBounds">The Panel's previous bounds</param>
		/// <param name="newBounds">The Panel's new bounds</param>
		protected override void UpdateBounds(Rectangle oldBounds, Rectangle newBounds)
		{
			Vector2 diff = newBounds.Location.ToVector2() - oldBounds.Location.ToVector2();
			foreach (var child in Controls)
			{
				child.Move(diff);
			}
		}

		/// <summary>
		/// Called when the Panel's <see cref="Control.Active"/> property is updated
		/// </summary>
		/// <param name="value"></param>
		protected override void ToggleActive(bool value)
		{
			foreach (var control in Controls)
			{
				control.Active = value;
			}
		}

		/// <summary>
		/// Called when the Panel's <see cref="Control.Visible"/> property is updated
		/// </summary>
		/// <param name="value"></param>
		protected override void ToggleVisible(bool value)
		{
			foreach (var control in Controls)
			{
				control.Visible = value;
			}
		}

		/// <summary>
		/// Called when the Panel is drawn
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (Visible)
			{
				if (Background != null)
				{
					spriteBatch.Draw(
						Background,
						Bounds,
						null,
						Color.White
					);
				}
				base.Draw(spriteBatch);
			}
		}
	}
}