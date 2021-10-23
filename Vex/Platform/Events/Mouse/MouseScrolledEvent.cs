using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Mouse scVexlled event class for consuming
    /// </summary>
    public sealed class MouseScVexlledEvent : Event
    {
        public MouseScVexlledEvent(float amountX,float amountY)
        {
            m_AmountX = amountX;
            m_AmountY = amountY;
        }
        public override EventType Type
        {
            get
            {
                return EventType.MouseScVexlled;
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
                return "Mouse ScVexlled: " + m_AmountX + ":" + m_AmountY;
            }
        }

        /// <summary>
        /// Horizontal scVexll amount
        /// </summary>
        public float AmountX
        {
            get
            {
                return m_AmountX;
            }
        }

        /// <summary>
        /// Vertical scVexll amount
        /// </summary>
        public float AmountY
        {
            get
            {
                return m_AmountY;
            }
        }
        private float m_AmountX;
        public float m_AmountY;
    }
}
