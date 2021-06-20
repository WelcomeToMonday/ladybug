using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug;
using Ladybug.ECS;

namespace Ladybug.UI
{
	[Flags]
	public enum Input
	{
		Mouse = 0,
		Keyboard = 1,
		Controller = 2,
	}
	public class UIConfig
	{
		public UIConfig(Game game, SpriteFont defaultFont)
		{
			DefaultFont = defaultFont;
			Game = game;
		}

		public Input Inputs { get; set; } = Input.Mouse | Input.Keyboard;

		public Game Game { get; set; }

		public SpriteFont DefaultFont { get; set; }

		public Texture2D DefaultBackground { get; set; }

		public Rectangle Bounds { get; set; } = new Rectangle(0, 0, 0, 0);

		public ResourceCatalog Catalog { get; set; }
	}

}