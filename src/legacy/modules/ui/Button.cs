#pragma warning disable 1591 // Hide XMLdoc warnings.

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.Legacy.UI
{
	[Obsolete("Ladybug Legacy UI is Obsolete. Please upgrade to 2.0 API ASAP")]
	public class Button : Control
	{

		public Button(Control parentControl = null, string name = "") : base (parentControl, name)
		{
			PositionChanged += OnPositionChanged;
			ClickStart += OnClick;
			ClickOut += OnClickOut;
		}

		public override void Initialize()
		{
			base.Initialize();
			//UI = parentControl?.UI;

			Panel = new Panel(this);
			Label = new Label(this);

			Panel.BackgroundImage = null;

			Panel.SetBounds(
				new Rectangle(
					(int)Label.Bounds.X,
					(int)Label.Bounds.Y,
					Label.Bounds.Width + 40,
					Label.Bounds.Height + 20
					)
			);

			SetBounds(Panel.Bounds);

			Label.SetBounds(
				Label.Bounds.CopyAtPosition(
					Panel.Bounds.GetHandlePosition(BoxHandle.Center), 
					BoxHandle.Center)
			);
		}

		protected virtual void OnPositionChanged(object sender, EventArgs e)
		{
			Panel.SetBounds(Bounds);
			SetLabelText(Label.Text);
		}

		protected virtual void OnClick(object sender, EventArgs e)
		{
			UI.SetFocus(this);
		}

		protected virtual void OnClickOut(object sender, EventArgs e)
		{
			if (HasFocus)
			{
				UI.ClearFocus();
			}
		}

		public void SetLabelText(string labelText)
		{
			Label.SetText(labelText);
			Label.SetBounds(
				Label.Bounds.CopyAtPosition(
					Panel.Bounds.GetHandlePosition(BoxHandle.Center), 
					BoxHandle.Center)
			);
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

		public Panel Panel {get; set;}
		public Label Label {get; set;}
	}
}

#pragma warning restore 1591