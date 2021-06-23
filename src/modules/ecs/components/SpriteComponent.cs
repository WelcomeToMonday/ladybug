using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.ECS.Components
{
	public class SpriteComponentSystem : ComponentSystem<SpriteComponent> {}

	public class SpriteComponent : Component
	{
		public SpriteComponent()
		{
			OnInitialize(Initialize);
			OnUpdate(Update);
			OnDraw(Draw);
		}

		public AnimatedSprite Sprite{get; private set;}

		public Color Color {get; set;} = Color.White;
		
		public void SetSprite(AnimatedSprite s) => Sprite = s;

		public void AddAnimation(
			string animationName, 
			Texture2D sourceTexture,
			int rows,
			int columns,
			int animationSpeed,
			int startFrame,
			int? endFrame = null,
			bool setDefault = false
			)
		{
			var sequence = new AnimationSequence(sourceTexture,rows,columns,startFrame,endFrame);
			sequence.Speed = animationSpeed;
			if (Sprite == null) Sprite = new AnimatedSprite();
			Sprite.AddAnimation(animationName,sequence,setDefault);
		}

		public AnimationSequence GetAnimation(string animationName) => Sprite.GetAnimation(animationName);

		public AnimationSequence GetAnimation() => Sprite.CurrentAnimation;

		public void SetAnimation(string animationName)
		{
			Sprite.SetAnimation(animationName);
		}
		
		private void Initialize()
		{
			if (Sprite == null) SetSprite(new AnimatedSprite());
		}
		
		private void Update(GameTime gameTime)
		{
			Sprite?.CurrentAnimation.Play();
		}
		
		private void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (Sprite != null && Visible)
			{
				var frame = Sprite.GetCurrentFrame;
				spriteBatch.Draw(
					frame.Texture,
					Entity.Transform.Bounds,
					frame.Frame,
					Color
				);
			}
		}
	}
}