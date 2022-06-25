using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Xunit;

namespace Ladybug.Tests.Util
{
	public class PolygonTests
	{
		[Fact]
		public void TestRectangle()
		{
			var r = new Polygon(
				new Vector2(5, 1),
				new Vector2(10, 1),
				new Vector2(10, 6),
				new Vector2(5, 6)
				);

			var x = r;
		}

		[Theory]
		[MemberData(nameof(GetTestPolyContainsData))]
		public void TestPolyContains(Polygon polygon, Vector2 vector, bool expected)
		{
			var contains = polygon.Contains(vector);
			Assert.Equal(expected, contains);
		}

		public static IEnumerable<object[]> GetTestPolyContainsData()
		{
			var res = new List<object[]>();
			
			// 5 x 5 square
			var square = new Polygon(
				new Vector2(5, 1),
				new Vector2(10, 1),
				new Vector2(10, 6),
				new Vector2(5, 6)
				);

			var squareTests = new List<object[]>
			{
				// Check Vertices
				// new object[] {square, new Vector2(5, 1), false},
				// new object[] {square, new Vector2(10, 1), false},
				// new object[] {square, new Vector2(10, 6), false},
				// new object[] {square, new Vector2(5, 5), false},

				// Check Edges
				// new object[] {square, new Vector2(6, 1), false},
				// new object[] {square, new Vector2(5, 2), false},
				// new object[] {square, new Vector2(10, 2), false},
				// new object[] {square, new Vector2(6, 6), false},

				// Misc
				new object[] {square, new Vector2(4, 1), false},
				new object[] {square, new Vector2(6, 2), true},
				new object[] {square, new Vector2(6, -1), false},
				new object[] {square, new Vector2(10, 7), false}
			};
			res.AddRange(squareTests);

			// 8 x 8 bounds Triangle
			var triangle = new Polygon(
				new Vector2(4, 0),
				new Vector2(0, 8),
				new Vector2(8, 8)
				);

			var triangleTests = new List<object[]>
			{
				// Check Vertices
				// new object[] {triangle, new Vector2(4, 0), true},
				// new object[] {triangle, new Vector2(0, 8), true},
				// new object[] {triangle, new Vector2(8, 8), true},

				// Check Edges
				// new object[] {triangle, new Vector2(2, 4), true},
				// new object[] {triangle, new Vector2(6, 4), true},
				// new object[] {triangle, new Vector2(4, 8), true},

				// Misc
				new object[] {triangle, new Vector2(4, 1), true},
				new object[] {triangle, new Vector2(5, 0), false},
				new object[] {triangle, new Vector2(5, 1), false},
				new object[] {triangle, new Vector2(5, 3), true},
				new object[] {triangle, new Vector2(5, -1), false},
			};
			res.AddRange(triangleTests);

			// 6 x 8 Point-top Hexagon
			var hexagon = new Polygon(
				new Vector2(4, 0),
				new Vector2(7, 2),
				new Vector2(7, 6),
				new Vector2(4, 8),
				new Vector2(1, 6),
				new Vector2(1, 2)
				);

			var hexagonTests = new List<object[]>
			{
				// Check Vertices
				// new object[] {hexagon, new Vector2(4, 0), true},
				// new object[] {hexagon, new Vector2(7, 2), true},
				// new object[] {hexagon, new Vector2(7, 6), true},
				// new object[] {hexagon, new Vector2(4, 8), true},
				// new object[] {hexagon, new Vector2(1, 6), true},
				// new object[] {hexagon, new Vector2(1, 2), true},

				// Check Edges
				// new object[] {hexagon, new Vector2(1, 4), true},
				// new object[] {hexagon, new Vector2(7, 3), true},

				// Misc
				new object[] {hexagon, new Vector2(2, 2), true},
				new object[] {hexagon, new Vector2(2, 1), false},
				new object[] {hexagon, new Vector2(4, 1), true},
				new object[] {hexagon, new Vector2(3, 0), false},
				new object[] {hexagon, new Vector2(2, 6), true},
				new object[] {hexagon, new Vector2(2, 7), false},
				new object[] {hexagon, new Vector2(3, 7), true},
				new object[] {hexagon, new Vector2(6, 6), true},
			};
			res.AddRange(hexagonTests);

			// 8 x 8 Flat-top hexagon
			var fHexagon = new Polygon(
				new Vector2(2, 0),
				new Vector2(6, 0),
				new Vector2(8, 4),
				new Vector2(6, 8),
				new Vector2(2, 8),
				new Vector2(0, 4)
				);

			var fHexagonTests = new List<object[]>
			{
				// Check Vertices
				// new object[] {fHexagon, new Vector2(2, 0), true},
				// new object[] {fHexagon, new Vector2(6, 0), true},
				// new object[] {fHexagon, new Vector2(8, 4), true},
				// new object[] {fHexagon, new Vector2(6, 8), true},
				// new object[] {fHexagon, new Vector2(2, 8), true},
				// new object[] {fHexagon, new Vector2(0, 4), true},

				// Check Edges
				// new object[] {fHexagon, new Vector2(1, 2), true},
				// new object[] {fHexagon, new Vector2(4, 0), true},
				// new object[] {fHexagon, new Vector2(5, 2), true},
				// new object[] {fHexagon, new Vector2(5, 6), true},
				// new object[] {fHexagon, new Vector2(4, 8), true},
				// new object[] {fHexagon, new Vector2(2, 6), true},

				// Misc
				new object[] {fHexagon, new Vector2(1, 1), false},
				new object[] {fHexagon, new Vector2(1, 3), true},
				new object[] {fHexagon, new Vector2(2, 4), true},
				new object[] {fHexagon, new Vector2(2, 2), true},
				new object[] {fHexagon, new Vector2(0, 5), false},
			};
			res.AddRange(fHexagonTests);

			// 11x10 Irregular convex polygon
			var iConvex = new Polygon(
				new Vector2(2, 1),
				new Vector2(8, 3),
				new Vector2(11, 8),
				new Vector2(5, 11),
				new Vector2(0, 7),
				new Vector2(0, 4)
				);

			var iConvexTests = new List<object[]>
			{
				// Check Vertices
				// new object[] {iConvex, new Vector2(2, 1), true},
				// new object[] {iConvex, new Vector2(8, 3), true},
				// new object[] {iConvex, new Vector2(11, 8), true},
				// new object[] {iConvex, new Vector2(5, 11), true},
				// new object[] {iConvex, new Vector2(0, 7), true},
				// new object[] {iConvex, new Vector2(0, 4), true},

				// Check Edges
				// new object[] {iConvex, new Vector2(5, 2), true},
				// new object[] {iConvex, new Vector2(9, 9), true},
				// new object[] {iConvex, new Vector2(7, 10), true},
				// new object[] {iConvex, new Vector2(0, 5), true},

				// Misc
				new object[] {iConvex, new Vector2(2, 2), true},
				new object[] {iConvex, new Vector2(4, 2), true},
				new object[] {iConvex, new Vector2(4, 1), false},
				new object[] {iConvex, new Vector2(6, 2), false},
				new object[] {iConvex, new Vector2(8, 4), true},
				new object[] {iConvex, new Vector2(4, 3), true},
				new object[] {iConvex, new Vector2(2, 9), false},
			};
			res.AddRange(iConvexTests);

			var iConcave = new Polygon(
				new Vector2(2, 0),
				new Vector2(8, 2),
				new Vector2(4, 4),
				new Vector2(7, 8),
				new Vector2(0, 8),
				new Vector2(0, 3)
				);

			var iConcaveTests = new List<object[]>
			{
				// Misc
				new object[] {iConcave, new Vector2(2, 1), true},
				new object[] {iConcave, new Vector2(4, 0), false},
				new object[] {iConcave, new Vector2(1, 2), true},
				new object[] {iConcave, new Vector2(8, 1), false},
				new object[] {iConcave, new Vector2(7, 2), true},
				new object[] {iConcave, new Vector2(7, 3), false},
				new object[] {iConcave, new Vector2(5, 4), false},
				new object[] {iConcave, new Vector2(5, 5), false},
				new object[] {iConcave, new Vector2(4, 5), true},
			};
			res.AddRange(iConcaveTests);

			return res;
		}
	}
}