using System;
using Microsoft.Xna.Framework;

namespace Ladybug.ECS
{
	[Obsolete("Ladybug.ECS is being deprecated upon 2.0 release. Use Ladybug.Entities instead.", false)]
	public abstract class Component
	{	
		public Component(Entity entity = null, string Name = null)
		{
			SetEntity(entity);
		}

		public Entity Entity { get; private set; }

		public EntitySystem System { get => Entity.System; }

		public ResourceCatalog ResourceCatalog { get => System.ResourceCatalog; }

		public string Name { get; protected set; }

		public bool Active { get; protected set; } = true;

		public void SetName(string name)
		{
			Name = name;
		}

		public void SetActive(bool active)
		{
			Active = active;
		}

		public virtual void Initialize(){}
		
		public virtual void PreUpdate(GameTime gameTime){}

		public virtual void Update(GameTime gameTime){}
		
		public virtual void PostUpdate(GameTime gameTime){}

		public override string ToString() => $"{GetType()}";

		internal void SetEntity(Entity entity)
		{
			Entity = entity;
		}
	}
}