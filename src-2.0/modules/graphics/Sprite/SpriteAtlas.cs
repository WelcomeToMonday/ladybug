using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Graphics
{
	public class SpriteAtlas
	{
		private Texture2D _sourceTexture;

		private int _rows;
		private int _cols;

		private int _spriteWidth;
		private int _spriteHeight;

		private Dictionary<string, Vector2> _tags;

		public SpriteAtlas(Texture2D sourceTexture, int rows, int cols)
		{
			_sourceTexture = sourceTexture;
			_rows = rows;
			_cols = cols;

			_spriteWidth = (int)(_sourceTexture.Width / _cols);
			_spriteHeight = (int)(_sourceTexture.Height / _rows);
		}

		public Texture2D Texture { get => _sourceTexture; }

		public Sprite this[int i]
		{
			get
			{
				var row = 0;
				var col = 0;

				if (i >= 0 && i <= (_rows * _cols) - 1)
				{
					row = (int)((float)i / (float)(_cols));
					col = i % _cols;
				}

				Rectangle frame = new Rectangle(
					(int)(_spriteWidth * col),
					(int)(_spriteHeight * row),
					(int)(_spriteWidth),
					(int)(_spriteHeight)
				);

				return new Sprite(_sourceTexture, frame);
			}
		}

		public Sprite this[int i, int j]
		{
			get
			{
				var row = 0;
				var col = 0;

				if (
					(i >= 0 && i <= _cols - 1) &&
					(j >= 0 && j <= _rows - 1)
				)
				{
					col = i;
					row = j;
				}

				Rectangle frame = new Rectangle(
					(int)(_spriteWidth * col),
					(int)(_spriteHeight * row),
					(int)(_spriteWidth),
					(int)(_spriteHeight)
				);

				return new Sprite(_sourceTexture, frame);
			}
		}

		public Sprite this[Vector2 coords]
		{
			get => this[(int)coords.X, (int)coords.Y];
		}

		public Sprite this[string tag]
		{
			get => GetSpriteFromTag(tag);
		}

		public void TagSprite(string tag, Vector2 coordinates)
		{
			if (_tags == null)
			{
				_tags = new Dictionary<string, Vector2>();
			}

			_tags[tag] = coordinates;
		}

		public void TagSprite(string tag, int x, int y) => TagSprite(tag, new Vector2(x, y));

		public Sprite GetSpriteFromTag(string tag)
		{
			var res = default(Sprite);

			if (_tags.ContainsKey(tag))
			{
				res = this[_tags[tag]];
			}

			return res;
		}
	}
}