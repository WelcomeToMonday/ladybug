using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.ECS.Components
{
	/// <summary>
	/// ComponentSystem responsible for processing <see cref="Ladybug.ECS.Components.SpriteComponent"/> Components
	/// </summary>
	public class SpriteComponentSystem : ComponentSystem<SpriteComponent> {}

	/// <summary>
	/// Ladybug standard component for sprites
	/// </summary>
	public class SpriteComponent : Component
	{
		/// <summary>
		/// Creates a new SpriteComponent instance
		/// </summary>
		public SpriteComponent()
		{
			OnInitialize(Initialize);
			OnUpdate(Update);
			OnDraw(Draw);
		}

		/// <summary>
		/// <see cref="Ladybug.Graphics.AnimatedSprite"/> instance used
		/// by this SpriteComponent
		/// </summary>
		/// <value></value>
		public AnimatedSprite Sprite{get; private set;}

		/// <summary>
		/// Color filter applied to this SpriteComponent
		/// </summary>
		/// <value></value>
		public Color Color {get; set;} = Color.White;
		
		/// <summary>
		/// Sets the component's animated sprite
		/// </summary>
		/// <param name="sprite"></param>
		public void SetSprite(AnimatedSprite sprite) => Sprite = sprite;

		/// <summary>
		/// Adds a new Animation to this SpriteComponent
		/// </summary>
		/// <param name="animationName">Name of the new Animation</param>
		/// <param name="sourceTexture">Source texture</param>
		/// <param name="rows">Rows in the source texture</param>
		/// <param name="columns">Columns in the source texture</param>
		/// <param name="animationSpeed">Speed of the animation</param>
		/// <param name="startFrame">First frame of the animation</param>
		/// <param name="endFrame">Last frame of the animation</param>
		/// <param name="setDefault">
		/// Whether to set this animation as the SpriteComponent's default
		/// animation
		/// </param>
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

		/// <summary>
		/// Gets an <see cref="Ladybug.Graphics.AnimationSequence"/> from
		/// this SpriteComponent's <see cref="Ladybug.Graphics.AnimatedSprite"/>
		/// </summary>
		/// <param name="animationName">Name of the animation to retrieve</param>
		/// <returns></returns>
		public AnimationSequence GetAnimation(string animationName) => Sprite.GetAnimation(animationName);

		/// <summary>
		/// Gets this the current <see cref="Ladybug.Graphics.AnimationSequence"/> 
		/// of this SpriteComponent's <see cref="Ladybug.Graphics.AnimatedSprite"/>
		/// </summary>
		/// <returns></returns>
		public AnimationSequence GetAnimation() => Sprite.CurrentAnimation;

		/// <summary>
		/// Sets the SpriteComponent's current 
		/// <see cref="Ladybug.Graphics.AnimationSequence"/> 
		/// </summary>
		/// <param name="animationName"></param>
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