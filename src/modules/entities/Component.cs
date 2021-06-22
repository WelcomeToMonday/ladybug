using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Entities
{
	public class Component
	{
		private int _drawPriority = 100;

		private Action _onInitialize = () => { };

		public string Name { get; set; }

		public Dictionary<string, Action<GameTime>> UpdateSteps { get; protected set; } = new Dictionary<string, Action<GameTime>>();
		public Dictionary<string, Action<GameTime, SpriteBatch>> DrawSteps { get; protected set; } = new Dictionary<string, Action<GameTime, SpriteBatch>>();

		public Entity Entity { get; internal set; }
		public EntitySystem EntitySystem { get => Entity.EntitySystem; }
		public Scene Scene { get => EntitySystem.Scene; }
		public Game Game { get => Scene.Game; }

		public bool Active { get; set; } = true;
		public bool Visible { get; set; } = true;
		public int DrawPriority
		{
			get => _drawPriority;
			set
			{
				if (_drawPriority != value)
				{
					_drawPriority = value;
					Entity.EntitySystem.SortDrawQueue();
				}
			}
		}

		internal void _Initialize()
		{
			_onInitialize();
		}

		public Component OnInitialize(Action action)
		{
			_onInitialize = action;
			return this;
		}

		public Component OnUpdateStep(string step, Action<GameTime> action)
		{
			UpdateSteps[step] = action;
			return this;
		}

		public Component OnDrawStep(string step, Action<GameTime, SpriteBatch> action)
		{
			DrawSteps[step] = action;
			return this;
		}
	}
}