using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;
using Ladybug.Graphics.BoxModel;

namespace Ladybug.Core.UI
{
	public class Label : Control
	{
		private TextSprite _textSprite;

		public Label(Control parentControl = null, string name = "") : base(parentControl, name)
		{
			PositionChanged += OnPositionChanged;
		}

		public override void Initialize()
		{
			/*
			_textSprite.SetBoundsToText();
			SetBounds(_textSprite.Bounds);
			*/
			SetText(Text);
		}

		public string Text { get; protected set; } = "Label";

		public void SetText(string newText)
		{
			if (Font != null)
			{
				Text = newText;
				var dimensions = Text == "" ? new Vector2(0, 0) : new Vector2(1000, 100);
				if (_textSprite == null)
				{
					_textSprite = new TextSprite(newText, Font, new Rectangle(0, 0, 1000, 1000));
				}
				_textSprite.SetBounds(new Rectangle(
					(int)Bounds.Location.X,
					(int)Bounds.Location.Y,
					(int)dimensions.X,
					(int)dimensions.Y));
				_textSprite.SetText(newText);
				_textSprite.SetBoundsToText();
				SetBounds(_textSprite.Bounds);
			}
		}

		public void SetColor(Color color) => _textSprite?.SetColor(color);

		public void SetScale(float scale) => _textSprite?.SetScale(scale);

		public override void SetFont(SpriteFont font)
		{
			base.SetFont(font);
			_textSprite?.SetFont(font);
		}

		private void OnPositionChanged(object sender, EventArgs e)
		{
			_textSprite?.SetBoundsToText();
			_textSprite?.SetBounds(
				_textSprite.Bounds.CopyAtPosition(Bounds.GetHandlePosition(BoxHandle.CENTER), BoxHandle.CENTER)
				);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (BackgroundImage != null)
			{
				spriteBatch.Draw
				(
					BackgroundImage,
					Bounds,
					null,
					Color.White
				);
			}
			_textSprite?.Draw(spriteBatch);
		}
	}
}