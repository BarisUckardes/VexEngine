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
    public sealed class WindowCloseEvent : PlatformEvent
    {
        public override PlatformEventType Type
        {
            get
            {
                return PlatformEventType.WindowClose;
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
                return "Window Closed";
            }
        }
    }
}
