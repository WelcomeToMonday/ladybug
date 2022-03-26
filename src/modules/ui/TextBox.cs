using System;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Ladybug.Graphics;
using Ladybug.UserInput;

namespace Ladybug.UI
{
	/// <summary>
	/// Static class containing unique resource keys for TextBoxes
	/// </summary>
	public static class TextBoxResources
	{
		/// <summary>
		/// Unique string key for default TextBox background
		/// </summary>
		public static readonly string DefaultBackground = "ladybug_textbox_background_default";

		/// <summary>
		/// Unique string key for default TextBox font
		/// </summary>
		public static readonly string DefaultFont = "ladybug_textbox_font_default";
	}

	/// <summary>
	/// Ladybug base text input control
	/// </summary>
	public class TextBox : Panel
	{
		/// <summary>
		/// Text content has been submitted
		/// </summary>
		public event EventHandler Submitted;

		private Label _label;

		private StringBuilder _stringBuilder;

		/// <summary>
		/// The TextBox's text content
		/// </summary>
		/// <value></value>
		public string Text
		{
			get => _label.Text;
			set
			{
				_label.Text = value;
			}
		}

		/// <summary>
		/// Label text offset from top-left corner of TextBox bounds
		/// </summary>
		/// <value></value>
		public Vector2 TextOffset
		{
			get => m_TextOffset;
			set
			{
				if (m_TextOffset != value)
				{
					m_TextOffset = value;
					SetBounds(Bounds);
				}
			}
		}
		private Vector2 m_TextOffset = Vector2.Zero;

		/// <summary>
		/// Color of the TextBox's text content
		/// </summary>
		/// <value></value>
		public Color TextColor
		{
			get => _label.TextColor;
			set => _label.TextColor = value;
		}

		/// <summary>
		/// Font used by the TextBox's text content
		/// </summary>
		/// <value></value>
		public SpriteFont Font
		{
			get => _label.Font;
			set => _label.Font = value;
		}

		/// <summary>
		/// Key used to submit TextBox content
		/// </summary>
		/// <value></value>
		public Keys SubmitKey { get; set; } = Keys.Enter;

		/// <summary>
		/// Maximum character limit
		/// </summary>
		/// <value></value>
		public int MaxCharacters { get; set; } = 20;

		/// <summary>
		/// Called when the TextBox is initialized
		/// </summary>
		protected override void Initialize()
		{
			_stringBuilder = new StringBuilder();
			if (ResourceCatalog.TryGetResource<Texture2D>(TextBoxResources.DefaultBackground, out Texture2D defaultBackground))
			{
				BackgroundTexture = defaultBackground;
			}

			SpriteFont font = null;

			if (ResourceCatalog.TryGetResource<SpriteFont>(UIResources.DefaultFont, out SpriteFont uiDefaultFont))
			{
				font = uiDefaultFont;
			}

			if (ResourceCatalog.TryGetResource<SpriteFont>(TextBoxResources.DefaultFont, out SpriteFont textboxDefaultFont))
			{
				font = textboxDefaultFont;
			}

			AddControl<Label>(out _label);

			Font = font;
		}

		/// <summary>
		/// Called when the TextBox is clicked on
		/// </summary>
		/// <param name="state"></param>
		protected override void Click(InputState state)
		{
			if (state == InputState.Released)
			{
				UI.SetFocus(this);
			}
		}

		/// <summary>
		/// Called when the TextBox gains focus
		/// </summary>
		protected override void Focus()
		{
			Game.Window.TextInput += HandleTextInput;
		}

		/// <summary>
		/// Called when the TextBox has focus, but an
		/// area outside its bounds is clicked
		/// </summary>
		protected override void ClickOut()
		{
			if (HasFocus)
			{
				UI.ClearFocus();
			}
		}

		/// <summary>
		/// Called when the TextBox loses focus
		/// </summary>
		protected override void Unfocus()
		{
			Game.Window.TextInput -= HandleTextInput;
		}

		/// <summary>
		/// Called when the TextBox's bounds are updated
		/// </summary>
		/// <param name="oldBounds"></param>
		/// <param name="newBounds"></param>
		protected override void UpdateBounds(Rectangle oldBounds, Rectangle newBounds)
		{
			_label.SetPosition(Bounds.Location.ToVector2() + TextOffset);
		}

		private void HandleTextInput(object sender, TextInputEventArgs e)
		{
			var glpyhs = Font.GetGlyphs();

			if (e.Key == Keys.Back)
			{
				if (_stringBuilder.Length > 0)
				{
					_stringBuilder.Remove(_stringBuilder.Length - 1, 1);
				}
			}
			else if (e.Key != SubmitKey && _stringBuilder.Length <= MaxCharacters)
			{
				if (glpyhs.ContainsKey(e.Character))
				{
					_stringBuilder.Append(e.Character);
				}
			}
			else if (e.Key == SubmitKey)
			{
				Submitted?.Invoke(this, new EventArgs());
			}

			_label.Text = _stringBuilder.ToString();
		}

		/// <summary>
		/// Clears the TextBox's text content
		/// </summary>
		public void ClearText()
		{
			_label.Text = "";
			_stringBuilder = new StringBuilder();
		}
	}
}