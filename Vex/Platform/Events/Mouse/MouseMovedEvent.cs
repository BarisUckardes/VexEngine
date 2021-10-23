using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Mouse moved event class for consuming
    /// </summary>
    public sealed class MouseMovedEvent : Event
    {
        public MouseMovedEvent(float x,float y)
        {
            m_X = x;
            m_Y = y;

        }
        public override EventType Type
        {
            get
            {
                return EventType.MouseMoved;
            }
        }

        public override EventCategory Category
        {
            get
            {
                return EventCategory.CategoryMouse;
            }
        }

        public override string AsString
        {
            get
            {
                return "Mouse Moved: " + m_X +":" + m_Y;
            }
        }

        /// <summary>
        /// Current horizontal mouse position in pixels
        /// </summary>
        public float X
        {
            get
            {
                return m_X;
            }
        }

        /// <summary>
        /// Current vertical mouse position in pixels
        /// </summary>
        public float Y
        {
            get
            {
                return m_Y;
            }
        }
        private float m_X;
        private float m_Y;
    }
}
