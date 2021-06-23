using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.ECS.Components;

namespace Ladybug.ECS
{
	public class ECS
	{
		private ulong _nextEntityID = 0;

		public ECS(Scene scene)
		{
			RegisterComponentSystem<InputComponent, InputComponentSystem>();
			Scene = scene;
		}

		public ResourceCatalog ResourceCatalog { get; set; }
		public Scene Scene { get; set; }

		public List<Entity> Entities { get; private set; } = new List<Entity>();

		private Dictionary<Type, Type> _componentsToSystems = new Dictionary<Type, Type>();

		private Dictionary<Type, Action<string, GameTime>> _updateMethods = new Dictionary<Type, Action<string, GameTime>>();
		private Dictionary<Type, Action<string, GameTime, SpriteBatch>> _drawMethods = new Dictionary<Type, Action<string, GameTime, SpriteBatch>>();

		private List<string> _updateSteps = new List<string> { "PreUpdate", "Update", "PostUpdate" };
		private List<string> _drawSteps = new List<string> { "PreDraw", "Draw", "PostDraw" };

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

		public bool TryGetComponentSystem(Component component, out Type system)
		{
			system = default(Type);

			if (_componentsToSystems.ContainsKey(component.GetType()))
			{
				system = _componentsToSystems[component.GetType()];
			}

			return system != default(Type);
		}

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
				if (TryGetComponentSystem(c, out Type system))
				{
					var args = new object[] { (object)c };
					system.GetMethod("DeregisterComponent", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.Invoke(null, args);
				}
			}
			Entities.Remove(e);
		}

		public Entity FindEntity(string name)
		=> Entities.Where((e => e.Name == name)).FirstOrDefault();

		public void Initialize()
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

		public void SetUpdateSteps(List<string> steps) => _updateSteps = steps;
		public void SetUpdateSteps(params string[] steps) => SetUpdateSteps(steps.ToList());

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

		public void SetDrawSteps(List<string> steps) => _drawSteps = steps;
		public void SetDrawSteps(params string[] steps) => SetDrawSteps(steps.ToList());

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