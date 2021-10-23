using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Mouse button released event for consuming
    /// </summary>
    public sealed class MouseButtonReleasedEvent : MouseButtonEvent
    {
        public MouseButtonReleasedEvent(int button) : base(button)
        {

        }
        public override EventType Type
        {
            get
            {
                return EventType.MouseButtonReleased;
            }
        }

        public override EventCategory Category
        {
            get
            {
                return EventCategory.CategoryMouseButton;
            }
        }

        public override string AsString
        {
            get
            {
                return "Mouse Button Released: Button [" + MouseButton + "]";
            }
        }
    }
}
