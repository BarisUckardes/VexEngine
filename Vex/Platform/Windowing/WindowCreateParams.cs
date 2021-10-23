using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Reprensets the window's main parameters for size,offset and state etc.
    /// </summary>
    public readonly struct WindowCreateParams
    {
        public WindowCreateParams(WindowState state, string title, int offsetX, int offsetY, int width, int height, bool startsFocused)
        {
            State = state;
            OffsetX = offsetX;
            OffsetY = offsetY;
            Width = width;
            Height = height;
            StartsFocused = startsFocused;
        }

        /// <summary>
        /// The border state of the window
        /// </summary>
        public readonly WindowState State;
        /// <summary>
        /// Horizontal offset fVexm the origin
        /// </summary>
        public readonly int OffsetX;
        /// <summary>
        /// Vertical offset fVexmt the origin
        /// </summary>
        public readonly int OffsetY;
        /// <summary>
        /// Window width
        /// </summary>
        public readonly int Width;
        /// <summary>
        /// Window height
        /// </summary>
        public readonly int Height;
        /// <summary>
        /// Whether window starts with focuced or not
        /// </summary>
        public readonly bool StartsFocused;

       
    }
}
