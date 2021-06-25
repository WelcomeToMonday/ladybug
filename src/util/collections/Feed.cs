using System;
using System.Collections.Generic;

namespace Ladybug
{
	/// <summary>
	/// Represents a List with a given
	/// range of elements that are currently
	/// accessible
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Feed<T>
	{
		/// <summary>
		/// Size of the <see cref="View"/>
		/// </summary>
		/// <value></value>
		public int MaxLines { get; private set; }
		private List<T> _lines = new List<T>();

		private int _lineOffset = 0;

		/// <summary>
		/// Creates a new Feed
		/// </summary>
		/// <param name="maxLines">
		/// Size of the <see cref="View"/>
		/// </param>
		public Feed(int maxLines)
		{
			MaxLines = maxLines;
		}

		/// <summary>
		/// Creates a new Feed
		/// </summary>
		/// <param name="list">Source list</param>
		/// <param name="maxLines">
		/// Size of the <see cref="View"/> 
		/// </param>
		/// <param name="startOffset">
		/// Initial <see cref="View"/> position
		/// </param>
		public Feed(List<T> list, int maxLines, int startOffset = 0)
		{
			_lines = list;
			MaxLines = maxLines;
			ScrollTo(startOffset);
		}

		/// <summary>
		/// Maximum possible offset value considering total
		/// item count and size of the <see cref="View"/>
		/// </summary>
		/// <returns></returns>
		public int MaxOffset { get => Math.Max(0, _lines.Count - MaxLines); }

		/// <summary>
		/// List representing the current accessible items in the
		/// Feed
		/// </summary>
		/// <value></value>
		public IList<T> View {get; private set;}
		private List<T> _view = new List<T>();

		/// <summary>
		/// Sets the maximum size of the <see cref="View"/>
		/// </summary>
		/// <param name="maxLines"></param>
		public void SetMaxLines(int maxLines)
		{
			Reset();
			View = _view.AsReadOnly();
			MaxLines = maxLines;
			ScrollTo(_lineOffset);
		}

		/// <summary>
		/// Resets the <see cref="View"/> to
		/// the beginning of the Feed
		/// </summary>
		public void ResetPosition() => _lineOffset = 0;

		/// <summary>
		/// Resets the Feed, clearing all items
		/// and returning the <see cref="View"/>
		/// position to the beginning of the Feed
		/// </summary>
		public void Reset()
		{
			ResetPosition();
			_lines.Clear();
		}

		/// <summary>
		/// Moves the <see cref="View"/> relative
		/// to its current position
		/// </summary>
		/// <param name="offset">
		/// Direction and distance to move the <see cref="View"/>
		/// </param>
		public void Scroll(int offset)
		{
			ScrollTo(_lineOffset + offset);
		}

		/// <summary>
		/// Moves the <see cref="View"/> to
		/// the specified position
		/// </summary>
		/// <param name="offset"></param>
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

		/// <summary>
		/// Moves the <see cref="View"/> to the
		/// furthest possible position
		/// </summary>
		public void ScrollToEnd() => ScrollTo(_lines.Count - 1);

		/// <summary>
		/// Adds an item to the Feed
		/// </summary>
		/// <param name="item"></param>
		/// <param name="preventScroll"></param>
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
			_view.Clear();

			var lineCount = Math.Min(MaxLines, _lines.Count);

			for (var i = _lineOffset; i < _lineOffset + lineCount; i++)
			{
				_view.Add(_lines[i]);
			}
		}
	}
}