using System;
using Xunit;

using Ladybug;

namespace Ladybug.Test
{
	public class SampleTest
	{
		/*
		[Fact]
		public void PassingTest()
		{
			Assert.Equal(4, Add(2, 2));
		}
		
		[Fact]
		public void FailingTest() => Assert.Equal(5, Add(2, 2));
		
		
		[Theory]
		[InlineData(3)]
		[InlineData(5)]
		[InlineData(6)]
		public void FirstTheory(int value) => Assert.True(IsOdd(value));
		*/

		private int Add(int x, int y) => x + y;

		private bool IsOdd(int value) => value % 2 == 1;
	}
}
