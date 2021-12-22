using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Supported event types
    /// </summary>
    public enum PlatformEventType
    {
        None = 0,
        WindowClose, WindowResize, WindowFocus, WindowLostFocus, WindowMoved,
        KeyPressed, KeyReleased, KeyChar,
        MouseButtonPressed, MouseButtonReleased, MouseMoved, MouseScrolled,
        FileDrop
    }
}
