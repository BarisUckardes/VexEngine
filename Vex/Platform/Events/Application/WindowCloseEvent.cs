using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Window closed event for consuming
    /// </summary>
    public sealed class WindowCloseEvent : Event
    {
        public override EventType Type
        {
            get
            {
                return EventType.WindowClose;
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
                return "Window Closed";
            }
        }
    }
}
