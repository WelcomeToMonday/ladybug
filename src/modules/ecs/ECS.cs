using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.ECS.Components;

namespace Ladybug.ECS
{
	/// <summary>
	/// Ladybug Entity-Component-System
	/// </summary>
	public class ECS
	{
		private ulong _nextEntityID = 0;

		/// <summary>
		/// Creates a new ECS instance
		/// </summary>
		/// <param name="scene">Scene object containing this ECS</param>
		public ECS(Scene scene)
		{
			RegisterComponentSystem<InputComponent, InputComponentSystem>();
			RegisterComponentSystem<SpriteComponent, SpriteComponentSystem>();
			Scene = scene;
			ResourceCatalog = scene.ResourceCatalog;
		}

		/// <summary>
		/// This ECS's resident <see cref="Ladybug.ResourceCatalog"/>
		/// </summary>
		/// <remarks>
		/// Default: Parent Scene's <see cref="Ladybug.ResourceCatalog"/>
		/// </remarks>
		public ResourceCatalog ResourceCatalog { get; set; }

		/// <summary>
		/// This ECS' parent <see cref="Ladybug.Scene"/> object
		/// </summary>
		public Scene Scene { get; set; }

		/// <summary>
		/// List of Entities managed by this ECS
		/// </summary>
		public List<Entity> Entities { get; private set; } = new List<Entity>();

		/// <summary>
		/// Dictionary tracking which ComponentSystems are responsible for processing
		/// which Components
		/// </summary>
		private Dictionary<Type, Type> _componentsToSystems = new Dictionary<Type, Type>();

		/// <summary>
		/// Dictionary of Update methods called when this ECS is Updated
		/// </summary>
		private Dictionary<Type, Action<string, GameTime>> _updateMethods = new Dictionary<Type, Action<string, GameTime>>();
		
		/// <summary>
		/// Dictionary of Draw methods called when this ECS is Drawn
		/// </summary>
		private Dictionary<Type, Action<string, GameTime, SpriteBatch>> _drawMethods = new Dictionary<Type, Action<string, GameTime, SpriteBatch>>();

		/// <summary>
		/// List of individual steps that are executed, in given order, during Update
		/// </summary>
		private List<string> _updateSteps = new List<string> { "PreUpdate", "Update", "PostUpdate" };

		/// <summary>
		/// List of individual steps that are executed, in given order, during Draw
		/// </summary>
		private List<string> _drawSteps = new List<string> { "PreDraw", "Draw", "PostDraw" };

		/// <summary>
		/// Registers a <see cref="Ladybug.ECS.Component"/> type and the <see cref="Ladybug.ECS.ComponentSystem{T}"/> type that will be processing it.
		/// </summary>
		/// <typeparam name="C"><see cref="Ladybug.ECS.Component"/> type to be registered</typeparam>
		/// <typeparam name="S"><see cref="Ladybug.ECS.ComponentSystem{T}"/> type that will be processing the given Component type</typeparam>
		public void RegisterComponentSystem<C, S>() where C : Component where S : ComponentSystem<C>
		{
			var component = typeof(C);
			var system = typeof(S);
			if (!_componentsToSystems.ContainsKey(component))
			{
				_componentsToSystems.Add(component, system);
				var updateMethod = system.GetMethod("Update", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				var drawMethod = system.GetMethod("Draw", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

				Action<string, GameTime> updateAction = (string step, GameTime gameTime) =>
				{
					var args = new object[] { step, gameTime };
					updateMethod?.Invoke(null, args);
				};

				Action<string, GameTime, SpriteBatch> drawAction = (string step, GameTime gameTime, SpriteBatch spriteBatch) =>
				{
					var args = new object[] { step, gameTime, spriteBatch };
					drawMethod?.Invoke(null, args);
				};

				_updateMethods.Add(system, updateAction);
				_drawMethods.Add(system, drawAction);
			}
		}

		/// <summary>
		/// Registers a <see cref="Ladybug.ECS.Component"/> with this ECS
		/// </summary>
		/// <param name="component"><see cref="Ladybug.ECS.Component"/> to be registered with the ECS</param>
		public void RegisterComponent(Component component)
		{
			if (TryGetComponentSystem(component, out Type system))
			{
				var args = new object[] { (object)component };
				var method = system.GetMethod("RegisterComponent", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				method?.Invoke(null, args);
			}
			else
			{
				throw new InvalidOperationException("Error registering component. No ComponentSystem is registered for this component type!");
			}
		}
		
		/// <summary>
		/// Attempts to retrieve a reference to the <see cref="Ladybug.ECS.ComponentSystem{T}"/> type responsible
		/// for processing the given <see cref="Ladybug.ECS.Component"/>
		/// </summary>
		/// <param name="component"><see cref="Ladybug.ECS.Component"/> to attempt to find <see cref="Ladybug.ECS.ComponentSystem{T}"/> for</param>
		/// <param name="system">Reference to the found <see cref="Ladybug.ECS.ComponentSystem{T}"/></param>
		/// <returns>True if matching ComponentSystem is found, otherwise False</returns>
		public bool TryGetComponentSystem(Component component, out Type system)
		{
			system = default(Type);

			if (_componentsToSystems.ContainsKey(component.GetType()))
			{
				system = _componentsToSystems[component.GetType()];
			}

			return system != default(Type);
		}

		/// <summary>
		/// Creates a new <see cref="Ladybug.ECS.Entity"/> to be managed by this ECS
		/// </summary>
		/// <returns>Reference to the new <see cref="Ladybug.ECS.Entity"/></returns>
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

		/// <summary>
		/// Removes the given <see cref="Ladybug.ECS.Entity"/> from this ECS
		/// </summary>
		/// <param name="e"><see cref="Ladybug.ECS.Entity"/> to be removed from the ECS</param>
		public void RemoveEntity(Entity e)
		{
			if (!Entities.Contains(e))
			{
				return;
			}

			foreach (var c in e.Components)
			{
				if (TryGetComponentSystem(c, out Type system))
				{
					var args = new object[] { (object)c };
					system.GetMethod("DeregisterComponent", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.Invoke(null, args);
				}
			}
			Entities.Remove(e);
		}

		/// <summary>
		/// Attempts to find an <see cref="Ladybug.ECS.Entity"/>
		/// </summary>
		/// <param name="name">Name of <see cref="Ladybug.ECS.Entity"/> to find</param>
		/// <param name="entity">Reference to found <see cref="Ladybug.ECS.Entity"/></param>
		/// <returns>True if matching <see cref="Ladybug.ECS.Entity"/> is found, otherwise False</returns>
		/// <remarks>
		/// This method will find only the first matching Entity.
		/// Results may be unexpected if multiple Entities exist with
		/// the same name (use id overload instead if this is the case).
		/// </remarks>
		public bool TryFindEntity(string name, out Entity entity)
		{
			entity = Entities.Where((e => e.Name == name)).FirstOrDefault();
			return entity != null;
		}

		/// <summary>
		/// Attempts to find an <see cref="Ladybug.ECS.Entity"/>
		/// </summary>
		/// <param name="id">ID of the <see cref="Ladybug.ECS.Entity"/> to find</param>
		/// <param name="entity">Reference to the found <see cref="Ladybug.ECS.Entity"/></param>
		/// <returns>True if matching Entity is found, otherwise False</returns>
		public bool TryFindEntity(ulong id, out Entity entity)
		{
			entity = Entities.Where((e => e.ID == id)).FirstOrDefault();
			return entity != null;
		}

		/// <summary>
		/// Initializes all Entities managed by the ECS
		/// </summary>
		/// <remarks>
		/// Call this after all Entities have been added to the ECS.
		/// Entities added to the ECS during runtime will have to have
		/// Entity.Initialize() called on them individually
		/// </remarks>
		public void Initialize()
		{
			foreach (var entity in Entities)
			{
				entity.Initialize();
			}
		}

		/// <summary>
		/// Finds all Components of type T managed by this ECS
		/// </summary>
		/// <typeparam name="T">Type of <see cref="Ladybug.ECS.Component"/> to find</typeparam>
		/// <returns>List of all found Components</returns>
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

		/// <summary>
		/// Adds a step to this ECS's update process
		/// </summary>
		/// <param name="step">Name of step to be added</param>
		/// <param name="index">
		/// Position of step in update process. Appends the step to 
		/// the end of the process if set to -1 or not specified
		/// </param>
		public void AddUpdateStep(string step, int index = -1)
		{
			if (_updateSteps.Contains(step))
			{
				return;
			}

			if (index == -1)
			{
				_updateSteps.Add(step);
			}
			else
			{
				_updateSteps.Insert(index, step);
			}
		}

		/// <summary>
		/// Defines the update steps used by this ECS
		/// </summary>
		/// <param name="steps">List of update steps to be used by this ECS</param>
		public void SetUpdateSteps(List<string> steps) => _updateSteps = steps;
		
		/// <summary>
		/// Defines the update steps used by this ECS
		/// </summary>
		/// <param name="steps">List of update steps to be used by this ECS</param>
		public void SetUpdateSteps(params string[] steps) => SetUpdateSteps(steps.ToList());

		/// <summary>
		/// Adds a step to this ECS's draw process
		/// </summary>
		/// <param name="step">Name of draw step to be added</param>
		/// <param name="index">
		/// Position of step in draw process. Appends the step to 
		/// the end of the process if set to -1 or not specified
		/// </param>
		public void AddDrawStep(string step, int index = -1)
		{
			if (_drawSteps.Contains(step))
			{
				return;
			}

			if (index == -1)
			{
				_drawSteps.Add(step);
			}
			else
			{
				_drawSteps.Insert(index, step);
			}
		}

		/// <summary>
		/// Defines the draw steps used by this ECS
		/// </summary>
		/// <param name="steps">List of draw steps to be used by this ECS</param>
		public void SetDrawSteps(List<string> steps) => _drawSteps = steps;

		/// <summary>
		/// Defines the draw steps used by this ECS
		/// </summary>
		/// <param name="steps">List of draw steps to be used by this ECS</param>
		public void SetDrawSteps(params string[] steps) => SetDrawSteps(steps.ToList());

		/// <summary>
		/// Runs the update process for the current frame for all
		/// components managed by this ECS
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			foreach (var step in _updateSteps)
			{
				foreach (var update in _updateMethods.Values)
				{
					update(step, gameTime);
				}
			}
		}

		/// <summary>
		/// Runs the draw process for the current frame for all
		/// components managed by this ECS
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (var step in _drawSteps)
			{
				foreach (var draw in _drawMethods.Values)
				{
					draw(step, gameTime, spriteBatch);
				}
			}
		}

		/// <summary>
		/// Requests that the <see cref="Ladybug.ECS.ComponentSystem{T}"/> responsible for managing
		/// the given <see cref="Ladybug.ECS.Component"/> resort the draw priority of its components.
		/// </summary>
		/// <param name="component"></param>
		/// <remarks>
		/// This is automatically called whenever a Component's DrawPriority
		/// property is modified
		/// </remarks>
		public void RequestDrawSort(Component component)
		{
			if (TryGetComponentSystem(component, out Type system))
			{
				system.GetMethod("RequestDrawSort", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.Invoke(null, null);
			}
		}

		private ulong GetEntityID()
		{
			var res = _nextEntityID;
			_nextEntityID++;
			return res;
		}
	}
}