using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Input;
namespace Vex.Platform
{
    /// <summary>
    /// Base key event type for all key events
    /// </summary>
    public abstract class PlatformKeyEvent : PlatformEvent
    {
        /// <summary>
        /// Returns the key code of this event
        /// </summary>
        public Keys KeyCode
        {
            get
            {
                return m_KeyCode;
            }
        }

        /// <summary>
        /// protected constructor for derived classes to call
        /// </summary>
        /// <param name="keyCode"></param>
        protected PlatformKeyEvent(Keys keyCode)
        {
            m_KeyCode = keyCode;
        }

        private Keys m_KeyCode;
    }
}
