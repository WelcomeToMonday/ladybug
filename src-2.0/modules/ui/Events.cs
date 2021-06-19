using System;

using Microsoft.Xna.Framework;

namespace Ladybug.UI
{
	public class UIControlChangeEvent : EventArgs
	{
		public UIControlChangeEvent(Control newControl, Control previousControl) : base()
		{
			NewControl = newControl;
			PreviousControl = previousControl;
		}

		public Control NewControl { get; private set; }
		public Control PreviousControl { get; private set; }
	}

	public class UIClickEvent : EventArgs
	{
		public UIClickEvent(Vector2 cursorPosition)
		{
			CursorPosition = cursorPosition;
		}

		public Vector2 CursorPosition { get; private set; }
	}

	public class UIStateChangeEvent : EventArgs
	{
		public UIStateChangeEvent(UIState newState, UIState oldState)
		{
			NewState = newState;
			OldState = oldState;
		}

		public UIState NewState { get; private set; }
		public UIState OldState { get; private set; }
	}

	public class ControlMoveEvent : EventArgs
	{
		public ControlMoveEvent(Vector2 oldPosition, Vector2 newPosition)
		{
			OldPosition = oldPosition;
			NewPosition = newPosition;

			var delta = newPosition - oldPosition;
			XOffset = (int)delta.X;
			YOffset = (int)delta.Y;
		}
		
		public Vector2 OldPosition { get; private set; }
		public Vector2 NewPosition { get; private set; }

		public int XOffset { get; private set; }

		public int YOffset { get; private set; }
	}
}