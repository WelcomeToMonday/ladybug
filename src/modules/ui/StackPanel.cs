using Microsoft.Xna.Framework;

namespace Ladybug.UI
{
	public class StackPanel : Panel
	{
		/// <summary>
		/// Creates a new StackPanel
		/// </summary>
		/// <returns></returns>
		public StackPanel() : base()
		{
			VerticalAlignment = VerticalAlignment.Top;
			HorizontalAlignment = HorizontalAlignment.Center;
		}

		/// <summary>
		/// Space, in pixels, between the StackPanel's contents
		/// </summary>
		/// <value></value>
		public int Separation
		{
			get => m_Separation;
			set
			{
				if (m_Separation != value)
				{
					m_Separation = value;
					UpdateLayout();
				}
			}
		}
		private int m_Separation = 0;

		public Margin Margin 
		{
			get => m_Margin;
			set
			{
				if (m_Margin != value)
				{
					m_Margin = value;
					UpdateLayout();
				}
			}
		}
		private Margin m_Margin = Margin.Zero;

		/// <summary>
		/// Orientation of the StackPanel's elements
		/// </summary>
		public Orientation Orientation = Orientation.Vertical;

		private void UpdateLayout()
		{
			SetBounds(Bounds);
		}

		private Vector2 GetAnchorStartPosition()
		{
			int x = 0;
			int y = 0;

			switch (VerticalAlignment)
			{
				case VerticalAlignment.Bottom: // temp
				case VerticalAlignment.Top:
					y = Bounds.Top + Margin.Top;
					break;
				case VerticalAlignment.Center:
					y = (int)Bounds.GetHandlePosition(BoxHandle.Center).Y;
					break;
			}

			switch (HorizontalAlignment)
			{
				case HorizontalAlignment.Left:
					x = Bounds.Left + Margin.Left;
					break;
				case HorizontalAlignment.Center:
					x = (int)Bounds.GetHandlePosition(BoxHandle.Center).X;
					break;
				case HorizontalAlignment.Right:
					x = Bounds.Right - Margin.Right;
					break;
			}

			return new Vector2(x, y);
		}

		/// <summary>
		/// Gets the BoxHandle used when determining the anchor for placing the next
		/// child control in UpdateLayout
		/// </summary>
		private BoxHandle GetBoxHandle()
		{
			var res = BoxHandle.Center;
			// todo: orientation check
			switch (HorizontalAlignment)
			{
				case HorizontalAlignment.Left:
					res = BoxHandle.TopLeft;
					break;
				case HorizontalAlignment.Right:
					res = BoxHandle.TopRight;
					break;
				case HorizontalAlignment.Center:
					res = BoxHandle.TopCenter;
					break;
			}

			return res;
		}

		/// <summary>
		/// Called when the StackPanel's bounds are updated
		/// </summary>
		/// <param name="oldBounds"></param>
		/// <param name="newBounds"></param>
		protected override void UpdateBounds(Rectangle oldBounds, Rectangle newBounds)
		{
			var nextPos = GetAnchorStartPosition();
			var handle = GetBoxHandle();
			var dirMod = VerticalAlignment == VerticalAlignment.Bottom ? -1 : 1;

			for (var i = 0; i < Controls.Count; i++)
			{
				var control = Controls[i];

				control.SetBounds(control.Bounds.CopyAtPosition(nextPos, handle));

				nextPos = control.Bounds.GetHandlePosition(GetBoxHandle()) + new Vector2(0, control.Bounds.Height + Separation);
			}
		}
	}
}