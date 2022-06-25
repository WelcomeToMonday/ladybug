using Microsoft.Xna.Framework;

using Xunit;

namespace Ladybug.Tests.Util
{
	public class LineTests
	{
		[Theory]
		[InlineData(0, 0, 0, 5, 0, 3, true)]
		[InlineData(0, 0, 0, 5, 0, 6, false)]
		[InlineData(0, 0, 5, 5, 3, 3, true)]
		[InlineData(0, 0, 5, 5, 3, 6, false)]
		public void TestContains(int px, int py, int qx, int qy, int x, int y, bool res)
		{
			var line = new Line(new Vector2(px, py), new Vector2(qx, qy));
			var rx = line.Contains(new Vector2(x, y));
			Assert.Equal(res, line.Contains(new Vector2(x, y)));
		}

		[Theory]
		[InlineData(0, 0, 0, 5, 0, 3, 3, 3, true)]
		[InlineData(5, 5, 5, 0, 3, 0, 3, 6, false)]
		[InlineData(5, 5, 5, 0, 3, 3, 6, 3, true)]
		[InlineData(5, 5, 5, 0, 4, 0, 6, 8, true)]
		public void TestIntersects(int p1x, int p1y, int q1x, int q1y, int p2x, int p2y, int q2x, int q2y, bool res)
		{
			var l1 = new Line(new Vector2(p1x, p1y), new Vector2(q1x, q1y));
			var l2 = new Line(new Vector2(p2x, p2y), new Vector2(q2x, q2y));

			Assert.Equal(res, l1.Intersects(l2));
			Assert.Equal(res, l2.Intersects(l1));
		}
	}
}