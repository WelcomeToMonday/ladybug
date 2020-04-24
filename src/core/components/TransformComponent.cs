using Microsoft.Xna.Framework;

using Ladybug.ECS;

namespace Ladybug.Core.Components
{
	public class TransformComponent : Component
	{
		public enum TransformHandle {TopRight, TopLeft, BottomRight, BottomLeft, Center}
		
		public Rectangle Bounds { get; private set; } = new Rectangle(10, 10, 20, 20);
		
		public float Rotation { get; private set; } = 0.0f;
		
		public float Scale { get; private set; } = 1.0f;
		
		public void Move(Vector2 newPos)
		{
			SetPosition(new Vector2((int)(newPos.X + Bounds.X),(int)(newPos.Y + Bounds.Y)));
		}

		public void Move(int xMove, int yMove)
		{
			Move(new Vector2(xMove,yMove));
		}
		
		public void SetBounds(Rectangle rectangle)
		{
			Bounds = rectangle;
		}

		public void SetPosition(Vector2 newPos, TransformHandle handle = TransformHandle.TopLeft)
		{
			int x, y;
			switch (handle)
			{
				default:
				case TransformHandle.TopLeft:
					x = (int)(newPos.X);
					y = (int)(newPos.Y);
					break;
				case TransformHandle.TopRight:
					x = (int)(newPos.X + Bounds.Width);
					y = (int)(newPos.Y);
					break;
				case TransformHandle.BottomLeft:
					x = (int)(newPos.X);
					y = (int)(newPos.Y + Bounds.Height);
					break;
				case TransformHandle.BottomRight:
					x = (int)(newPos.X + Bounds.Width);
					y = (int)(newPos.Y + Bounds.Height);
					break;
				case TransformHandle.Center:
					x = (int)(newPos.X + (Bounds.Width / 2));
					y = (int)(newPos.Y + (Bounds.Height / 2));
					break;
			}
			Bounds = new Rectangle(x,y,Bounds.Width,Bounds.Height);
		}

		public void RotateBy(float newRot)
		{
			Rotation += newRot;
		}

		public void SetRotation(float newRot)
		{
			Rotation = newRot;
		}

		public void ScaleBy(float newScale)
		{
			Scale += newScale;
		}

		public void SetScale(float newScale)
		{
			Scale = newScale;
		}

		public override void WriteXml(System.Xml.XmlWriter writer)
		{
			writer.WriteStartElement(ToString());
			
			// Bounds
			writer.WriteAttributeString("x", $"{Bounds.X}");
			writer.WriteAttributeString("y", $"{Bounds.Y}");
			writer.WriteAttributeString("width", $"{Bounds.Width}");
			writer.WriteAttributeString("height", $"{Bounds.Height}");
			
			// Rotation/Scale
			writer.WriteAttributeString("rotation",$"{Rotation}");
			writer.WriteAttributeString("scale",$"{Scale}");
			
			writer.WriteEndElement();
		}
		
		public override void ReadXml(System.Xml.XmlReader reader)
		{
			int x = 0;
			int y = 0;
			int width = 0;
			int height = 0;

			float rotation = 0.0f;
			float scale = 1.0f;

			while (reader.Read())
			{
				reader.MoveToAttribute("x");
				int.TryParse(reader.Value, out x);

				reader.MoveToAttribute("y");
				int.TryParse(reader.Value, out y);

				reader.MoveToAttribute("width");
				int.TryParse(reader.Value, out width);

				reader.MoveToAttribute("height");
				int.TryParse(reader.Value, out height);

				reader.MoveToAttribute("rotation");
				float.TryParse(reader.Value, out rotation);

				reader.MoveToAttribute("scale");
				float.TryParse(reader.Value, out scale);

				reader.MoveToElement();
			}
			
			Bounds = new Rectangle(x, y, width, height);
			Rotation = rotation;
			Scale = scale;
			
		}
	}
}