// https://codereview.stackexchange.com/questions/175480/convert-the-string-from-nested-parentheses-to-indented-outline-format
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Graphics
{
	public class FormattedString
	{
		private enum ColorType {NAME, HEX, RGB}
		private const char OPEN_TOKEN = '[';
		private const char CLOSE_TOKEN = ']';

		private SpriteFont m_font;

		public FormattedString() { }

		public FormattedString(string text, SpriteFont defaultFont)
		{
			Text = FormatFragment(text);
			DefaultFont = defaultFont;
			ProcessProperties();
		}

		public SpriteFont DefaultFont { get; private set; }

		public SpriteFont Font
		{
			get
			{
				return m_font == null ? DefaultFont : m_font;
			}
			set
			{
				m_font = value;
			}
		}

		public Color Color { get; private set; } = Color.White;

		public float Rotation { get; private set; } = 0.0f;

		public float Scale { get; private set; } = 1.0f;

		public string Text { get; set; }

		public Dictionary<string, string> Pallete {get; private set;} = new Dictionary<string, string>();

		public Dictionary<string, string> Properties { get; private set; } = new Dictionary<string, string>();

		public static List<FormattedString> Format(string input, SpriteFont defaultFont)
		{
			List<FormattedString> output = new List<FormattedString>();
			StringBuilder sb = new StringBuilder();
			var glyphs = defaultFont.GetGlyphs();

			foreach (var c in input)
			{
				switch (c)
				{
					case OPEN_TOKEN:
						if (sb.Length > 0)
						{
							output.Add(new FormattedString(sb.ToString(), defaultFont));
						}
						sb.Clear();
						break;

					case CLOSE_TOKEN:
						output.Add(new FormattedString(sb.ToString(), defaultFont));
						sb.Clear();
						break;

					default:
						if (glyphs.ContainsKey(c))
						{
							sb.Append(c);
						}
						break;
				}
			}

			if (sb.Length > 0)
			{
				output.Add(new FormattedString(sb.ToString(), defaultFont));
			}

			return output;
		}

		public void SetProperty(string key, string value)
		{
			Properties[key] = value;
			ProcessProperties();
		}

		public void ClearProperty(string key)
		{
			if (Properties.ContainsKey(key))
			{
				Properties.Remove(key);
				ProcessProperties();
			}
		}

		public string GetProperty(string key)
		{
			string res = "";
			if (Properties.ContainsKey(key))
			{
				res = Properties[key];
			}
			return res;
		}

		public void SetColor(Color c)
		{
			Color = c;
		}

		public void SetPalette(Dictionary<string, string> palette)
		{
			Pallete = palette;
		}

		public void SetFont(SpriteFont font)
		{
			Font = font;
		}

		public void SetScale(float scale)
		{
			Scale = scale;
		}

		public void CopyProperties(FormattedString source)
		{
			Properties = new Dictionary<string, string>(source.Properties);
		}

		public virtual void ProcessProperties()
		{
			foreach (var prop in Properties)
			{
				if (prop.Key == "color")
				{
					//Color = ColorExtensions.ParseColor(prop.Value, Pallete);
					if (ColorExtensions.TryParseColor(prop.Value, out Color color, Pallete))
					{
						Color = color;
					}
				}
			}
		}

		private string FormatFragment(string rawText)
		{
			var splitText = rawText.Split('|');
			var res = "";
			if (splitText.Length > 1)
			{
				res = splitText[1];
				var props = splitText[0].Split(';');
				foreach (var p in props)
				{
					var kv = p.Split(':');
					//Properties.Add(kv[0], kv[1]);
					SetProperty(kv[0], kv[1]);
				}
			}
			else
			{
				res = rawText;
			}

			return res;
		}
	}
}