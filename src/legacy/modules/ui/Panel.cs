#pragma warning disable 1591 // Hide XMLdoc warnings.

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Legacy.UI
{
	[Obsolete("Ladybug Legacy UI is Obsolete. Please upgrade to 2.0 API ASAP")]
	public class Panel : Control
	{
		public Panel(Control parentControl = null, string name = "") : base(parentControl, name)
		{

		}

		public override void Enable()
		{
			base.Enable();
			if (Controls != null && Controls.Count > 0)
			{
				foreach (var c in Controls)
				{
					c.Enable();
				}
			}
		}

		public override void Disable()
		{
			base.Disable();
			if (Controls != null && Controls.Count > 0)
			{
				foreach (var c in Controls)
				{
					c.Disable();
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (Visible)
			{
				if (BackgroundImage != null)
				{
					spriteBatch.Draw(
						BackgroundImage,
						Bounds,
						null,
						Color.White
					);
				}
				base.Draw(spriteBatch);
			}
		}
	}
}

#pragma warning restore 1591