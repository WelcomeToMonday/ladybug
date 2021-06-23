using System.Collections.Generic;
using System.Linq;

namespace Ladybug.ECS
{
	public class Entity
	{
		internal Entity(ECS ecs)
		{
			ECS = ecs;
		}

		public bool Initialized { get; private set; } = false;

		public Transform Transform {get; set;} = new Transform();

		public ECS ECS { get; private set; }

		public List<string> Tags { get; set; } = new List<string>();
		public string Name { get; set; }
		public ulong ID { get; internal set; }
		public bool Active { get; set; } = true;
		public bool Visible { get; set; } = true;

		public List<Component> Components = new List<Component>();

		public void Initialize()
		{
			if (Initialized)
			{
				return;
			}

			foreach (var c in Components)
			{
				c._Initialize();
			}

			Initialized = true;
		}

		public Entity AddComponent<T>() where T : Component, new()
		=> AddComponent<T>(out T component);

		public Entity AddComponent<T>(out T component) where T : Component, new()
		{
			component = new T()
			{
				Entity = this
			};
			Components.Add(component);
			ECS.RegisterComponent(component);
			return this;
		}

		public bool TryGetComponent<T>(out T component) where T : Component
		{
			component = GetComponent<T>();
			return component != default(T);
		}

		public bool TryGetComponent<T>(string name, out T component) where T : Component
		{
			component = GetComponent<T>(name);
			return component != default(T);
		}

		public T GetComponent<T>() where T : Component
		=> Components.OfType<T>().Where(c => c.GetType() == typeof(T)).FirstOrDefault();

		public T GetComponent<T>(string name) where T : Component
		=> Components.OfType<T>().Where(c => c.GetType() == typeof(T) && c.Name == name).FirstOrDefault();
	}
}