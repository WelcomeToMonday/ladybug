using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.UI
{
	/// <summary>
	/// Static class containing resource keys for common Label resources
	/// </summary>
	public static class LabelResources
	{
		/// <summary>
		/// Unique string key for default Label font
		/// </summary>
		public static readonly string DefaultFont = "ladybug_label_default_font";
	}

	/// <summary>
	/// Ladybug base Label control
	/// </summary>
	public class Label : Control
	{
		private TextSprite _textSprite;

		/// <summary>
		/// The Label's text content
		/// </summary>
		public string Text
		{
			get => m_Text;
			set
			{
				if (m_Text != value)
				{
					m_Text = value;
					SetText(m_Text);
				}
			}
		}

		/// <summary>
		/// The Label's Text Color
		/// </summary>
		public Color TextColor
		{
			get => _textSprite.DefaultTextColor;
			set => _textSprite.SetColor(value);
		}

		/// <summary>
		/// The Label's Font
		/// </summary>
		public SpriteFont Font
		{
			get => m_Font;
			set
			{
				if (m_Font != value)
				{
					m_Font = value;
					SetText(Text);
				}
			}
		}

		private string m_Text = "";
		private SpriteFont m_Font;

		/// <summary>
		/// Called when the label is initialized
		/// </summary>
		//protected override void Initialize()
		protected override void Attach(IControlContainer parentControl)
		{
			if (ResourceCatalog.TryGetResource<SpriteFont>(UIResources.DefaultFont, out SpriteFont uiDefaultFont))
			{
				Font = uiDefaultFont;
			}

			if (ResourceCatalog.TryGetResource<SpriteFont>(LabelResources.DefaultFont, out SpriteFont labelDefaultFont))
			{
				Font = labelDefaultFont;
			}

			_textSprite = new TextSprite("", Font);
		}

		private void SetText(string text)
		{
			if (Font == null || _textSprite == null)
			{
				return;
			}
			Text = text;
			_textSprite.SetBounds(new Rectangle(Bounds.Location.X, Bounds.Location.Y, int.MaxValue, int.MaxValue));
			_textSprite.SetText(text);
			_textSprite.SetBoundsToText();
			SetBounds(_textSprite.Bounds);
		}

		/// <summary>
		/// Called when the Label's bounds are updated
		/// </summary>
		/// <param name="oldBounds"></param>
		/// <param name="newBounds"></param>
		protected override void UpdateBounds(Rectangle oldBounds, Rectangle newBounds)
		{
			_textSprite.SetBounds(newBounds);
		}

		/// <summary>
		/// Called when the Label is drawn
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			_textSprite.Draw(spriteBatch);
		}
	}
}