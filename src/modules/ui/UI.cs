using System;
using System.Collections.Generic;

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
	public class UI : Control
	{
		private List<Control> _controls = new List<Control>();

		private List<Control> _controlsByPriority = new List<Control>();

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
			Controls = _controls.AsReadOnly();
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
		public IList<Control> Controls { get; private set; }

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
		/*
		/// <summary>
		/// Called when a Control is attached to this UI
		/// </summary>
		/// <param name="control"></param>
		protected override void AddChild(Control control)
		{
			_controls.Add(control);
			_controlsByPriority.Add(control);
			RequestSort();
		}
		*/

		/// <summary>
		/// Registers a Control to be managed by this UI
		/// </summary>
		/// <param name="control"></param>
		public void RegisterControl(Control control)
		{
			if (_controls.Contains(control))
			{
				return;
			}

			_controls.Add(control);
			_controlsByPriority.Add(control);
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

			for (var i = 0; i < _controlsByPriority.Count; i++)
			{
				var control = _controlsByPriority[i];
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
		/// Called when the UI is updated
		/// </summary>
		public new void Update()
		{
			CheckInput();
			_Update();
			for (var i = 0; i < Children.Count; i++)
			{
				Controls[i]._Update();
			}
		}

		/// <summary>
		/// Called when the UI is drawn
		/// </summary>
		/// <param name="spriteBatch"></param>
		public new void Draw(SpriteBatch spriteBatch)
		{
			if (_sortRequired)
			{
				_controlsByPriority.Sort((Control x, Control y) =>
				{
					var res = 0;
					if (x.ZIndex > y.ZIndex) res = 1; //todo: verify correct </> values
					if (x.ZIndex < y.ZIndex) res = -1;
					return res;
				});
			}

			_Draw(spriteBatch);

			for (var i = 0; i < Controls.Count; i++)
			{
				Controls[i]._Draw(spriteBatch);
			}
		}
	}
}