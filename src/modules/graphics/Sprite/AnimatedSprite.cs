using System.Collections.Generic;

namespace Ladybug.Graphics
{
	public class AnimatedSprite
	{
		private Dictionary<string, AnimationSequence> _animationList = new Dictionary<string, AnimationSequence>();
		private string _currentAnimationName;

		public AnimatedSprite()
		{

		}

		public AnimatedSprite(AnimationSequence defaultAnimation)
		{
			AddAnimation("default", defaultAnimation, true);
			SetAnimation("default");
		}

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

		public Sprite GetCurrentFrame => CurrentAnimation?.GetCurrentFrame();//_animationList[_currentAnimationName].GetCurrentFrame();

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

		public void SetAnimation(string animationName)
		{
			if (_animationList.ContainsKey(animationName)) _currentAnimationName = animationName;
		}

		public AnimationSequence GetAnimation() => CurrentAnimation;

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