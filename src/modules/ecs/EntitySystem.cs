using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.ECS
{
	public class EntitySystem
	{
		public ResourceCatalog ResourceCatalog { get; set; }

		private ulong _nextEntityID = 0;
		private Dictionary<ulong, Entity> _entityList = new Dictionary<ulong, Entity>();
		private Dictionary<Type, List<Component>> _componentList;
		private List<IDrawableComponent> _drawableComponents = new List<IDrawableComponent>();

		public EntitySystem() { }

		public EntitySystem(GraphicsDevice graphicsDevice)
		{
			GraphicsDevice = graphicsDevice;
		}

		public EntitySystem(GraphicsDevice graphicsDevice, ResourceCatalog resourceCatalog) : this(graphicsDevice)
		{
			ResourceCatalog = resourceCatalog;
		}

		public GraphicsDevice GraphicsDevice { get; set; }

		public Entity FindEntity(string name)
		=> _entityList.Where((item => item.Value.Name == name)).FirstOrDefault().Value;

		public List<T> FindAllComponents<T>() where T : Component
		{
			List<T> res = null;

			_entityList.Values.ToList().ForEach(e => 
			{
				var comp = e.GetComponent<T>();
				if (comp != null)
				{
					res.Add(comp);
				}
			});

			return res;
		}

		public void InitializeComponents()
		{
			if (_componentList != null && _componentList.Count > 0)
			{
				for (var i = 0; i < _componentList.Count; i++)
				{
					var cList = _componentList.ElementAt(i).Value;
					for (var j = 0; j < cList.Count; j++)
					{
						cList[j].Initialize();
					}
				}
			}
		}

		public void PreUpdate(GameTime gameTime)
		{
			if (_componentList != null && _componentList.Count > 0)
			{
				for (var i = 0; i < _componentList.Count; i++)
				{
					var cList = _componentList.ElementAt(i).Value;
					for (var j = 0; j < cList.Count; j++)
					{
						if (cList[j].Entity.Active && cList[j].Active)
						{
							cList[j].PreUpdate(gameTime);
						}
					}
				}
			}
		}

		public void Update(GameTime gameTime)
		{
			if (_componentList != null && _componentList.Count > 0)
			{
				for (var i = 0; i < _componentList.Count; i++)
				{
					var cList = _componentList.ElementAt(i).Value;
					for (var j = 0; j < cList.Count; j++)
					{
						if (cList[j].Entity.Active && cList[j].Active)
						{
							cList[j].Update(gameTime);
						}
					}
				}
			}
		}

		public void PostUpdate(GameTime gameTime)
		{
			if (_componentList != null && _componentList.Count > 0)
			{
				for (var i = 0; i < _componentList.Count; i++)
				{
					var cList = _componentList.ElementAt(i).Value;
					for (var j = 0; j < cList.Count; j++)
					{
						if (cList[j].Entity.Active && cList[j].Active)
						{
							cList[j].PostUpdate(gameTime);
						}
					}
				}
			}
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (_drawableComponents != null && _drawableComponents.Count > 0)
			{
				for (var i = 0; i < _drawableComponents.Count; i++)
				{
					if (_drawableComponents[i].Visible)
					{
						_drawableComponents[i].Draw(gameTime, spriteBatch);
					}
				}
			}
		}

		internal ulong RegisterEntity(Entity e)
		{
			var eID = RequestID();
			_entityList.Add(eID, e);
			return eID;
		}

		internal void RegisterComponent(Component c)
		{
			var t = c.GetType();
			if (_componentList == null)
			{
				_componentList = new Dictionary<Type, List<Component>>();
			}
			if (!(_componentList.ContainsKey(t)))
			{
				_componentList[t] = new List<Component>();
			}
			_componentList[t].Add(c);

			IDrawableComponent drawableComponent = c as IDrawableComponent;

			if (drawableComponent != null)
			{
				_drawableComponents.Add(drawableComponent);
				SortDrawableComponents();
			}
		}

		internal void DeregisterComponent(Component c)
		{
			var t = c.GetType();
			_componentList[t].Remove(c);

			IDrawableComponent drawableComponent = c as IDrawableComponent;

			if (drawableComponent != null)
			{
				_drawableComponents.Remove(drawableComponent);
			}
		}

		internal void DeregisterEntity(Entity e)
		{
			_entityList.Remove((ulong)e.ID);
		}

		private ulong RequestID()
		{
			ulong id = _nextEntityID;
			_nextEntityID++;

			return id;
		}

		public void SortDrawableComponents()
		{
			_drawableComponents.Sort(delegate (IDrawableComponent x, IDrawableComponent y)
				{
					int res = 0;
					if (x.DrawPriority > y.DrawPriority) res = 1;
					if (x.DrawPriority < y.DrawPriority) res = -1;
					if (x.DrawPriority == y.DrawPriority) res = 0;
					return res;
				}
			);
		}
	}
}