using System.Collections.Generic;
using System.Linq;

namespace Ladybug.Entities
{
	public class Entity
	{
		internal Entity(EntitySystem system)
		{
			EntitySystem = system;
		}

		public bool Initialized {get; private set;} = false;

		public EntitySystem EntitySystem { get; private set; }

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

		public T AddComponent<T>() where T : Component, new()
		{
			var res = new T()
			{
				Entity = this
			};
			Components.Add(res);
			EntitySystem.RegisterComponent(res);
			return res;
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