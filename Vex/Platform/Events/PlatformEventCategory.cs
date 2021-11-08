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
		CategoryNone = 0,
		CategoryApplication,
		CategoryInput,
		CategoryKeyboard,
		CategoryMouse,
		CategoryMouseButton
	}
}
