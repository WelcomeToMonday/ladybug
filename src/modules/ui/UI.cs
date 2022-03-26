using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.UserInput;

namespace Ladybug.UI
{
	/// <summary>
	/// Static class containing resource keys for common UI resources
	/// </summary>
	public static class UIResources
	{
		/// <summary>
		/// Default background texture used by this UI's controls
		/// </summary>
		public static readonly string DefaultBackground = "ladybug_ui_background_default";

		/// <summary>
		/// Default font used by this UI's controls
		/// </summary>
		public static readonly string DefaultFont = "ladybug_ui_font_default";
	}

	/// <summary>
	/// Ladybug root UI manager
	/// </summary>
	public class UI : ControlContainer
	{
		private List<Control> _allControls = new List<Control>();

		private bool _sortRequired = false;

		/// <summary>
		/// Creates a new UI
		/// </summary>
		/// <param name="scene"><see cref="Ladybug.Scene"/> managing this UI</param>
		/// <returns></returns>
		public UI(Scene scene) : base()
		{
			UI = this;
			Scene = scene;
			ResourceCatalog = Scene.ResourceCatalog;
			AllControls = _allControls.AsReadOnly();
			ZIndex = -1;

			BlockCursor = false;
			_Initialize();
		}

		/// <summary>
		/// Control that is currently in focus
		/// </summary>
		public Control FocusedControl { get; private set; }

		/// <summary>
		/// Control that is currently targeted by the cursor
		/// </summary>
		public Control TargetedControl { get; private set; }

		/// <summary>
		/// List of Controls managed by this UI
		/// </summary>
		public IList<Control> AllControls { get; private set; }

		/// <summary>
		/// Scene that is managing this UI
		/// </summary>
		public new Scene Scene { get; private set; }

		/// <summary>
		/// Virtual Resolution Container that is rendering this UI
		/// </summary>
		public VRC VRC { get; set; }

		/// <summary>
		/// This UI's resident ResourceCatalog
		/// </summary>
		public new ResourceCatalog ResourceCatalog { get; set; }

		/// <summary>
		/// Locates a Control managed by this UI by name
		/// </summary>
		/// <param name="name"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public override T FindControl<T>(string name)
		=> AllControls.OfType<T>().Where(control => control.Name == name).FirstOrDefault();

		/// <summary>
		/// Request the UI update the order of managed controls.
		/// </summary>
		public void RequestSort()
		{
			_sortRequired = true;
		}

		/// <summary>
		/// Get the current position of the cursor
		/// </summary>
		public Vector2 GetCursorPosition()
		{
			var res = Input.Mouse.GetCursorPosition();

			if (VRC != null)
			{
				res = VRC.ScreenToCanvasSpace(res);
			}

			return res;
		}

		/// <summary>
		/// Registers a Control to be managed by this UI
		/// </summary>
		/// <param name="control"></param>
		public void RegisterControl(Control control)
		{
			if (_allControls.Contains(control))
			{
				return;
			}
			_allControls.Add(control);
			RequestSort();
		}

		/// <summary>
		/// Sets the UI's focused control
		/// </summary>
		/// <param name="control"></param>
		public void SetFocus(Control control)
		{
			if (FocusedControl == control || !Controls.Contains(control))
			{
				return;
			}

			if (FocusedControl != null)
			{
				FocusedControl._Unfocus();
				FocusedControl = null;
			}

			FocusedControl = control;
			FocusedControl._Focus();
		}

		/// <summary>
		/// Clear's the UI's focused control
		/// </summary>
		public void ClearFocus()
		{
			if (FocusedControl != null)
			{
				FocusedControl._Unfocus();
				FocusedControl = null;
			}
		}

		private void CheckInput()
		{
			TargetedControl = null;

			for (var i = 0; i < AllControls.Count; i++)
			{
				var control = AllControls[i];

				if (!control.Active || !control.Visible)
				{
					continue;
				}

				if (control.Bounds.Contains(GetCursorPosition()))
				{
					TargetedControl = control;
					if (control.BlockCursor)
					{
						break;
					}
				}
			}

			var clickState = Input.Mouse.GetInputState(MouseButtons.Left);
			if (clickState != InputState.Up)
			{
				if (FocusedControl != null && TargetedControl != FocusedControl)
				{
					FocusedControl._ClickOut();
				}

				TargetedControl?._Click(clickState);
			}
		}

		/// <summary>
		/// Updates the UI and its components each frame
		/// </summary>
		public override void Update()
		{
			if (!Active)
			{
				return;
			}

			if (_sortRequired)
			{
				_allControls.Sort((Control x, Control y) =>
				{
					var res = 0;
					if (x.ZIndex < y.ZIndex) res = 1;
					if (x.ZIndex > y.ZIndex) res = -1;
					return res;
				});
				_sortRequired = false;
			}

			CheckInput();
			base.Update();
		}

		/// <summary>
		/// Draws the UI and its components each frame
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (!Visible)
			{
				return;
			}

			base.Draw(spriteBatch);
		}
	}
}