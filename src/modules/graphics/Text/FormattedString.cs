// https://codereview.stackexchange.com/questions/175480/convert-the-string-from-nested-parentheses-to-indented-outline-format
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Graphics
{
	/// <summary>
	/// Represents a string containing formatting metadata
	/// </summary>
	public class FormattedString
	{
		private enum ColorType {NAME, HEX, RGB}
		private const char OPEN_TOKEN = '[';
		private const char CLOSE_TOKEN = ']';

		private SpriteFont m_font;

		/// <summary>
		/// Create a FormattedString
		/// </summary>
		public FormattedString() { }

		/// <summary>
		/// Create a FormattedString
		/// </summary>
		/// <param name="text"></param>
		/// <param name="defaultFont"></param>
		public FormattedString(string text, SpriteFont defaultFont)
		{
			Text = FormatFragment(text);
			DefaultFont = defaultFont;
			ProcessProperties();
		}

		/// <summary>
		/// The FormattedString's default font
		/// </summary>
		/// <value></value>
		public SpriteFont DefaultFont { get; private set; }

		/// <summary>
		/// The FormattedString's font
		/// </summary>
		/// <value></value>
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

		/// <summary>
		/// The FormattedString's text color
		/// </summary>
		/// <value></value>
		public Color Color { get; private set; } = Color.White;

		/// <summary>
		/// The rotation of the FormattedString's text
		/// </summary>
		/// <value></value>
		public float Rotation { get; private set; } = 0.0f;

		/// <summary>
		/// The scale of the FormattedString's text
		/// </summary>
		/// <value></value>
		public float Scale { get; private set; } = 1.0f;

		/// <summary>
		/// The content of the FormattedString's text
		/// </summary>
		/// <value></value>
		public string Text { get; set; }

		/// <summary>
		/// The color palette available for the FormattedString
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, string> Pallete {get; private set;} = new Dictionary<string, string>();

		/// <summary>
		/// The FormattedString's properties used in rendering its output text
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, string> Properties { get; private set; } = new Dictionary<string, string>();

		/// <summary>
		/// Parses an input string into a list of one or more FormattedStrings
		/// </summary>
		/// <param name="input"></param>
		/// <param name="defaultFont"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Set a property of the FormattedString
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void SetProperty(string key, string value)
		{
			Properties[key] = value;
			ProcessProperties();
		}

		/// <summary>
		/// Clear a property of the FormattedString
		/// </summary>
		/// <param name="key"></param>
		public void ClearProperty(string key)
		{
			if (Properties.ContainsKey(key))
			{
				Properties.Remove(key);
				ProcessProperties();
			}
		}

		/// <summary>
		/// Retrieve the value of a property of the FormattedString
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetProperty(string key)
		{
			string res = "";
			if (Properties.ContainsKey(key))
			{
				res = Properties[key];
			}
			return res;
		}

		/// <summary>
		/// Set the FormattedString's text color
		/// </summary>
		/// <param name="c"></param>
		public void SetColor(Color c)
		{
			Color = c;
		}

		/// <summary>
		/// Set the FormattedString's color palette
		/// </summary>
		/// <param name="palette"></param>
		public void SetPalette(Dictionary<string, string> palette)
		{
			Pallete = palette;
		}

		/// <summary>
		/// Set the FormattedString's font
		/// </summary>
		/// <param name="font"></param>
		public void SetFont(SpriteFont font)
		{
			Font = font;
		}

		/// <summary>
		/// Set the FormattedString's scale
		/// </summary>
		/// <param name="scale"></param>
		public void SetScale(float scale)
		{
			Scale = scale;
		}

		/// <summary>
		/// Copy the properties of another FormattedString into this FormattedString's properties
		/// </summary>
		/// <param name="source"></param>
		public void CopyProperties(FormattedString source)
		{
			Properties = new Dictionary<string, string>(source.Properties);
		}

		/// <summary>
		/// Handle properties defined for this FormattedString
		/// </summary>
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