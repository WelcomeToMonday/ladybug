using System;
using Xunit;

using Microsoft.Xna.Framework.Graphics;

using Ladybug.UI;
using GUI = Ladybug.UI;

namespace Ladybug.Tests.UI
{
	public class UIConstructionTests
	{
		[Fact]
		public void TestAccessControlByName()
		{
			using (var game = new Game())
			{
				var scene = Scene.Compose();
				game.LoadScene(scene);

				var ui = new GUI.UI(scene)
					.AddControl<Panel>("root", out Panel rootPanel);

				rootPanel.AddControl<Panel>("child");

				var childPanel = ui["root"]["child"] as Panel;

				Assert.NotNull(childPanel);
			}
		}

		[Fact]
		public void TestAccessControlByFind()
		{
			Assert.False(TestConfig.FAIL_NYI, "Test not yet implemented");
		}

		[Fact]
		public void TestCreateAndAttachButton()
		{
			using (var game = new Game())
			{
				var scene = Scene.Compose();
				scene.OnLoadContent(() =>
				{
					scene.ResourceCatalog.LoadResource<SpriteFont>(UIResources.DefaultFont, "font/default");
				});
				game.LoadScene(scene);

				var ui = new GUI.UI(scene)
					.AddControl<Button>(out Button button);

				Assert.NotNull(button);
			}
		}
	}
}