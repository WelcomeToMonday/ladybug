using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Input;
using Ladybug.ECS;
using Ladybug.SceneManagement;

public enum UIState 
	{
	 ACTIVE, // Update and Draw called every frame
	 PAUSED, // Draw called every frame, but not Update
	 SUSPENDED // Neither Update nor Draw called
	}

namespace Ladybug.Core.UI
{
	public class UI
	{

		public event EventHandler<UIControlChangeEvent> FocusChange;

		public event EventHandler<UIClickEvent> ClickStart;
		public event EventHandler<UIClickEvent> ClickHold;
		public event EventHandler<UIClickEvent> ClickEnd;

		public event EventHandler<UIClickEvent> RightClickStart;
		public event EventHandler<UIClickEvent> RightClickHold;
		public event EventHandler<UIClickEvent> RightClickEnd;

		public event EventHandler<UIStateChangeEvent> StateChanged;

		private Panel m_rootPanel;

		protected MouseMonitor MouseMonitor { get; set; }
		protected KeyboardMonitor KeyboardMonitor { get; set; }
		protected GamepadMonitor GamepadMonitor { get; set; }

		public UIState State { get; protected set; } = UIState.ACTIVE;

		public UI(UIConfig config)
		{
			DefaultFont = config.DefaultFont;
			RootPanel.SetBounds(config.Bounds);
			RootPanel.SetFont(config.DefaultFont);
			Inputs = config.Inputs;
			SceneManager = config.SceneManager;
			Catalog = config.Catalog;

			if (config.DefaultBackground != null)
			{
				DefaultBackground = config.DefaultBackground;
			}

			MouseMonitor = new MouseMonitor();
			KeyboardMonitor = new KeyboardMonitor();
			GamepadMonitor = new GamepadMonitor();
		}

		public Control this[string name] { get => RootPanel[name]; }

		public SpriteFont DefaultFont { get; private set; }

		public Texture2D DefaultBackground { get; private set; }

		public Input Inputs { get; set; }

		public ResourceCatalog Catalog { get; set; }

		public Control FocusedControl { get; private set; }

		public SceneManager SceneManager { get; set; }

		public Panel RootPanel
		{
			get
			{
				if (m_rootPanel == null)
				{
					m_rootPanel = new Panel();
					m_rootPanel.UI = this;
				}
				return m_rootPanel;
			}
		}

		public Vector2 CursorPosition { get; protected set; }

		public void Pause()
		{
			if (State != UIState.PAUSED)
			{
				SetState(UIState.PAUSED);
			}
		}

		public void Unpause()
		{
			if (State == UIState.PAUSED)
			{
				SetState(UIState.ACTIVE);
			}
		}

		public void Suspend()
		{
			if (State != UIState.SUSPENDED)
			{
				SetState(UIState.SUSPENDED);
			}
		}

		public void Unsuspend()
		{
			if (State == UIState.SUSPENDED)
			{
				SetState(UIState.ACTIVE);
			}
		}

		public void SetState(UIState newState)
		{
			var oldState = State;
			State = newState;
			StateChanged?.Invoke(this, new UIStateChangeEvent(newState, oldState));
		}

		public void AddControl(Control control)
		{
			RootPanel.AddControl(control);
		}

		public void SetFocus(Control control)
		{
			if (FocusedControl != control)
			{
				var oldControl = FocusedControl;

				FocusedControl = control;
				FocusChange?.Invoke(this, new UIControlChangeEvent(control, oldControl));
			}
		}

		public void ClearFocus()
		{
			SetFocus(null);
		}

		private Vector2 GetCursorPosition()
		{
			Vector2 res = Vector2.Zero;

			if (Inputs.HasFlag(Input.Mouse))
			{
				res = MouseMonitor.GetCursorPosition();
			}
			else
			{
				if (FocusedControl != null)
				{
					res = FocusedControl.Bounds.Center.ToVector2();
				}
			}

			return res;
		}

		protected void OnClickStart(UIClickEvent e)
		{
			ClickStart?.Invoke(this, e);
		}

		protected void OnClickHold(UIClickEvent e)
		{
			ClickHold?.Invoke(this, e);
		}

		protected void OnClickEnd(UIClickEvent e)
		{
			ClickEnd?.Invoke(this, e);
		}

		protected void OnRightClickStart(UIClickEvent e)
		{
			RightClickStart?.Invoke(this, e);
		}

		protected void OnRightClickHold(UIClickEvent e)
		{
			RightClickHold?.Invoke(this, e);
		}

		protected void OnRightClickEnd(UIClickEvent e)
		{
			RightClickEnd?.Invoke(this, e);
		}

		protected virtual void HandleInput()
		{
			if (Inputs.HasFlag(Input.Mouse))
			{
				MouseMonitor.BeginUpdate(Mouse.GetState());

				var cPos = MouseMonitor.GetCursorPosition();

				if (MouseMonitor.CheckButton(MouseButtons.LeftClick, InputState.Pressed))
				{
					OnClickStart(new UIClickEvent(cPos));
				}

				if (MouseMonitor.CheckButton(MouseButtons.LeftClick, InputState.Down))
				{
					OnClickHold(new UIClickEvent(cPos));
				}

				if (MouseMonitor.CheckButton(MouseButtons.LeftClick, InputState.Released))
				{
					OnClickEnd(new UIClickEvent(cPos));
				}

				if (MouseMonitor.CheckButton(MouseButtons.RightClick, InputState.Pressed))
				{
					OnRightClickStart(new UIClickEvent(cPos));
				}

				if (MouseMonitor.CheckButton(MouseButtons.RightClick, InputState.Down))
				{
					OnRightClickHold(new UIClickEvent(cPos));
				}

				if (MouseMonitor.CheckButton(MouseButtons.RightClick, InputState.Released))
				{
					OnRightClickEnd(new UIClickEvent(cPos));
				}

				MouseMonitor.EndUpdate();
			}

			CursorPosition = GetCursorPosition();
		}

		public virtual void Update()
		{
			if (State == UIState.ACTIVE)
			{
				HandleInput();
				RootPanel.Update();
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (State != UIState.SUSPENDED)
			{
				RootPanel.Draw(spriteBatch);
			}
		}
	}
}