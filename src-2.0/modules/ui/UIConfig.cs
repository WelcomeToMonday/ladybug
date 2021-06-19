using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Scene;
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
		public UIConfig(SceneManager sceneManager, SpriteFont defaultFont)
		{
			DefaultFont = defaultFont;
			SceneManager = sceneManager;
		}

		public Input Inputs { get; set; } = Input.Mouse | Input.Keyboard;

		public SceneManager SceneManager { get; set; }

		public SpriteFont DefaultFont { get; set; }

		public Texture2D DefaultBackground { get; set; }

		public Rectangle Bounds { get; set; } = new Rectangle(0, 0, 0, 0);

		public ResourceCatalog Catalog { get; set; }
	}

}