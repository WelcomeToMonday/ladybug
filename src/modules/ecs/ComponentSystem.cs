using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.ECS
{
	/// <summary>
	/// A System responsible for processing <see cref="Ladybug.ECS.Component"/> 
	/// update and draw behavior
	/// </summary>
	/// <typeparam name="T">
	/// Type of <see cref="Ladybug.ECS.Component"/> this ComponentSystem
	/// will process
	/// </typeparam>
	public abstract class ComponentSystem<T> where T : Component
	{
		private static List<T> _components = new List<T>();

		private static bool _drawSortRequired = true;

		/// <summary>
		/// Registers a <see cref="Ladybug.ECS.Component"/> to be
		/// processed by this ComponentSystem
		/// </summary>
		/// <param name="component"></param>
		public static void RegisterComponent(T component)
		{
			_components.Add(component);
			RequestDrawSort();
		}

		/// <summary>
		/// Removes a <see cref="Ladybug.ECS.Component"/> from
		/// this ComponentSystem's process queues
		/// </summary>
		/// <param name="component"></param>
		public static void DeregisterComponent(T component)
		{
			if (_components.Contains(component))
			{
				_components.Remove(component);
			}
		}

		/// <summary>
		/// Run an Update step for all <see cref="Ladybug.ECS.Component"/>
		/// instances registered with this ComponentSystem
		/// </summary>
		/// <param name="step">Update step to run</param>
		/// <param name="gameTime"></param>
		public static void Update(string step, GameTime gameTime)
		{
			if (_components == null || _components.Count < 1)
			{
				return;
			}
			for (var i = 0; i < _components.Count; i++)
			{
				var c = _components[i];
				if (!c.Entity.Initialized)
				{
					throw new InvalidOperationException("Entity has not been Initialized!");
				}
				if (c.CheckRunUpdate() && c._UpdateSteps.ContainsKey(step))
				{
					c._UpdateSteps[step](gameTime);
				}
			}
		}

		/// <summary>
		/// Run a Draw step for all <see cref="Ladybug.ECS.Component"/>
		/// instances registered with this ComponentSystem
		/// </summary>
		/// <param name="step">Draw step to run</param>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public static void Draw(string step, GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (_components == null || _components.Count < 1)
			{
				return;
			}

			if (_drawSortRequired)
			{
				SortDrawQueue();
			}
			for (var i = 0; i < _components.Count; i++)
			{
				var c = _components[i];
				if (!c.Entity.Initialized)
				{
					throw new InvalidOperationException("Entity has not been Initialized!");
				}
				if (c.CheckRunDraw() && c._DrawSteps.ContainsKey(step))
				{
					c._DrawSteps[step](gameTime, spriteBatch);
				}
			}
		}

		/// <summary>
		/// Requests the ComponentSystem to resort registered
		/// <see cref="Ladybug.ECS.Component"/> instances by
		/// their <see cref="Ladybug.ECS.Component.DrawPriority"/>
		/// </summary>
		public static void RequestDrawSort() => _drawSortRequired = true;

		private static void SortDrawQueue()
		{
			// We are using the _components list to
			// determine draw order at this time. 
			// This also technically affects update order.
			_components.Sort(delegate (T x, T y)
				{
					int res = 0;
					if (x.DrawPriority > y.DrawPriority) res = 1;
					if (x.DrawPriority < y.DrawPriority) res = -1;
					if (x.DrawPriority == y.DrawPriority) res = 0;
					return res;
				}
			);
			_drawSortRequired = false;
		}
	}
}