using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Ladybug.Core
{
	/// <summary>
	/// A CollisionGroup contains methods to query a list of
	/// ICollision objects to get CollisionResults
	/// </summary>
	public class CollisionGroup
	{
		private List<ICollision> _otherColliders;
		private ICollision _targetCollider;

		internal CollisionGroup(ICollision targetCollider, List<ICollision> otherColliders)
		{
			_targetCollider = targetCollider;
			_otherColliders = otherColliders;
		}

		public CollisionResult<ICollision> CheckCollisionByBounds(int offset = 0)
		{
			return CheckCollisionByBounds<ICollision>(offset);
		}
		// todo: Add Vector4 offset support
		public CollisionResult<T> CheckCollisionByBounds<T>(int offset = 0) where T : ICollision
		{
			if (_otherColliders.Contains(_targetCollider)) _otherColliders.Remove(_targetCollider);

			var results = new CollisionResult<T>();
			results.Top = new List<T>();
			results.Bottom = new List<T>();
			results.Left = new List<T>();
			results.Right = new List<T>();

			var list = _otherColliders.Where(c => c.GetType() == typeof(T)).Cast<T>().ToList();

			foreach (var c in list)
			{
				float w = 0.5f * ((_targetCollider.CollisionBounds.Width + offset) + c.CollisionBounds.Width);
				float h = 0.5f * ((_targetCollider.CollisionBounds.Height + offset) + c.CollisionBounds.Height);

				float dx = _targetCollider.CollisionBounds.Center.X - c.CollisionBounds.Center.X;
				float dy = _targetCollider.CollisionBounds.Center.Y - c.CollisionBounds.Center.Y;

				if (Math.Abs(dx) <= w && Math.Abs(dy) <= h)
				{
					float wy = w * dy;
					float hx = h * dx;

					if (wy > hx)
					{
						if (wy > -hx)
						{
							results.Top.Add(c);
						}
						else
						{
							results.Right.Add(c);
						}
					}
					else
					{
						if (wy > -hx)
						{
							results.Left.Add(c);
						}
						else
						{
							results.Bottom.Add(c);
						}
					}
				}
			}
			return results;
		}

		public CollisionResult<ICollision> CheckCollisionByPoints(int offset = 0) => CheckCollisionByPoints<ICollision>(offset);
		public CollisionResult<ICollision> CheckCollisionByPoints(Vector2 offset) => CheckCollisionByPoints<ICollision>(offset);
		public CollisionResult<ICollision> CheckCollisionByPoints(Vector4 offset) => CheckCollisionByPoints<ICollision>(offset);

		public CollisionResult<T> CheckCollisionByPoints<T>(int offset = 0) where T : ICollision => CheckCollisionByPoints<T>(new Vector4(offset, offset, offset, offset));
		public CollisionResult<T> CheckCollisionByPoints<T>(Vector2 offset) where T : ICollision => CheckCollisionByPoints<T>(new Vector4(offset.X, offset.X, offset.Y, offset.Y));
		
		public CollisionResult<T> CheckCollisionByPoints<T>(Vector4 offset) where T : ICollision
		{
			var z = new Vector4();
			if (typeof(T) == _targetCollider.GetType() && _otherColliders.Contains((T)_targetCollider)) _otherColliders.Remove((T)_targetCollider);

			Vector2 topPoint = new Vector2(_targetCollider.CollisionBounds.Center.X, _targetCollider.CollisionBounds.Top - offset.Z);
			Vector2 rightPoint = new Vector2(_targetCollider.CollisionBounds.Right + offset.Y, _targetCollider.CollisionBounds.Center.Y);
			Vector2 bottomPoint = new Vector2(_targetCollider.CollisionBounds.Center.X, _targetCollider.CollisionBounds.Bottom + offset.W);
			Vector2 leftPoint = new Vector2(_targetCollider.CollisionBounds.Left - offset.X, _targetCollider.CollisionBounds.Center.Y);

			var results = new CollisionResult<T>();
			results.Top = new List<T>();
			results.Bottom = new List<T>();
			results.Left = new List<T>();
			results.Right = new List<T>();

			var list = _otherColliders.Where(c => c.GetType() == typeof(T)).Cast<T>().ToList();

			foreach (var c in list)
			{
				if (c.CollisionBounds.Contains(topPoint)) { results.Top.Add(c); }
				if (c.CollisionBounds.Contains(rightPoint)) { results.Right.Add(c); }
				if (c.CollisionBounds.Contains(bottomPoint)) { results.Bottom.Add(c); }
				if (c.CollisionBounds.Contains(leftPoint)) { results.Left.Add(c); }
			}

			return results;
		}
	}
}