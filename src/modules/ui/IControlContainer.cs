using System;
using System.Collections.Generic;

namespace Ladybug.UI
{
	/// <summary>
	/// Interface for objects which contain UI Controls
	/// </summary>
	public interface IControlContainer
	{
		/// <summary>
		/// UI managing this container
		/// </summary>
		/// <value></value>
		UI UI { get; }
		/// <summary>
		/// Controls contained within this container
		/// </summary>
		/// <value></value>
		IList<Control> Controls { get; }
		
		/// <summary>
		/// Z-Index depth of control
		/// </summary>
		/// <value></value>
		int ZIndex { get; }
	}
}