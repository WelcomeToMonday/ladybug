// https://gamedevelopment.tutsplus.com/tutorials/quick-tip-use-quadtrees-to-detect-likely-collisions-in-2d-space--gamedev-374
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Ladybug.Collision
{
	public class QuadTree
	{
		private int _maxObjects = 10;
		private int _maxLevels = 5;

		private int _level;
		private List<ICollision> _objects;
		private Rectangle _bounds;
		private QuadTree[] _nodes;

		public QuadTree(int level, Rectangle bounds)
		{
			_level = level;
			_objects = new List<ICollision>();
			_bounds = bounds;
			_nodes = new QuadTree[4];
		}

		public Rectangle Bounds { get => _bounds; } // Temp, used for testing
		public QuadTree[] Nodes { get => _nodes; }  // Temp, used for testing

		public void Clear()
		{
			_objects.Clear();

			for (int i = 0; i < _nodes.Length; i++)
			{
				if (_nodes[i] != null)
				{
					_nodes[i].Clear();
					_nodes[i] = null;
				}
			}
		}

		public void Insert(ICollision collider)
		{
			if (_nodes[0] != null)
			{
				var index = GetIndices(collider);

				if (index.Count > 0)
				{
					foreach (var i in index)
					{
						_nodes[i].Insert(collider);
					}
				}
			}
			else
			{
				_objects.Add(collider);

				if (_objects.Count > _maxObjects && _level < _maxLevels)
				{
					if (_nodes[0] == null)
					{
						Split();
					}

					int i = 0;
					while (i < _objects.Count)
					{
						var index = GetIndices(_objects[i]);
						if (index.Count > 0)
						{
							foreach (var idx in index)
							{
								_nodes[idx].Insert(_objects[i]);
							}
							_objects.RemoveAt(i);
						}
						else
						{
							i++;
						}
					}
				}
			}
		}
		
		public CollisionGroup Retrieve(ref List<ICollision> returnObjects, ICollision collider)
		{
			var index = GetIndices(collider);

			if (index.Count > 0 && _nodes[0] != null)
			{
				foreach (var i in index)
				{
					_nodes[i].Retrieve(ref returnObjects, collider);
				}
			}

			returnObjects.AddRange(_objects);

			return new CollisionGroup(collider, returnObjects);
		}

		public CollisionGroup RetrieveByType<T>(ref List<T> returnObjects, ICollision collider) where T : ICollision
		{
			var index = GetIndices(collider);

			if (index.Count > 0 && _nodes[0] != null)
			{
				foreach (var i in index)
				{
					_nodes[i].RetrieveByType<T>(ref returnObjects, collider);
				}
			}

			foreach (var o in _objects)
			{
				if (o.GetType() == typeof(T))
				{
					returnObjects.Add((T)o);
				}
			}

			List<ICollision> resList = new List<ICollision>();

			foreach (var o in returnObjects)
			{
				resList.Add(o);
			}

			return new CollisionGroup(collider, resList);
		}

		private void Split()
		{
			int subWidth = (int)(_bounds.Width / 2);
			int subHeight = (int)(_bounds.Height / 2);

			int x = (int)(_bounds.X);
			int y = (int)(_bounds.Y);

			_nodes[0] = new QuadTree(_level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
			_nodes[1] = new QuadTree(_level + 1, new Rectangle(x, y, subWidth, subHeight));
			_nodes[2] = new QuadTree(_level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
			_nodes[3] = new QuadTree(_level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
		}

		private List<int> GetIndices(ICollision collider)
		{
			var indicies = new List<int>();

			Vector2 midPoint = _bounds.GetHandlePosition(BoxHandle.Center);

			Rectangle tL = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width / 2, Bounds.Height / 2);
			Rectangle tR = tL.CopyAtOffset(tL.Width,0);
			Rectangle bL = tL.CopyAtOffset(0,tL.Height);
			Rectangle bR = tR.CopyAtOffset(0,tR.Height);

		  var col = collider.CollisionBounds;

			if (col.Intersects(tR)) indicies.Add(0);
			if (col.Intersects(tL)) indicies.Add(1);
			if (col.Intersects(bL)) indicies.Add(2);
			if (col.Intersects(bR)) indicies.Add(3);

			return indicies;
		}
	}
}