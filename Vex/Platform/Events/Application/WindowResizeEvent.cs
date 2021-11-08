using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Window resized event for consuming
    /// </summary>
    public sealed class WindowResizeEvent : PlatformEvent
    {

        public WindowResizeEvent(uint width,uint height)
        {
            m_Width = width;
            m_Height = height;
        }

        /// <summary>
        /// Returns the current width in pixels
        /// </summary>
        public uint Width
        {
            get
            {
                return m_Width;
            }
        }

        /// <summary>
        /// Returns the current height in pixels
        /// </summary>
        public uint Height
        {
            get
            {
                return m_Height;
            }
        }

        public override PlatformEventType Type
        {
            get
            {
                return PlatformEventType.WindowResize;
            }
        }

        public override PlatformEventCategory Category
        {
            get
            {
                return PlatformEventCategory.CategoryApplication;
            }
        }

        public override string AsString
        {
            get
            {
                return "Window Resized: " + m_Width + ":" + m_Height;
            }
        }

        private uint m_Width;
        private uint m_Height;
    }
}
