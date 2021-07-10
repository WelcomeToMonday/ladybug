using System;
using System.Collections.Generic;

namespace Ladybug.Beta.UI
{
	public class UI : Panel
	{
		private List<Control> _controls = new List<Control>();

		private List<Control> _controlsByPriority = new List<Control>();

		private bool _sortRequired = false;

		public UI(Scene scene) : base()
		{
			Scene = scene;
			Controls = _controls.AsReadOnly();
		}

		public IList<Control> Controls { get; private set; }
		
		public Scene Scene { get; private set; }
		
		public VRC VRC { get; set; }

		public void RequestSort()
		{
			_sortRequired = true;
		}

		public void Update()
		{
			if (_sortRequired)
			{
				_controls.Sort((Control x, Control y) => 
				{
					var res = 0;
					if (x.ZIndex > y.ZIndex) res = 1;
					if (x.ZIndex < y.ZIndex) res = 1;
					return res;
				});
			}
		}
	}
}