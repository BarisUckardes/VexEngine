using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Base mouse button event
    /// </summary>
    public abstract class PlatformMouseButtonEvent : PlatformEvent
    {
        /// <summary>
        /// Returns the button
        /// </summary>
        public int MouseButton
        {
            get
            {
                return m_Button;
            }
        }

        protected PlatformMouseButtonEvent(int button)
        {
            m_Button = button;
        }

        private int m_Button;
    }
}
