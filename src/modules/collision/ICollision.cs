using Microsoft.Xna.Framework;

namespace Ladybug.Collision
{
	public interface ICollision
	{
		Rectangle CollisionBounds { get; }
	}
}