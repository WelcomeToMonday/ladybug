using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.UserInput;

namespace Ladybug.Beta.UI
{
	public class UI : Panel
	{
		private List<Control> _controls = new List<Control>();

		private List<Control> _controlsByPriority = new List<Control>();

		private bool _sortRequired = false;

		public UI(Scene scene) : base()
		{
			Scene = scene;
			Controls = _controls.AsReadOnly();
			_OnInitialize();
			OnAddChild(AddChild);
		}

		public Control FocusedControl { get; private set; }

		public IList<Control> Controls { get; private set; }

		public Scene Scene { get; private set; }

		public VRC VRC { get; set; }

		public void RequestSort()
		{
			_sortRequired = true;
		}

		private Vector2 GetCursorPosition()
		{
			var res = Input.Mouse.GetCursorPosition();

			if (VRC != null)
			{
				res = VRC.ScreenToCanvasSpace(res);
			}

			return res;
		}

		private void AddChild(Control control)
		{
			_controls.Add(control);
			_controlsByPriority.Add(control);
			RequestSort();
		}

		private void CheckInput()
		{
			for (var i = 0; i < _controlsByPriority.Count; i++)
			{
				var control = _controlsByPriority[i];
				if (control.Bounds.Contains(GetCursorPosition()))
				{
					//_control._OnCursorOver(); //we'll use this for CursorEnter/CursorLeave
					// actually, we can probably do this in Control.cs in the update loop better.
					if (control.BlockCursor)
					{
						break;
					}
				}
			}
		}

		public void Update()
		{
			CheckInput();
			_OnUpdate();
			for (var i = 0; i < Controls.Count; i++)
			{
				Controls[i]._OnUpdate();
			}
		}

		public void Draw(SpriteBatch spriteBatch)
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

			_OnDraw(spriteBatch);

			for (var i = 0; i < Controls.Count; i++)
			{
				Controls[i]._OnDraw(spriteBatch);
			}
		}
	}
}