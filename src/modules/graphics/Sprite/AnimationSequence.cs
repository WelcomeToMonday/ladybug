using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Graphics
{
	/// <summary>
	/// Ladybug AnimationSequence
	/// </summary>
	public class AnimationSequence
	{
		private int m_currentFrameIndex;
		private int m_speed;
		private int _delayTimer;
		
		private Sprite[] _frames;

		/// <summary>
		/// Creates a new AnimationSequence
		/// </summary>
		/// <param name="sourceAtlas"></param>
		/// <param name="startFrame"></param>
		/// <param name="endFrame"></param>
		/// <returns></returns>
		public AnimationSequence(SpriteAtlas sourceAtlas, int startFrame, int endFrame) => BuildFrames(sourceAtlas, startFrame, endFrame);

		/// <summary>
		/// Creates a new AnimationSequence
		/// </summary>
		/// <param name="sprites"></param>
		/// <returns></returns>
		public AnimationSequence(params Sprite[] sprites) => BuildFrames(sprites);

		/// <summary>
		/// Length of the animation in frames
		/// </summary>
		/// <value></value>
		public int Length
		{
			get => _frames.Length;
		}

		/// <summary>
		/// Speed of the animation, measured by the delay between frames
		/// </summary>
		/// <value></value>
		/// <remarks>A lower value results in a faster animation</remarks>
		public int Speed
		{
			get => m_speed;
			set
			{
				if (value >= 0)
				{
					m_speed = value;
					_delayTimer = value;
				}
			}
		}

		/// <summary>
		/// Index of the current frame
		/// </summary>
		/// <value></value>
		public int CurrentFrameIndex
		{
			get => m_currentFrameIndex;
			set
			{
				m_currentFrameIndex = (value > (Length - 1)) ? 0 : value;
			}
		}

		/// <summary>
		/// Gets the Sprite for the current animation frame
		/// </summary>
		/// <returns></returns>
		public Sprite GetCurrentFrame() => _frames[CurrentFrameIndex];

		/// <summary>
		/// Plays the AnimationSequence
		/// </summary>
		public void Play()
		{
			if (_delayTimer <= 0)
			{
				_delayTimer = Speed;
				NextFrame();
			}
			_delayTimer--;
		}

		private void NextFrame()
		{
			_delayTimer = Speed;
			CurrentFrameIndex++;
		}
		
		private void BuildFrames(SpriteAtlas sourceAtlas, int startFrame, int endFrame)
		{
			_frames = new Sprite[endFrame - startFrame];
			for (var i = 0; i < endFrame - startFrame; i++)
			{
				_frames[i] = sourceAtlas[startFrame + i];
			}
		}

		private void BuildFrames(Sprite[] sprites) => _frames = sprites;
	}
}