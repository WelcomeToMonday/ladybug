using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Ladybug;

namespace Ladybug.Test
{
	public class GameTests
	{
		[Fact]
		public void CreateGame()
		{
			using (var game = new Game())
			{
				Assert.True(game != null, "Game reference null; Game instantiation failed");
			}
		}

		[Fact]
		
		public void CreateGameAndAddScene()
		{
			using (var game = new Game())
			{
				var scene = new Scene(game);
			}
		}
	}
}
