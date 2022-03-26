using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Ladybug.Graphics
{
	/// <summary>
	/// Represents a drawable section of text
	/// </summary>
	public class TextSprite
	{

		private string _rawText;
		private List<FormattedString> _output;

		/// <summary>
		/// Create a TextSprite
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="bounds"></param>
		/// <returns></returns>
		public TextSprite(string text, SpriteFont font, Rectangle bounds = new Rectangle())
		{
			SetBounds(bounds);
			SetFont(font);
			SetText(text);
		}

		/// <summary>
		/// Create a TextSprite
		/// </summary>
		/// <param name="input"></param>
		/// <param name="defaultFont"></param>
		/// <param name="bounds"></param>
		/// <returns></returns>
		public TextSprite(List<FormattedString> input, SpriteFont defaultFont, Rectangle bounds = new Rectangle())
		{
			SetBounds(bounds);
			SetFont(defaultFont);
			_output = input;
		}

		/// <summary>
		/// The formatted text this TextSprite will render
		/// </summary>
		/// <value></value>
		public List<FormattedString> FormattedText
		{
			get
			{
				List<FormattedString> res = null;
				if (_output != null)
				{
					res = _output;
				}
				return res;
			}
		}

		/// <summary>
		/// This TextSprite's default font
		/// </summary>
		/// <value></value>
		public SpriteFont DefaultFont { get; private set; }

		/// <summary>
		/// This TextSprite's default text color
		/// </summary>
		/// <value></value>
		public Color DefaultTextColor { get; private set; } = Color.White;

		/// <summary>
		/// This TextSprite's default scale value
		/// </summary>
		/// <value></value>
		public float DefaultScale { get; private set; } = 1.0f;

		/// <summary>
		/// The Color Palette used by this TextSprite
		/// </summary>
		/// <value></value>
		public Dictionary<string, string> ColorPalette {get; private set;}

		/// <summary>
		/// The TextSprite's bounds
		/// </summary>
		/// <value></value>
		public Rectangle Bounds { get; private set; }

		/// <summary>
		/// The current length of the TextSprite's text content
		/// </summary>
		/// <value></value>
		public int Length
		{
			get
			{
				int res = 0;

				if (_output != null && _output.Count > 0)
				{
					foreach (var line in _output)
					{
						res += line.Text.Length;
					}
				}

				return res;
			}
		}

		/// <summary>
		/// Set the TextSprite's text content
		/// </summary>
		/// <param name="text"></param>
		public void SetText(string text)
		{
			_rawText = text;
			BuildOutput();
		}

		/// <summary>
		/// Set the TextSprite's font
		/// </summary>
		/// <param name="font"></param>
		public void SetFont(SpriteFont font)
		{
			DefaultFont = font;
		}

		/// <summary>
		/// Set the bounds of the TextSprite
		/// </summary>
		/// <param name="r"></param>
		public void SetBounds(Rectangle r)
		{
			Bounds = r;
		}

		/// <summary>
		/// Set the color of the TextSprite
		/// </summary>
		/// <param name="c"></param>
		public void SetColor(Color c)
		{
			DefaultTextColor = c;
		}

		/// <summary>
		/// Set the scale of the TextSprite
		/// </summary>
		/// <param name="s"></param>
		public void SetScale(float s)
		{
			DefaultScale = s;
		}

		/// <summary>
		/// Set the TextSprite's color palette
		/// </summary>
		/// <param name="palette"></param>
		public void SetPalette(Dictionary<string, string> palette) => ColorPalette = palette;

		/// <summary>
		/// Set the TextSprite's bounds to fit its text content
		/// </summary>
		public void SetBoundsToText()
		{
			//Vector2 dimensions = Font.MeasureString(Text);
			//WrapText = false;
			if (_output != null && _output.Count > 0)
			{
				float maxWidth = 0f;
				int lineHeight = _output[0].Font.LineSpacing;
				int maxHeight = lineHeight;

				var sb = new StringBuilder();

				foreach (var line in _output)
				{

					if (line.GetProperty("newline") == "true")
					{
						maxHeight += lineHeight;
						var linew = DefaultFont.MeasureString(sb.ToString()).X;
						maxWidth = linew > maxWidth ? linew : maxWidth;
						sb.Clear();
					}
					sb.Append(line.Text);
					var linew2 = DefaultFont.MeasureString(sb.ToString()).X;
					maxWidth = linew2 > maxWidth ? linew2 : maxWidth;
				}
				SetBounds(new Rectangle(Bounds.X, Bounds.Y, (int)maxWidth, maxHeight));
			}
		}


		private void BuildOutput()
		{
			if (_output != null)
			{
				_output.Clear();
			}

			var f = FormattedString.Format(_rawText, DefaultFont);
			float lineWidth = 0f;
			for (var i = 0; i < f.Count; i++)
			{
				string[] words = f[i].Text.Split(' ');
				StringBuilder sb = new StringBuilder();

				SpriteFont font = DefaultFont; //ToDo: Add font prop support;
				float spaceWidth = font.MeasureString(" ").X;

				for (var j = 0; j < words.Length; j++)
				{
					var word = words[j];
					if (word == "")
					{
						continue;
					}
					Vector2 size = font.MeasureString(word);

					if (lineWidth + size.X < Bounds.Width)
					{
						sb.Append(word + " ");
						lineWidth += size.X + spaceWidth;
					}
					else
					{
						f[i].Text = sb.ToString();
						sb.Clear();
						sb.Append(word + " ");
						var newStr = new FormattedString();
						//newStr.Properties = f[i].Properties;
						newStr.CopyProperties(f[i]);
						newStr.SetProperty("newline", "true");
						//newStr.ProcessProperties();
						f.Insert(i + 1, newStr);
						i++;
						lineWidth = size.X + spaceWidth;
					}
				}
				if (sb.Length > 0)
				{
					f[i].Text = sb.ToString();
					sb.Clear();
				}
			}
			if (f.Count > 0)
			{
				f[f.Count - 1].Text = f[f.Count - 1].Text.TrimEnd(' ');
			}
			_output = f;
		}

		/// <summary>
		/// Draw the TextSprite
		/// </summary>
		/// <param name="spriteBatch"></param>
		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 offset = Bounds.Location.ToVector2();
			for (var i = 0; i < _output.Count; i++)
			{
				var str = _output[i];

				if (str.GetProperty("font") != "")
				{
					// ToDo: Add font customization here?
				}
				else
				{
					str.SetFont(DefaultFont);
				}

				if (str.GetProperty("scale") != "")
				{
					str.SetScale(float.Parse(str.GetProperty("scale")));
				}
				else
				{
					str.SetScale(DefaultScale);
				}

				if (ColorPalette != null && ColorPalette.Count > 0)
				{
					str.SetPalette(ColorPalette);
				}

				if (str.GetProperty("newline") == "true")
				{
					offset = new Vector2(Bounds.Location.X, offset.Y + str.Font.LineSpacing);
				}

				if (str.GetProperty("color") == "")
				{
					str.SetColor(DefaultTextColor);
				}

				if (str.Text != null && str.Text.Length > 0)
				{
					spriteBatch.DrawString(
						str.Font,
						str.Text,
						offset,
						str.Color,
						str.Rotation,
						Vector2.Zero,
						str.Scale,
						SpriteEffects.None,
						0f
					);
				offset = new Vector2(offset.X + str.Font.MeasureString(str.Text).X, offset.Y);
				}
			}
		}
	}
}