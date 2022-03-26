using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.UI
{
	/// <summary>
	/// Base class for Control container UI components
	/// </summary>
	public abstract class ControlContainer : Control, IControlContainer
	{
		/// <summary>
		/// Creates a new ControlContainer
		/// </summary>
		public ControlContainer()
		{
			Controls = _controls.AsReadOnly();
		}

		/// <summary>
		/// Access one of this Control's children by name
		/// </summary>
		/// <value></value>
		public Control this[string name]
		{
			get => FindControl<Control>(name);
		}

		/// <summary>
		/// List of Controls within this container
		/// </summary>
		/// <value></value>
		public IList<Control> Controls { get; private set; }
		private List<Control> _controls = new List<Control>();

		/// <summary>
		/// Finds a control within this container
		/// </summary>
		/// <param name="name"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual T FindControl<T>(string name) where T : Control =>
		Controls.OfType<T>().Where(control => control.Name == name).FirstOrDefault();

		/// <summary>
		/// Adds a new child Control of type T to this Control
		/// </summary>
		/// <param name="name">Name of the new Control</param>
		/// <typeparam name="T">Type of the new Control</typeparam>
		/// <returns>Reference to the current Control</returns>
		public ControlContainer AddControl<T>(string name = null) where T : Control, new() => AddControl<T>(name, out T control);

		/// <summary>
		/// Adds a new child Control of type T to this Control
		/// </summary>
		/// <param name="control">Reference to the new Control</param>
		/// <typeparam name="T">Type of the new Control</typeparam>
		/// <returns>Reference to the current Control</returns>
		public ControlContainer AddControl<T>(out T control) where T : Control, new() => AddControl<T>(null, out control);

		/// <summary>
		/// Adds a new child Control of type T to this Control
		/// </summary>
		/// <param name="name">Name of the new Control</param>
		/// <param name="control">Reference to the new Control</param>
		/// <typeparam name="T">Type of the new Control</typeparam>
		/// <returns>Reference to the current Control</returns>
		public ControlContainer AddControl<T>(string name, out T control) where T : Control, new()
		{
			control = new T();

			if (name != null && name != string.Empty)
			{
				control.Name = name;
			}
			control._Attach(this);
			_AddChild(control);
			return this;
		}

		/// <summary>
		/// Adds an existing Control as a child of this Control
		/// </summary>
		/// <param name="control">Control to add as a child of this Control</param>
		/// <returns>Reference to the current Control</returns>
		public ControlContainer AddControl(Control control)
		{
			control._Attach(this);
			_AddChild(control);
			return this;
		}

		/// <summary>
		/// Called when a child control is attached to this Control
		/// </summary>
		/// <param name="childControl"></param>
		protected virtual void AddChild(Control childControl) { }
		internal void _AddChild(Control childControl)
		{
			if (!_controls.Contains(childControl))
			{
				_controls.Add(childControl);
				AddChild(childControl);
			}
		}

		/// <summary>
		/// Updates this container and its controls each frame
		/// </summary>
		public override void Update()
		{
			base.Update();
			for (var i = 0; i < Controls.Count; i++)
			{
				Controls[i].Update();
			}
		}

		/// <summary>
		/// Draws this container and its controls each frame
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (Visible)
			{
				base.Draw(spriteBatch);
				for (var i = 0; i < Controls.Count; i++)
				{
					Controls[i].Draw(spriteBatch);
				}
			}
		}
	}
}