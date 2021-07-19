using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.UserInput;

namespace Ladybug.Beta.UI
{
	public class UI : Control
	{
		private List<Control> _controls = new List<Control>();

		private List<Control> _controlsByPriority = new List<Control>();

		private bool _sortRequired = false;

		public UI(Scene scene) : base()
		{
			UI = this;
			Scene = scene;
			ResourceCatalog = Scene.ResourceCatalog;
			Controls = _controls.AsReadOnly();

			BlockCursor = false;

			_Initialize();
		}

		public Control FocusedControl { get; private set; }

		public Control TargetedControl { get; private set; }

		public IList<Control> Controls { get; private set; }

		public new Scene Scene { get; private set; }

		public VRC VRC { get; set; }

		public new ResourceCatalog ResourceCatalog { get; set; }

		public void RequestSort()
		{
			_sortRequired = true;
		}

		public Vector2 GetCursorPosition()
		{
			var res = Input.Mouse.GetCursorPosition();

			if (VRC != null)
			{
				res = VRC.ScreenToCanvasSpace(res);
			}

			return res;
		}

		protected override void AddChild(Control control)
		{
			_controls.Add(control);
			_controlsByPriority.Add(control);
			RequestSort();
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

			if (TargetedControl != null && clickState != InputState.Up)
			{
				TargetedControl._Click(clickState);
			}
		}

		public new void Update()
		{
			CheckInput();
			_Update();
			for (var i = 0; i < Controls.Count; i++)
			{
				Controls[i]._Update();
			}
		}

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