using System.Collections.Generic;
using System.Linq;

namespace Ladybug.Collision
{
	/// <summary>
	/// Represents the result of a collision check
	/// </summary>
	public struct CollisionResult<T> where T : ICollision
	{
		/// <summary>
		/// Objects that have collided with the top of the target collider
		/// </summary>
		public List<T> Top;

		/// <summary>
		/// Objects that have collided with the bottom of the target collider
		/// </summary>
		public List<T> Bottom;

		/// <summary>
		/// Objects that have collided with the left side of the target collider
		/// </summary>
		public List<T> Left;

		/// <summary>
		/// Objects that have collided with the right side of the target collider
		/// </summary>
		public List<T> Right;

		/// <summary>
		/// All objects that have collided with the target collider
		/// </summary>
		/// <value></value>
		public List<T> All
		{
			get
			{
				var l = new List<T>();
				if (Top != null) l.AddRange(Top);
				if (Bottom != null) l.AddRange(Bottom);
				if (Left != null) l.AddRange(Left);
				if (Right != null) l.AddRange(Right);
				return l;
			}
		}

		/// <summary>
		/// Whether any collisions have occurred at the top edge of the collider
		/// </summary>
		/// <value></value>
		public bool TopExists { get => Top != null && Top.Count > 0; }

		/// <summary>
		/// Whether any collisions have occurred at the bottom edge of the collider
		/// </summary>
		/// <value></value>
		public bool BottomExists { get => Bottom != null && Bottom.Count > 0; }

		/// <summary>
		/// Whether any collisions have occurred at the left edge of the collider
		/// </summary>
		/// <value></value>
		public bool LeftExists { get => Left != null && Left.Count > 0; }

		/// <summary>
		/// Whether any collisions have occurred at the right edge of the collider
		/// </summary>
		/// <value></value>
		public bool RightExists { get => Right != null && Right.Count > 0;}

		/// <summary>
		/// Whether any collisions have occurred at any of the collider's edges
		/// </summary>
		/// <value></value>
		public bool AnyExists { get => TopExists || BottomExists || LeftExists || RightExists; }

		/// <summary>
		/// Whether collisions have occurred at all of the collider's edges
		/// </summary>
		/// <value></value>
		public bool AllExists { get => TopExists && BottomExists && LeftExists && RightExists; }

		/// <summary>
		/// Converts this CollisionResult container to a CollisionResult container containing basic ICollision references 
		/// </summary>
		/// <returns></returns>
		public CollisionResult<ICollision> ToGeneric()
		{
			var result = new CollisionResult<ICollision>();
			result.Top = this.Top.Cast<ICollision>().ToList();
			result.Bottom = this.Bottom.Cast<ICollision>().ToList();
			result.Left = this.Left.Cast<ICollision>().ToList();
			result.Right = this.Right.Cast<ICollision>().ToList();

			return result;
		}

		/// <summary>
		/// Combines this CollisionResult with another
		/// </summary>
		/// <param name="results"></param>
		/// <returns></returns>
		public static CollisionResult<ICollision> Combine(params CollisionResult<ICollision>[] results)
		{
			var result = new CollisionResult<ICollision>();
			
			result.Top = new List<ICollision>();
			result.Bottom = new List<ICollision>();
			result.Left = new List<ICollision>();
			result.Right = new List<ICollision>();
			
			foreach (var r in results)
			{
				if (r.Top != null) result.Top.AddRange(r.Top);
				if (r.Bottom != null) result.Bottom.AddRange(r.Bottom);
				if (r.Left != null) result.Left.AddRange(r.Left);
				if (r.Right != null) result.Right.AddRange(r.Right);
			}

			return result;
		}
	}
}