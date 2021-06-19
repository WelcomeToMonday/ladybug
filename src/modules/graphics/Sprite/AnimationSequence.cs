using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Graphics
{
	public class AnimationSequence
	{
		private int m_currentFrameIndex;
		private int m_speed;
		private int _delayTimer;

		private Texture2D _sourceTexture;		
		private Sprite[] _frames;

		public AnimationSequence(Texture2D sourceTexture, int rows, int columns, int start = 0, int? end = null)
		{
			_sourceTexture = sourceTexture;
			if (end == null) end = rows * columns;
			BuildFrames(rows, columns, start,(int)end);
		}

		public int Length
		{
			get => _frames.Length;
		}

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

		public int CurrentFrameIndex
		{
			get => m_currentFrameIndex;
			set
			{
				m_currentFrameIndex = (value > (Length - 1)) ? 0 : value;
			}
		}

		public Sprite GetCurrentFrame() => _frames[CurrentFrameIndex];

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
		
		private void BuildFrames(int rows, int columns, int startFrame, int endFrame)
		{
			int frameCount = Math.Abs(endFrame - startFrame) + 1;
			int width = (int)(_sourceTexture.Width / columns);
			int height = (int)(_sourceTexture.Height / rows);
			_frames = new Sprite[frameCount];
			for (var i = 0; i < frameCount; i++)
			{
				int row = (int)(startFrame + i) / columns;
				int column = (startFrame + i) % columns;
				Rectangle frame = new Rectangle(
					(int)(width * column), 
					(int)(height * row), 
					(int)(width), 
					(int)(height)
					);
				_frames[i] = new Sprite(_sourceTexture,frame);
			}
		}
	}
}