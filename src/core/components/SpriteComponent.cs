using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.ECS;
using Ladybug.Graphics;

namespace Ladybug.Core.Components
{
	public class SpriteComponent : Component, IDrawableComponent
	{
		private TransformComponent _entityTransform;

		private int _drawPriority;

		public AnimatedSprite Sprite{get; private set;}

		public int DrawPriority {get => _drawPriority;}

		public bool Visible {get => Active && Entity.Active;}

		public Color Color {get; set;} = Color.White;
		
		public void SetSprite(AnimatedSprite s) => Sprite = s;

		public void SetDrawPriority(int priority)
		{
			_drawPriority = priority;
			System.SortDrawableComponents();
		}

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
		
		public override void Initialize()
		{
			_entityTransform = Entity.GetComponent<TransformComponent>();
			if (Sprite == null) SetSprite(new AnimatedSprite());
		}
		
		public override void Update(GameTime gameTime)
		{
			Sprite?.CurrentAnimation.Play();
		}
		
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (Sprite != null && Visible)
			{
				var frame = Sprite.GetCurrentFrame;
				spriteBatch.Draw(
					frame.Texture,
					_entityTransform.Bounds,
					frame.Frame,
					Color
				);
			}
		}
	}
}