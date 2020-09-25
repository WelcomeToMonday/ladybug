using System;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Ladybug.Graphics;
using Ladybug.Graphics.BoxModel;

namespace Ladybug.Core.UI
{
	public class TextBox : Control
	{
		private StringBuilder _stringBuilder;

		public TextBox(Control parentControl = null, string name = "") : base(parentControl, name)
		{
			PositionChanged += OnPositionChanged;
			ClickStart += OnClick;
			Focus += OnFocus;
			UnFocus += OnUnFocus;
			ClickOut += OnClickOut;
		}

		public Panel Panel { get; set; }
		public Label Label { get; set; }

		public int MaxCharacters { get; set; } = 16;

		public override void Initialize()
		{
			base.Initialize();

			Panel = new Panel(this);
			Label = new Label(this);

			_stringBuilder = new StringBuilder(Label.Text);

			Panel.BackgroundImage = null;
			Panel.SetBounds(
				new Rectangle(0, 0, 400, (int)Label.Font.MeasureString(" ").Y + 10)
			);

			SetBounds(Panel.Bounds);
		}

		private void OnPositionChanged(object sender, EventArgs e)
		{
			Panel.SetBounds(Bounds);
			Label.SetBounds(Label.Bounds.CopyAtPosition
				(
					new Vector2(
						(int)Panel.Bounds.GetHandlePosition(BoxHandle.BOTTOMLEFT).X + 5,
						(int)Panel.Bounds.GetHandlePosition(BoxHandle.BOTTOMLEFT).Y - 5
					),
					BoxHandle.BOTTOMLEFT
				)
			);
		}

		public virtual void OnClick(object sender, EventArgs e)
		{
			UI.SetFocus(this);
		}

		public virtual void OnFocus(object sender, EventArgs e)
		{
			UI.SceneManager.Window.TextInput += HandleTextInput;
		}

		public virtual void OnClickOut(object sender, EventArgs e)
		{
			if (HasFocus)
			{
				UI.ClearFocus();
			}
		}

		public virtual void OnUnFocus(object sender, EventArgs e)
		{
			UI.SceneManager.Window.TextInput -= HandleTextInput;
		}

		public virtual void HandleTextInput(object sender, TextInputEventArgs e)
		{
			var glpyhs = Font.GetGlyphs();

			if (e.Key == Keys.Back)
			{
				if (_stringBuilder.Length > 0)
				{
					_stringBuilder.Remove(_stringBuilder.Length - 1, 1);
				}
			}
			else if (e.Key != Keys.Enter && _stringBuilder.Length <= MaxCharacters)
			{
				if (glpyhs.ContainsKey(e.Character))
				{
					_stringBuilder.Append(e.Character);
				}
			}


			Label.SetText(_stringBuilder.ToString());
		}

		public override void Update()
		{
			if (HasFocus)
			{
				//capture keyboard input
			}
			else
			{

			}

			base.Update();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (BackgroundImage != null)
			{
				spriteBatch.Draw
				(
					BackgroundImage,
					Bounds,
					null,
					Color.White
				);
			}
			Panel.Draw(spriteBatch);
			Label.Draw(spriteBatch);
		}
	}
}