using System.Collections.Generic;

namespace Ladybug.Graphics
{
	/// <summary>
	/// Represents a sprite with defined animation sequences
	/// </summary>
	public class AnimatedSprite
	{
		private Dictionary<string, AnimationSequence> _animationList = new Dictionary<string, AnimationSequence>();
		private string _currentAnimationName;

		/// <summary>
		/// Create an AnimatedSprite
		/// </summary>
		public AnimatedSprite()
		{

		}

		/// <summary>
		/// Create an AnimatedSprite with a given default animation sequence
		/// </summary>
		/// <param name="defaultAnimation"></param>
		public AnimatedSprite(AnimationSequence defaultAnimation)
		{
			AddAnimation("default", defaultAnimation, true);
			SetAnimation("default");
		}

		/// <summary>
		/// This AnimatedSprite's current animation sequence, if set
		/// </summary>
		/// <value></value>
		public AnimationSequence CurrentAnimation
		{
			get
			{
				AnimationSequence res = null;
				if (_currentAnimationName != null && _animationList.TryGetValue(_currentAnimationName, out AnimationSequence sequence))
				{
					res = sequence;
				}
				return res;
			}
		}

		//todo: this should probably just be renamed to "CurrentFrame". The "Get" prefix is more of a method thing
		/// <summary>
		/// The AnimatedSprite's current frame
		/// </summary>
		/// <returns></returns>
		public Sprite GetCurrentFrame => CurrentAnimation?.GetCurrentFrame();//_animationList[_currentAnimationName].GetCurrentFrame();

		/// <summary>
		/// Adds an animation sequence to this AnimatedSprite's defined sequences
		/// </summary>
		/// <param name="animationName"></param>
		/// <param name="sequence"></param>
		/// <param name="makeActive"></param>
		public void AddAnimation(string animationName, AnimationSequence sequence, bool makeActive = false)
		{
			if (!_animationList.ContainsKey(animationName))
			{
				_animationList.Add(animationName, sequence);
			}
			else
			{
				_animationList[animationName] = sequence;
			}

			if (makeActive || (_animationList.Count == 1 && _animationList.ContainsKey(animationName)))
			{
				SetAnimation(animationName);
			}
		}

		/// <summary>
		/// Sets the AnimatedSprite to a specific AnimationSequence
		/// </summary>
		/// <param name="animationName"></param>
		public void SetAnimation(string animationName)
		{
			if (_animationList.ContainsKey(animationName)) _currentAnimationName = animationName;
		}

		/// <summary>
		/// Get the AnimatedSprite's current animation sequence
		/// </summary>
		/// <returns></returns>
		public AnimationSequence GetAnimation() => CurrentAnimation;

		/// <summary>
		/// Get one of the AnimatedSprite's animation sequences by name
		/// </summary>
		/// <param name="animationName"></param>
		/// <returns></returns>
		public AnimationSequence GetAnimation(string animationName)
		{
			AnimationSequence sequence = null;
			if (_animationList.ContainsKey(animationName))
			{
				sequence = _animationList[animationName];
			}
			return sequence;
		}
	}
}