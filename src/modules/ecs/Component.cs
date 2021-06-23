using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.ECS
{
	public abstract class Component
	{
		private int _drawPriority = 100;

		private Action _onInitialize = () => { };

		internal Dictionary<string, Action<GameTime>> _UpdateSteps { get; private set; } = new Dictionary<string, Action<GameTime>>();
		internal Dictionary<string, Action<GameTime, SpriteBatch>> _DrawSteps { get; private set; } = new Dictionary<string, Action<GameTime, SpriteBatch>>();

		public string Name { get; set; }

		public Entity Entity { get; internal set; }
		public ECS ECS { get => Entity.ECS; }
		public Scene Scene { get => ECS.Scene; }
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
					ECS.RequestDrawSort(this);
				}
			}
		}

		internal void _Initialize()
		{
			_onInitialize();
		}

		public virtual bool CheckRunUpdate() => Entity.Active && Active;

		public virtual bool CheckRunDraw() => Entity.Visible && Visible;

		public Component OnInitialize(Action action)
		{
			_onInitialize = action;
			return this;
		}

		public Component OnUpdate(Action<GameTime> action) => OnUpdate("Update", action);

		public Component OnUpdate(string step, Action<GameTime> action)
		{
			_UpdateSteps[step] = action;
			return this;
		}

		public Component OnDraw(Action<GameTime, SpriteBatch> action) => OnDraw("Draw", action);

		public Component OnDraw(string step, Action<GameTime, SpriteBatch> action)
		{
			_DrawSteps[step] = action;
			return this;
		}
	}
}