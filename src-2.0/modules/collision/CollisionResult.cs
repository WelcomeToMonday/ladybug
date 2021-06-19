using System.Collections.Generic;
using System.Linq;

namespace Ladybug.Collision
{
	public struct CollisionResult<T> where T : ICollision
	{
		public List<T> Top;
		public List<T> Bottom;
		public List<T> Left;
		public List<T> Right;
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
		public bool TopExists { get => Top != null && Top.Count > 0; }
		public bool BottomExists { get => Bottom != null && Bottom.Count > 0; }
		public bool LeftExists { get => Left != null && Left.Count > 0; }
		public bool RightExists { get => Right != null && Right.Count > 0;}
		public bool AnyExists { get => TopExists || BottomExists || LeftExists || RightExists; }
		public bool AllExists { get => TopExists && BottomExists && LeftExists && RightExists; }

		public CollisionResult<ICollision> ToGeneric()
		{
			var result = new CollisionResult<ICollision>();
			result.Top = this.Top.Cast<ICollision>().ToList();
			result.Bottom = this.Bottom.Cast<ICollision>().ToList();
			result.Left = this.Left.Cast<ICollision>().ToList();
			result.Right = this.Right.Cast<ICollision>().ToList();

			return result;
		}

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