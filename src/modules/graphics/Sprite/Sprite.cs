using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Graphics
{
	/// <summary>
	/// Ladybug Sprite
	/// </summary>
	public class Sprite
	{
		/// <summary>
		/// Creates a new Sprite instance
		/// </summary>
		/// <param name="texture">Source Texture</param>
		public Sprite(Texture2D texture)
		{
			Texture = texture;
			Frame = new Rectangle(0, 0, texture.Width, texture.Height);
			Transform.SetBounds(Frame);
		}

		/// <summary>
		/// Creates a new Sprite instance
		/// </summary>
		/// <param name="texture">Source Texture</param>
		/// <param name="frame">Frame</param>
		public Sprite(Texture2D texture, Rectangle frame)
		{
			Texture = texture;
			Frame = frame;
			Transform.SetBounds(new Rectangle(0, 0, texture.Width, texture.Height));
		}

		/// <summary>
		/// The source Texture of the sprite
		/// </summary>
		public Texture2D Texture { get; private set; }

		/// <summary>
		/// The portion of the Texture that contains the Sprite
		/// </summary>
		public Rectangle Frame { get; set; }

		/// <summary>
		/// Transform containing this Sprite's size, scale, rotation, and
		/// position information
		/// </summary>
		/// <returns></returns>
		public Transform Transform { get; set; } = new Transform();

		/// <summary>
		/// Color value of the Sprite
		/// </summary>
		/// <value></value>
		public Color Color { get; set; } = Color.White;

		/// <summary>
		/// Draws the Sprite
		/// </summary>
		/// <param name="spriteBatch"></param>
		public void Draw(SpriteBatch spriteBatch)
		{
			if (Transform.Scale != Transform.DefaultScale)
			{
				spriteBatch.Draw(
				Texture,
				Transform.Location,
				Frame,
				Color,
				Transform.Rotation,
				Vector2.Zero,
				Transform.Scale,
				SpriteEffects.None,
				0
				);
			}
			else
			{
				spriteBatch.Draw(
					Texture,
					Transform.Bounds,
					Frame,
					Color,
					Transform.Rotation,
					Vector2.Zero,
					SpriteEffects.None,
					0
					);
			}
		}

		/// <summary>
		/// Creates a texture from a 9-slice image map
		/// </summary>
		/// <param name="sourceMap">Source image map</param>
		/// <param name="spriteDimensions">Target dimensions of resulting texture</param>
		/// <param name="graphicsDevice"></param>
		/// <param name="cellSideLength"></param>
		/// <returns></returns>
		public static Texture2D GetTextureFromMap(Texture2D sourceMap, Vector2 spriteDimensions, GraphicsDevice graphicsDevice, int? cellSideLength = null)
		{
			int sideLength = (cellSideLength == null) ? (int)(sourceMap.Bounds.Width / 3) : (int)cellSideLength;

			// Create component sprites
			Texture2D[,] mapCells = new Texture2D[3, 3];
			int cellW = sourceMap.Width / 3;
			int cellH = sourceMap.Height / 3;

			for (var x = 0; x < 3; x++)
			{
				for (var y = 0; y < 3; y++)
				{
					Rectangle sourceRectangle = new Rectangle(x * cellW, y * cellH, cellW, cellH);
					Texture2D newCell = new Texture2D(graphicsDevice, sourceRectangle.Width, sourceRectangle.Height);
					Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
					sourceMap.GetData(0, sourceRectangle, data, 0, data.Length);
					newCell.SetData(data);
					mapCells[y, x] = newCell;
				}
			}

			// Create RenderTarget
			RenderTarget2D target = new RenderTarget2D
			(
				graphicsDevice,
				(int)spriteDimensions.X,
				(int)spriteDimensions.Y,
				false,
				graphicsDevice.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24
			);

			// Draw to RenderTarget

			int targetW = (int)Math.Ceiling((double)spriteDimensions.X / (double)sideLength);
			int targetH = (int)Math.Ceiling((double)spriteDimensions.Y / (double)sideLength);

			graphicsDevice.SetRenderTarget(target);
			graphicsDevice.Clear(Color.Transparent);

			SpriteBatch sb = new SpriteBatch(graphicsDevice);
			sb.Begin();

			Texture2D tex = mapCells[1, 1];

			for (int y = 0; y < targetW; y++)
			{
				for (int x = 0; x < targetH; x++)
				{
					tex = mapCells[1, 1];

					if (y == 0)
					{
						if (x == 0)
						{
							tex = mapCells[0, 0];
						}
						else if (x == targetH - 1)
						{
							tex = mapCells[2, 0];
						}
						else
						{
							tex = mapCells[1, 0];
						}
					}

					if (y == targetW - 1)
					{
						if (x == 0)
						{
							tex = mapCells[0, 2];
						}
						else if (x == targetH - 1)
						{
							tex = mapCells[2, 2];
						}
						else
						{
							tex = mapCells[1, 2];
						}
					}

					if (x == 0)
					{
						if (y != 0 && y != targetW - 1)
						{
							tex = mapCells[0, 1];
						}
					}

					if (x == targetH - 1)
					{
						if (y != 0 && y != targetW - 1)
						{
							tex = mapCells[2, 1];
						}
					}
					sb.Draw(
						tex,
						new Rectangle
						(
							sideLength * y,
							sideLength * x,
							sideLength,
							sideLength
						),
						new Rectangle(0, 0, tex.Width, tex.Height),
						Color.White
					);
				}
			}

			sb.End();

			// Create Texture2D from RenderTarget2D
			Texture2D fullTexture = new Texture2D(graphicsDevice, (int)spriteDimensions.X, (int)spriteDimensions.Y);

			Color[] texdata = new Color[fullTexture.Width * fullTexture.Height];
			target.GetData(texdata);
			fullTexture.SetData(texdata);

			// Set Texture as Background
			//Background = fullTexture;

			graphicsDevice.SetRenderTarget(null);
			graphicsDevice.Clear(Color.Black);

			return fullTexture;
		}

		/// <summary>
		/// Creates a 1-pixel Texture of a solid color
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		public static Texture2D GetTextureFromColor(GraphicsDevice graphicsDevice, Color color)
		{
			var texture = new Texture2D(graphicsDevice, 1, 1);
			texture.SetData(new Color[] { color });
			return texture;
		}
	}
}