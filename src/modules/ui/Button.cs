using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.UI
{
	/// <summary>
	/// Static class containing unique resource keys for Buttons
	/// </summary>
	public static class ButtonResources
	{
		/// <summary>
		/// Unique string key for default Button background
		/// </summary>
		public static readonly string DefaultBackground = "ladybug_button_background_default";

		/// <summary>
		/// Unique string key for default Button font
		/// </summary>
		public static readonly string DefaultFont = "ladybug_button_font_default";
	}

	/// <summary>
	/// Ladybug base Button control
	/// </summary>
	public class Button : Panel
	{
		private Label _label;

		/// <summary>
		/// The Button's text content
		/// </summary>
		/// <value></value>
		public string Text
		{
			get => _label.Text;
			set
			{
				_label.Text = value;
				SetBounds(Bounds); // re-center text
			}
		}

		/// <summary>
		/// The color of the Button's text
		/// </summary>
		/// <value></value>
		public Color TextColor
			{
			get => _label.TextColor;
			set => _label.TextColor = value;
			}

		/// <summary>
		/// The Button's font
		/// </summary>
		/// <value></value>
		public SpriteFont Font
		{
			get => _label.Font;
			set => _label.Font = value;
		}

		/// <summary>
		/// Called when the Button is initialized
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			if (ResourceCatalog.TryGetResource<Texture2D>(ButtonResources.DefaultBackground, out Texture2D defaultBackground))
			{
				BackgroundTexture = defaultBackground;
			}

			AddControl<Label>(out _label);

			if (ResourceCatalog.TryGetResource<SpriteFont>(ButtonResources.DefaultFont, out SpriteFont defaultFont))
			{
				_label.Font = defaultFont;
			}
		}

		/// <summary>
		/// Called when the Button's bounds are updated
		/// </summary>
		/// <param name="oldBounds"></param>
		/// <param name="newBounds"></param>
		protected override void UpdateBounds(Rectangle oldBounds, Rectangle newBounds)
		{
			_label.SetBounds(_label.Bounds.CopyAtPosition(Bounds.GetHandlePosition(BoxHandle.Center), BoxHandle.Center));
		}
	}
}