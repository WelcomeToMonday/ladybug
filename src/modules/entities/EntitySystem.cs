using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Entities
{
	public class EntitySystem
	{
		private ulong _nextEntityID = 0;
		private bool _drawSortRequired = true;
		private List<Component> _drawQueue = new List<Component>();

		public EntitySystem(Scene scene)
		{
			Scene = scene;

			UpdateSteps = new List<string>
			{
				"Update"
			};

			DrawSteps = new List<string>
			{
				"Draw"
			};
		}

		public ResourceCatalog ResourceCatalog { get; set; }
		public Scene Scene { get; set; }

		public List<Entity> Entities { get; private set; } = new List<Entity>();
		public Dictionary<Type, List<Component>> Components { get; private set; } = new Dictionary<Type, List<Component>>();

		public List<string> UpdateSteps { get; private set; }
		public List<string> DrawSteps { get; private set; }

		public Entity CreateEntity()
		{
			var newID = GetEntityID();
			var res = new Entity(this)
			{
				ID = newID,
				Name = $"Entity_{newID}"
			};
			Entities.Add(res);
			return res;
		}

		public void RemoveEntity(Entity e)
		{
			if (!Entities.Contains(e))
			{
				return;
			}

			foreach (var c in e.Components)
			{
				DeregisterComponent(c);
			}
			Entities.Remove(e);
		}

		public Entity FindEntity(string name)
		=> Entities.Where((e => e.Name == name)).FirstOrDefault();

		public void InitializeEntities()
		{
			foreach (var entity in Entities)
			{
				entity.Initialize();
			}
		}

		public List<T> FindAllComponents<T>() where T : Component
		{
			List<T> res = null;

			Entities.ToList().ForEach(e =>
			{
				if (e.TryGetComponent<T>(out T component))
				{
					res.Add(component);
				}
			});

			return res;
		}

		public void Update(GameTime gameTime)
		{
			if (Components == null || Components.Count < 1)
			{
				return;
			}
			foreach (var step in UpdateSteps)
			{
				for (var i = 0; i < Components.Count; i++)
				{
					var cList = Components.ElementAt(i).Value;
					for (var j = 0; j < cList.Count; j++)
					{
						if (!cList[j].Entity.Initialized)
						{
							throw new InvalidOperationException("Entity has not been Initialized!");
						}
						if (cList[j].Entity.Active && cList[j].Active && cList[j].UpdateSteps.ContainsKey(step))
						{
							cList[j].UpdateSteps[step](gameTime);
						}
					}
				}
			}
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (Components == null || Components.Count < 1)
			{
				return;
			}

			if (_drawSortRequired)
			{
				SortDrawQueue();
			}

			foreach (var step in DrawSteps)
			{
				for (var i = 0; i < Components.Count; i++)
				{
					var cList = Components.ElementAt(i).Value;
					for (var j = 0; j < cList.Count; j++)
					{
						if (!cList[j].Entity.Initialized)
						{
							throw new InvalidOperationException("Entity has not been Initialized!");
						}
						if (cList[j].Entity.Visible && cList[j].Visible && cList[j].DrawSteps.ContainsKey(step))
						{
							cList[j].DrawSteps[step](gameTime, spriteBatch);
						}
					}
				}
			}
		}

		internal void RegisterComponent(Component c)
		{
			var t = c.GetType();

			if (!(Components.ContainsKey(t)))
			{
				Components[t] = new List<Component>();
			}
			_drawQueue.Add(c);
			Components[t].Add(c);
		}

		internal void DeregisterComponent(Component c)
		{
			var t = c.GetType();
			Components[t].Remove(c);
			_drawQueue.Remove(c);
		}

		private ulong GetEntityID()
		{
			var res = _nextEntityID;
			_nextEntityID++;
			return res;
		}

		internal void SortDrawQueue()
		{
			_drawQueue.Sort(delegate (Component x, Component y)
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