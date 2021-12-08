using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Base class for all platform events
    /// </summary>
    public abstract class PlatformEvent
    {
        public PlatformEvent()
        {
            m_Handled = false;
        }

        /// <summary>
        /// Returns the event type
        /// </summary>
        public abstract PlatformEventType Type { get; }

        /// <summary>
        /// Returns the event category
        /// </summary>
        public abstract PlatformEventCategory Category { get; }

        /// <summary>
        /// Returns this event as string representation
        /// </summary>
        public abstract string AsString { get; }

        /// <summary>
        /// Returns whether this event should not be consumed
        /// </summary>
        public bool IsHandled
        {
            get
            {
                return m_Handled;
            }
           
        }

        /// <summary>
        /// Marks this evnet handled
        /// </summary>
        public void MarkHandled()
        {
            m_Handled = true;
        }

        private bool m_Handled;
    }
}
