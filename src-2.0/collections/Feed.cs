using System;
using System.Linq;
using System.Collections.Generic;

namespace Ladybug
{
	public class Feed<T>
	{
		public int MaxLines { get; private set; }
		private List<T> _lines = new List<T>();

		private int _lineOffset = 0;

		public Feed(int maxLines)
		{
			MaxLines = maxLines;
		}

		public Feed(List<T> list, int maxLines, int startOffset = 0)
		{
			_lines = list;
			MaxLines = maxLines;
			ScrollTo(startOffset);
		}

		public int MaxOffset { get => Math.Max(0, _lines.Count - MaxLines); }

		public List<T> View { get; private set; } = new List<T>();

		public void SetMaxLines(int maxLines)
		{
			Reset();
			MaxLines = maxLines;
			ScrollTo(_lineOffset);
		}

		public void ResetPosition() => _lineOffset = 0;

		public void Reset()
		{
			ResetPosition();
			_lines.Clear();
		}

		public void Scroll(int offset)
		{
			ScrollTo(_lineOffset + offset);
		}

		public void ScrollTo(int offset)
		{
			if (offset < 0)
			{
				offset = 0;
			}

			if (_lines.Count > MaxLines)
			{
				_lineOffset = Math.Min(offset, MaxOffset);
			}
			else
			{
				offset = 0;
			}

			RebuildView();
		}

		public void ScrollToEnd() => ScrollTo(_lines.Count - 1);

		public void AddLine(T item, bool preventScroll = false)
		{
			_lines.Add(item);

			if (!preventScroll && _lines.Count > MaxLines)
			{
				ScrollTo(_lines.Count - MaxLines);
			}
			else
			{
				RebuildView();
			}
		}

		private void RebuildView()
		{
			View.Clear();

			var lineCount = Math.Min(MaxLines, _lines.Count);

			for (var i = _lineOffset; i < _lineOffset + lineCount; i++)
			{
				View.Add(_lines[i]);
			}
		}
	}
}