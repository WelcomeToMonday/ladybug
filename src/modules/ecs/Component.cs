using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.ECS
{
	/// <summary>
	/// A Component that can be registered and managed
	/// by an <see cref="Ladybug.ECS.ECS"/> instance
	/// </summary>
	public abstract class Component
	{
		private int _drawPriority = 100;

		private Action _onInitialize = () => { };

		internal Dictionary<string, Action<GameTime>> _UpdateSteps { get; private set; } = new Dictionary<string, Action<GameTime>>();
		internal Dictionary<string, Action<GameTime, SpriteBatch>> _DrawSteps { get; private set; } = new Dictionary<string, Action<GameTime, SpriteBatch>>();

		/// <summary>
		/// Name of the Component instance
		/// </summary>
		/// <value></value>
		public string Name { get; set; }

		/// <summary>
		/// <see cref="Ladybug.ECS.Entity"/> that contains this Component
		/// </summary>
		/// <value></value>
		public Entity Entity { get; internal set; }

		/// <summary>
		/// <see cref="Ladybug.ECS.ECS"/> that is managing this Component
		/// </summary>
		/// <value></value>
		public ECS ECS { get => Entity.ECS; }

		/// <summary>
		/// <see cref="Ladybug.Scene"/> that contains the <see cref="Ladybug.ECS.ECS"/> that
		/// manages this Component
		/// </summary>
		/// <value></value>
		public Scene Scene { get => ECS.Scene; }

		/// <summary>
		/// <see cref="Ladybug.Game"/> instance that is managing the
		/// <see cref="Ladybug.Scene"/> that contains the 
		/// <see cref="Ladybug.ECS.Entity"/> managing this Component
		/// </summary>
		/// <value></value>
		public Game Game { get => Scene.Game; }

		/// <summary>
		/// Whether the Component is active
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// By default, Active is used to
		/// determine whether to update this
		/// Component. This behavior can be changed
		/// by overriding <see cref="CheckRunUpdate()"/>
		/// </remarks>
		public bool Active { get; set; } = true;

		/// <summary>
		/// Whether the Component is visible
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// By default, Visible is used to
		/// determine whether to draw this
		/// Component. This behavior can be changed
		/// by overriding <see cref="CheckRunDraw()"/>
		/// </remarks>
		public bool Visible { get; set; } = true;

		/// <summary>
		/// The Draw Priority of this Component
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// Components with a smaller value in DrawPriority
		/// are drawn earlier than those with a larger value
		/// </remarks>
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

		/// <summary>
		/// Determines whether to Update this component each frame
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// By default, checks that Active and Entity.Active are true
		/// </remarks>
		public virtual bool CheckRunUpdate() => Entity.Active && Active;

		/// <summary>
		/// Determines whether to Draw this component each frame
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// By default, checks that <see cref="Visible"/> and <see cref="Entity.Visible"/> are true
		/// </remarks>
		public virtual bool CheckRunDraw() => Entity.Visible && Visible;

		/// <summary>
		/// Defines this Component's behavior upon initialization
		/// </summary>
		/// <param name="action">Action to be performed by this Component upon initialization</param>
		/// <returns></returns>
		public Component OnInitialize(Action action)
		{
			_onInitialize = action;
			return this;
		}

		/// <summary>
		/// Defines this Component's behavior upon updating
		/// </summary>
		/// <param name="action">Action to be performed by this Component upon each update</param>
		/// <returns></returns>
		/// <remarks>
		/// This overload sets the action taken during the "Update" Update step
		/// </remarks>
		public Component OnUpdate(Action<GameTime> action) => OnUpdate("Update", action);

		/// <summary>
		/// Defines this Component's behavior upon the given update step
		/// </summary>
		/// <param name="step">Update step on which the given action will be performed</param>
		/// <param name="action">Action to perform on the given Update step</param>
		/// <returns></returns>
		public Component OnUpdate(string step, Action<GameTime> action)
		{
			_UpdateSteps[step] = action;
			return this;
		}

		/// <summary>
		/// Defines this Component's behavior upon being drawn
		/// </summary>
		/// <param name="action">Action to be performed by this Component upon each draw call</param>
		/// <returns></returns>
		/// <remarks>
		/// This overload sets the action taken upon the "Draw" Draw step
		/// </remarks>
		public Component OnDraw(Action<GameTime, SpriteBatch> action) => OnDraw("Draw", action);

		/// <summary>
		/// Defines this Component's behavior upon the given draw step
		/// </summary>
		/// <param name="step">Draw step on which the given action will be performed</param>
		/// <param name="action">Action to perform on the given Draw step</param>
		/// <returns></returns>
		public Component OnDraw(string step, Action<GameTime, SpriteBatch> action)
		{
			_DrawSteps[step] = action;
			return this;
		}
	}
}