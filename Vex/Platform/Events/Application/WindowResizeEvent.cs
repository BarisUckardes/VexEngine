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
    public sealed class WindowResizeEvent : Event
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

        public override EventType Type
        {
            get
            {
                return EventType.WindowResize;
            }
        }

        public override EventCategory Category
        {
            get
            {
                return EventCategory.CategoryApplication;
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
