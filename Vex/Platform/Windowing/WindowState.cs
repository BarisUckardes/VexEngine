using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Supported window state
    /// </summary>
    public enum WindowState
    {
        Normal = 0,
        Minimized = 1,
        Maximized = 2,
        Fullscreen = 3
    }

    /// <summary>
    /// Helper class for converting window states to native window states
    /// </summary>
    public static class WindowStateUtils
    {
        /// <summary>
        /// Convert window state to native state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static OpenTK.Windowing.Common.WindowState GetNativeWindowState(WindowState state)
        {
            switch (state)
            {
                case WindowState.Normal:
                    return OpenTK.Windowing.Common.WindowState.Normal;
                    break;
                case WindowState.Minimized:
                    return OpenTK.Windowing.Common.WindowState.Minimized;
                    break;
                case WindowState.Maximized:
                    return OpenTK.Windowing.Common.WindowState.Maximized;
                    break;
                case WindowState.Fullscreen:
                    return OpenTK.Windowing.Common.WindowState.Fullscreen;
                    break;
                default:
                    return OpenTK.Windowing.Common.WindowState.Normal;
                    break;
            }
        }


    }
}
