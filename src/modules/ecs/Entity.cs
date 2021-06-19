using System; //temp

using System.Linq;
using System.Collections.Generic;

namespace Ladybug.ECS
{
	public sealed class Entity
	{
		public static Dictionary<int, string> Tags;

		private List<Component> m_components;

		public Entity(EntitySystem entitySystem, string name = null)
		{
			System = entitySystem;
			ID = System.RegisterEntity(this);
			if (name != null)
			{
				Name = name;
			}
		}

		public EntitySystem System { get; private set; }

		public string Name { get; private set; }

		public ulong? ID { get; private set; } = null;

		public bool Active { get; set; } = true;

		public List<Component> Components
		{
			get
			{
				if (m_components == null)
				{
					m_components = new List<Component>();
				}
				return m_components;
			}
			private set => m_components = value;
		}

		public void SetName(string name)
		{
			Name = name;
		}

		public T AddComponent<T>(string name = null) where T : Component, new()
		{
			var comp = new T();
			comp.SetEntity(this);
			if (name != null)
			{
				comp.SetName(name);
			}
			Components.Add(comp);
			System.RegisterComponent(comp);
			return comp;
		}

		public Component AddComponent(Component c, string name = null)
		{
			c.SetEntity(this);
			Components.Add(c);
			if (name != null)
			{
				c.SetName(name);
			}
			System.RegisterComponent(c);
			return c;
		}

		public void RemoveComponent(Component c)
		{
			System.DeregisterComponent(c);
			Components.Remove(c);
		}

		public T GetComponent<T>() where T : Component
		=> Components.OfType<T>().Where(item => item.GetType() == typeof(T)).FirstOrDefault();

		public T GetComponent<T>(string name) where T : Component
		=> Components.OfType<T>().Where(item => (item.GetType() == typeof(T) && item.Name == name)).FirstOrDefault();

		public void InitializeComponents()
		{
			if (Components != null && Components.Count > 0)
			{
				foreach(var c in Components)
				{
					c.Initialize();
				}
			}
		}

		public void Remove()
		{
			foreach (var component in Components)
			{
				System.DeregisterComponent(component);
			}
			System.DeregisterEntity(this);
		}
	}
}