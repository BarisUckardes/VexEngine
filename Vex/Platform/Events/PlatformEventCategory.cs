using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
	/// <summary>
	/// Supported event categories
	/// </summary>
    public enum PlatformEventCategory
    {
		None = 0,
		Application,
		Input,
		Keyboard,
		Mouse,
		MouseButton,
		File
	}
}
