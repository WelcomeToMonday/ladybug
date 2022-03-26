using System;
using Xunit;

using Microsoft.Xna.Framework.Graphics;

using Ladybug.UI;
using GUI = Ladybug.UI;

namespace Ladybug.Tests.UI
{
	[Collection("Unique: Game Instantiation")]
	public class UIConstructionTests
	{
		[Fact]
		public void TestAccessControlByName()
		{
			using (var game = new Game())
			{
				var scene = Scene.Compose();
				game.LoadScene(scene);

				var ui = new GUI.UI(scene);

				ui.AddControl<Panel>("root", out Panel rootPanel);

				rootPanel.AddControl<Panel>("child");

				var childPanel = ui["child"] as Panel;
				var childPanel2 = rootPanel["child"];

				Assert.NotNull(childPanel);
				Assert.NotNull(childPanel2);
				Assert.Equal(childPanel, childPanel2);
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