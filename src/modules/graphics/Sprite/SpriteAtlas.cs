using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Graphics
{
	/// <summary>
	/// Ladybug SpriteAtlas
	/// </summary>
	public class SpriteAtlas
	{
		private Texture2D _sourceTexture;

		private int _rows;
		private int _cols;

		private Dictionary<Vector2, Sprite> _sprites = new Dictionary<Vector2, Sprite>();

		private Dictionary<string, Vector2> _tags;

		/// <summary>
		/// Creates a new SpriteAtlas
		/// </summary>
		/// <param name="sourceTexture">Source texture</param>
		/// <param name="cols">Number of sprite columns in source texture</param>
		/// <param name="rows">Number of sprite rows in source texture</param>
		public SpriteAtlas(Texture2D sourceTexture, int cols, int rows)
		{
			_sourceTexture = sourceTexture;
			_rows = rows;
			_cols = cols;

			SpriteWidth = (int)(_sourceTexture.Width / _cols);
			SpriteHeight = (int)(_sourceTexture.Height / _rows);
		}

		/// <summary>
		/// Width of a single sprite in this SpriteAtlas
		/// </summary>
		/// <value></value>
		public int SpriteWidth { get; private set; }

		/// <summary>
		/// Height of a single sprite in the SpriteAtlas
		/// </summary>
		/// <value></value>
		public int SpriteHeight { get; private set; }
		
		/// <summary>
		/// Source texture used by this SpriteAtlas
		/// </summary>
		/// <value></value>
		public Texture2D Texture { get => _sourceTexture; }

		/// <summary>
		/// Returns a <see cref="Sprite"/> stored in this SpriteAtlas
		/// </summary>
		/// <value></value>
		public Sprite this[int i] => this[Index2Coord(i)];

		/// <summary>
		/// Returns a <see cref="Sprite"/> stored in this SpriteAtlas
		/// </summary>
		/// <value></value>
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

				var loc = new Vector2(col, row);

				if (!_sprites.ContainsKey(loc))
				{
					Rectangle frame = new Rectangle(
						(int)(SpriteWidth * col),
						(int)(SpriteHeight * row),
						(int)(SpriteWidth),
						(int)(SpriteHeight)
					);

					_sprites.Add(loc, new Sprite(_sourceTexture, frame));
				}

				return _sprites[loc];
			}
		}

		/// <summary>
		/// Returns a <see cref="Sprite"/> stored in this SpriteAtlas
		/// </summary>
		/// <value></value>
		public Sprite this[Vector2 coords]
		{
			get => this[(int)coords.X, (int)coords.Y];
		}

		/// <summary>
		/// Returns a <see cref="Sprite"/> stored in this SpriteAtlas
		/// </summary>
		/// <value></value>
		public Sprite this[string tag]
		{
			get => GetSpriteFromTag(tag);
		}

		/// <summary>
		/// Adds a string tag to a <see cref="Sprite"/> stored in this SpriteAtlas
		/// </summary>
		/// <param name="tag">Name of tag</param>
		/// <param name="coordinates">Coordinate of <see cref="Sprite"/> in SpriteAtlas</param>
		public void TagSprite(string tag, Vector2 coordinates)
		{
			if (_tags == null)
			{
				_tags = new Dictionary<string, Vector2>();
			}

			_tags[tag] = coordinates;
		}

		/// <summary>
		/// Adds a string tag to a <see cref="Sprite"/> stored in this SpriteAtlas
		/// </summary>
		/// <param name="tag">Name of tag</param>
		/// <param name="x">X coordinate of <see cref="Sprite"/> in SpriteAtlas</param>
		/// <param name="y">Y coordinate of <see cref="Sprite"/> in SpriteAtlas</param>
		public void TagSprite(string tag, int x, int y) => TagSprite(tag, new Vector2(x, y));

		/// <summary>
		/// Adds a string tag to a <see cref="Sprite"/> stored in this SpriteAtlas
		/// </summary>
		/// <param name="tag">Name of tag</param>
		/// <param name="index">Index of <see cref="Sprite"/> in SpriteAtlas</param>
		public void TagSprite(string tag, int index) => TagSprite(tag, Index2Coord(index));

		/// <summary>
		/// Retrieves the <see cref="Sprite"/> with the given tag
		/// </summary>
		/// <param name="tag">Name of tag</param>
		/// <returns></returns>
		public Sprite GetSpriteFromTag(string tag)
		{
			var res = default(Sprite);

			if (_tags.ContainsKey(tag))
			{
				res = this[_tags[tag]];
			}

			return res;
		}

		private Vector2 Index2Coord(int index)
		{
			var row = 0;
			var col = 0;

			if (index >= 0 && index <= (_rows * _cols) - 1)
			{
				row = (int)((float)index / (float)(_cols));
				col = index % _cols;
			}

			return new Vector2(col, row);
		}
	}
}