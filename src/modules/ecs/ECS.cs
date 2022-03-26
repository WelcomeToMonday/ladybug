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
			RegisterComponentSystem<ComposedComponent, ComposedComponentSystem>();
			RegisterComponentSystem<SpriteComponent, SpriteComponentSystem>();
			Scene = scene;
			ResourceCatalog = scene.ResourceCatalog;
		}

		/// <summary>\
		/// ECS Destructor
		/// </summary>
		~ECS()
		{
			Unload();
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
		/// Dictionary of PreUpdate methods called when this ECS is Updated
		/// </summary>
		private Dictionary<Type, Action<GameTime>> _preUpdateMethods = new Dictionary<Type, Action<GameTime>>();
		/// <summary>
		/// Dictionary of Update methods called when this ECS is Updated
		/// </summary>
		private Dictionary<Type, Action<GameTime>> _updateMethods = new Dictionary<Type, Action<GameTime>>();
		/// <summary>
		/// Dictionary of PostUpdate methods called when this ECS is Updated
		/// </summary>
		private Dictionary<Type, Action<GameTime>> _postUpdateMethods = new Dictionary<Type, Action<GameTime>>();

		/// <summary>
		/// Dictionary of PreDraw methods called when this ECS is Drawn
		/// </summary>
		private Dictionary<Type, Action<GameTime, SpriteBatch>> _preDrawMethods = new Dictionary<Type, Action<GameTime, SpriteBatch>>();
		/// <summary>
		/// Dictionary of Draw methods called when this ECS is Drawn
		/// </summary>
		private Dictionary<Type, Action<GameTime, SpriteBatch>> _drawMethods = new Dictionary<Type, Action<GameTime, SpriteBatch>>();
		/// <summary>
		/// Dictionary of PostDraw methods called when this ECS is Drawn
		/// </summary>
		private Dictionary<Type, Action<GameTime, SpriteBatch>> _postDrawMethods = new Dictionary<Type, Action<GameTime, SpriteBatch>>();

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

				var preUpdateMethod = system.GetMethod("PreUpdate", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				var updateMethod = system.GetMethod("Update", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				var postUpdateMethod = system.GetMethod("PostUpdate", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

				var preDrawMethod = system.GetMethod("Draw", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				var drawMethod = system.GetMethod("Draw", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				var postDrawMethod = system.GetMethod("Draw", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

				Action<GameTime> preUpdateAction = (GameTime gameTime) =>
				{
					var args = new object[] { gameTime };
					preUpdateMethod?.Invoke(null, args);
				};
				Action<GameTime> updateAction = (GameTime gameTime) =>
				{
					var args = new object[] { gameTime };
					updateMethod?.Invoke(null, args);
				};
				Action<GameTime> postUpdateAction = (GameTime gameTime) =>
				{
					var args = new object[] { gameTime };
					postUpdateMethod?.Invoke(null, args);
				};

				Action<GameTime, SpriteBatch> preDrawAction = (GameTime gameTime, SpriteBatch spriteBatch) =>
				{
					var args = new object[] { gameTime, spriteBatch };
					drawMethod?.Invoke(null, args);
				};
				Action<GameTime, SpriteBatch> drawAction = (GameTime gameTime, SpriteBatch spriteBatch) =>
				{
					var args = new object[] { gameTime, spriteBatch };
					drawMethod?.Invoke(null, args);
				};
				Action<GameTime, SpriteBatch> postDrawAction = (GameTime gameTime, SpriteBatch spriteBatch) =>
				{
					var args = new object[] { gameTime, spriteBatch };
					drawMethod?.Invoke(null, args);
				};

				_preUpdateMethods.Add(system, preUpdateAction);
				_updateMethods.Add(system, updateAction);
				_postUpdateMethods.Add(system, postUpdateAction);

				_preDrawMethods.Add(system, preDrawAction);
				_drawMethods.Add(system, drawAction);
				_postDrawMethods.Add(system, postDrawAction);
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
		/// Unloads the ECS, clearing all of its managed entities and components
		/// </summary>
		public void Unload()
		{
			foreach (var system in _componentsToSystems.Values)
			{
				system.GetMethod("DeregisterAll", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.Invoke(null, null);
			}
		}

		/// <summary>
		/// Runs the update process for the current frame for all
		/// components managed by this ECS
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			foreach (var preUpdate in _preUpdateMethods.Values)
			{
				preUpdate(gameTime);
			}
			foreach (var update in _updateMethods.Values)
			{
				update(gameTime);
			}
			foreach (var postUpdate in _postUpdateMethods.Values)
			{
				postUpdate(gameTime);
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
			foreach (var preDraw in _preDrawMethods.Values)
			{
				preDraw(gameTime, spriteBatch);
			}
			foreach (var draw in _drawMethods.Values)
			{
				draw(gameTime, spriteBatch);
			}
			foreach (var postDraw in _postDrawMethods.Values)
			{
				postDraw(gameTime, spriteBatch);
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