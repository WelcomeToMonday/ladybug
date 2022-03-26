using Xunit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Tests.Graphics
{
	[Collection("Unique: Game Instantiation")]
	public class SpriteAtlasTests
	{
		[Fact]
		public void TestCreateAndLoadAtlas()
		{
			Assert.False(TestConfig.FAIL_NYI, "Test not yet implemented");
		}

		[Theory]
		[InlineData("image/ladybug-icon", 1, 1)]
		[InlineData("image/bot", 3, 4)]
		public void TestLoadedSpriteDimensions(string file, int cols, int rows)
		{
			Assert.False(TestConfig.FAIL_NYI, "Test not yet implemented");
		}
	}
}