using Microsoft.Xna.Framework;

using Ladybug;

public class Transform
{
	public Rectangle Bounds { get; private set; } = new Rectangle();

	public float Rotation { get; private set; } = 0.0f;

	public float Scale { get; private set; } = 1.0f;

	public void Move(Vector2 newPos)
	{
		SetPosition(new Vector2((int)(newPos.X + Bounds.X), (int)(newPos.Y + Bounds.Y)));
	}

	public void Move(int xMove, int yMove)
	{
		Move(new Vector2(xMove, yMove));
	}

	public void SetBounds(Rectangle rectangle)
	{
		Bounds = rectangle;
	}

	public void SetPosition(Vector2 newPos, BoxHandle handle = BoxHandle.TopLeft)
	{
		int x, y;
		switch (handle)
		{
			default:
			case BoxHandle.TopLeft:
				x = (int)(newPos.X);
				y = (int)(newPos.Y);
				break;
			case BoxHandle.TopRight:
				x = (int)(newPos.X + Bounds.Width);
				y = (int)(newPos.Y);
				break;
			case BoxHandle.BottomLeft:
				x = (int)(newPos.X);
				y = (int)(newPos.Y + Bounds.Height);
				break;
			case BoxHandle.BottomRight:
				x = (int)(newPos.X + Bounds.Width);
				y = (int)(newPos.Y + Bounds.Height);
				break;
			case BoxHandle.Center:
				x = (int)(newPos.X + (Bounds.Width / 2));
				y = (int)(newPos.Y + (Bounds.Height / 2));
				break;
		}
		Bounds = new Rectangle(x, y, Bounds.Width, Bounds.Height);
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
}
