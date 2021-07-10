using System;
using System.Collections.Generic;

namespace Ladybug.Beta.UI
{
	public abstract class Control
	{
		private int _zIndex = 0;
		private List<Control> _children = new List<Control>();

		internal Control() { }
		/*
		public Control(Control parentControl) //public? internal?
		{
			Parent = parentControl;
			UI = Parent.UI;
			Children = _children.AsReadOnly();
		}
		*/
		public Control AddControl<T>(string name = null) where T : Control, new()
		{
			T control = new T()
			{
				Parent = this,
				UI = this.UI
			};
			if (name != null || name != string.Empty)
			{
				control.Name = name;
			}
			return this;
		}

		public string Name { get; set; }

		public Control Parent { get; private set; }

		public IList<Control> Children { get; private set; }

		public UI UI { get; private set; }

		public int ZIndex
		{
			get => _zIndex;
			set
			{
				if (value != _zIndex)
				{
					_zIndex = value;
					UI.RequestSort();
				}
			}
		}
	}
}