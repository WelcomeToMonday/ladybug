using System;
using Xunit;

using Microsoft.Xna.Framework.Input;

using Ladybug.UserInput;

namespace Ladybug.Tests.UserInput
{
	public class StaticInputTests
	{
		[Theory]
		[InlineData(4)]
		[InlineData(6)]
		[InlineData(1)]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(1000)]
		public void TestInputInstantiation(int count)
		{
			var caughtUnderOver = true;
			if (count < 0 || count > GamePad.MaximumGamePadCount)
			{
				caughtUnderOver = false;
			}

			try
			{
				Assert.NotNull(Input.Keyboard);
				Assert.NotNull(Input.Mouse);
				
				Input.SetGamepadCount(count);
				var gp = Input.GamePads;

				Assert.NotNull(gp);
				Assert.True(gp.Length == count);

				foreach (var p in gp)
				{
					Assert.True(p != null);
				}
			}
			catch (InvalidOperationException)
			{
				caughtUnderOver = true;
			}
			finally
			{
				Assert.True(caughtUnderOver);
			}
		}
	}
}