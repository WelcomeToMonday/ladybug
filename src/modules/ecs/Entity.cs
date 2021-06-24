using System.Collections.Generic;
using System.Linq;

namespace Ladybug.ECS
{
	/// <summary>
	/// Ladybug ECS Entity
	/// </summary>
	public class Entity
	{
		internal Entity(ECS ecs)
		{
			ECS = ecs;
		}

		/// <summary>
		/// Whether the Entity has been initialized
		/// </summary>
		/// <value></value>
		public bool Initialized { get; private set; } = false;

		/// <summary>
		/// The Entity's <see cref="Ladybug.Transform"/>
		/// </summary>
		/// <returns></returns>
		public Transform Transform {get; set;} = new Transform();

		/// <summary>
		/// The <seee cref="Ladybug.ECS.ECS"/> managing this Entity
		/// </summary>
		/// <value></value>
		public ECS ECS { get; private set; }

		/// <summary>
		/// List of Tags associated with this Entity
		/// </summary>
		public List<string> Tags { get; set; } = new List<string>();

		/// <summary>
		/// Name of the Entity
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Unique ID of the Entity
		/// </summary>
		public ulong ID { get; internal set; }

		/// <summary>
		/// Whether the Entity is active
		/// </summary>
		public bool Active { get; set; } = true;

		/// <summary>
		/// Whether the Entity is visible.
		/// </summary>
		/// <value></value>
		public bool Visible { get; set; } = true;

		/// <summary>
		/// List of <see cref="Ladybug.ECS.Component"/> instances
		/// contained in this Entity
		/// </summary>
		public List<Component> Components = new List<Component>();

		/// <summary>
		/// Initializes the Entity's Components
		/// </summary>
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

		/// <summary>
		/// Adds a new <see cref="Ladybug.ECS.Component"/> to this Entity
		/// </summary>
		/// <typeparam name="T">
		/// Type of <see cref="Ladybug.ECS.Component"/> 
		/// to add to this Entity
		/// </typeparam>
		/// <returns>Entity</returns>
		public Entity AddComponent<T>() where T : Component, new()
		=> AddComponent<T>(out T component);

		/// <summary>
		/// Adds a new <see cref="Ladybug.ECS.Component"/> to this Entity
		/// </summary>
		/// <typeparam name="T">
		/// Type of <see cref="Ladybug.ECS.Component"/> 
		/// to add to this Entity
		/// </typeparam>
		/// <param name="component">
		/// Reference to the new <see cref="Ladybug.ECS.Component"/>
		/// added to the Entity
		/// </param>
		/// <returns>Entity</returns>
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

		/// <summary>
		/// Attempts to retrieve a <see cref="Ladybug.ECS.Component"/>
		/// contained in the Entity
		/// </summary>
		/// <param name="component">Matching <see cref="Ladybug.ECS.Component"/> </param>
		/// <typeparam name="T"><see cref="Ladybug.ECS.Component"/>  type to find</typeparam>
		/// <returns>True if <see cref="Ladybug.ECS.Component"/> found, otherwise False</returns>
		public bool TryGetComponent<T>(out T component) where T : Component
		{
			component = GetComponent<T>();
			return component != default(T);
		}
		
		/// <summary>
		/// Attempts to retrieve a <see cref="Ladybug.ECS.Component"/>
		/// contained in the Entity
		/// </summary>
		/// <param name="name">Name of <see cref="Ladybug.ECS.Component"/> to find</param>
		/// <param name="component">Matching <see cref="Ladybug.ECS.Component"/> </param>
		/// <typeparam name="T"><see cref="Ladybug.ECS.Component"/>  type to find</typeparam>
		/// <returns>True if <see cref="Ladybug.ECS.Component"/> found, otherwise False</returns>
		public bool TryGetComponent<T>(string name, out T component) where T : Component
		{
			component = GetComponent<T>(name);
			return component != default(T);
		}

		/// <summary>
		/// Retrieves a <see cref="Ladybug.ECS.Component"/> contained
		/// in the entity
		/// </summary>
		/// <typeparam name="T">Type of <see cref="Ladybug.ECS.Component"/> to retrieve</typeparam>
		/// <returns>Matching <see cref="Ladybug.ECS.Component"/> if found, otherwise null</returns>
		public T GetComponent<T>() where T : Component
		=> Components.OfType<T>().Where(c => c.GetType() == typeof(T)).FirstOrDefault();

		/// <summary>
		/// Retrieves a <see cref="Ladybug.ECS.Component"/> contained
		/// in the entity
		/// </summary>
		/// <param name="name">Name of <see cref="Ladybug.ECS.Component"/> to retrieve</param>
		/// <typeparam name="T">Type of <see cref="Ladybug.ECS.Component"/> to retrieve</typeparam>
		/// <returns>Matching <see cref="Ladybug.ECS.Component"/> if found, otherwise null</returns>
		public T GetComponent<T>(string name) where T : Component
		=> Components.OfType<T>().Where(c => c.GetType() == typeof(T) && c.Name == name).FirstOrDefault();
	}
}