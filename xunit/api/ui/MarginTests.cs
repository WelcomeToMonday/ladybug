using Xunit;

using Ladybug.UI;

namespace Ladybug.Tests.UI
{
	public class MarginTests
	{
		[Fact]
		public void TestEqualityOperators()
		{
			var m1 = new Margin(1, 2, 3, 4);
			var m2 = new Margin(2, 2, 3, 4);
			var m3 = new Margin(1, 2, 3, 4);

			Assert.True(m1 == m3);
			Assert.True(m1 != m2);
		}

		[Theory]
		[InlineData(2, 4, 5, 6)]
		[InlineData(1, 1, 1, 1)]
		public void TestConstructors(int arg1, int arg2, int arg3, int arg4)
		{
			var m1 = new Margin(arg1);
			var m2 = new Margin(arg1, arg2);
			var m3 = new Margin(arg1, arg2, arg3);
			var m4 = new Margin(arg1, arg2, arg3, arg4);

			Assert.True(
				m1.Top == arg1 &&
				m1.Right == arg1 &&
				m1.Bottom == arg1 &&
				m1.Left == arg1
			);

			Assert.True(
				m2.Top == arg1 &&
				m2.Right == arg2 &&
				m2.Bottom == arg1 &&
				m2.Left == arg2
			);

			Assert.True(
				m3.Top == arg1 &&
				m3.Right == arg2 &&
				m3.Bottom == arg3 &&
				m3.Left == arg2
			);

			Assert.True(
				m4.Top == arg1 &&
				m4.Right == arg2 &&
				m4.Bottom == arg3 &&
				m4.Left == arg4
			);
		}
	}
}