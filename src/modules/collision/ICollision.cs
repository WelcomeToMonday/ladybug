using Microsoft.Xna.Framework;

namespace Ladybug.Collision
{
	/// <summary>
	/// Describes an object that can be checked for collision
	/// </summary>
	public interface ICollision
	{
		/// <summary>
		/// The collision bounds of the object
		/// </summary>
		/// <value></value>
		Rectangle CollisionBounds { get; }
	}
}