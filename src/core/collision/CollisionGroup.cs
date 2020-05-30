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
				float w = 0.5f * (_targetCollider.CollisionBounds.Width + c.CollisionBounds.Width);
				float h = 0.5f * (_targetCollider.CollisionBounds.Height + c.CollisionBounds.Height);

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

		public CollisionResult<ICollision> CheckCollisionByPoints(int offset = 0)
		{
			return CheckCollisionByPoints<ICollision>();
		}

		public CollisionResult<T> CheckCollisionByPoints<T>(int offset = 0) where T : ICollision
		{
			if (typeof(T) == _targetCollider.GetType() && _otherColliders.Contains((T)_targetCollider)) _otherColliders.Remove((T)_targetCollider);

			Vector2 topPoint = new Vector2(_targetCollider.CollisionBounds.Center.X, _targetCollider.CollisionBounds.Top - offset);
			Vector2 rightPoint = new Vector2(_targetCollider.CollisionBounds.Right + offset, _targetCollider.CollisionBounds.Center.Y);
			Vector2 bottomPoint = new Vector2(_targetCollider.CollisionBounds.Center.X, _targetCollider.CollisionBounds.Bottom + offset);
			Vector2 leftPoint = new Vector2(_targetCollider.CollisionBounds.Left - offset, _targetCollider.CollisionBounds.Center.Y);

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