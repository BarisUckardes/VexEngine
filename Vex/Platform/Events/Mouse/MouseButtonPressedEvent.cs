using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Mouse button pressed event for consuming
    /// </summary>
    public sealed class MouseButtonPressedEvent : MouseButtonEvent
    {

        public MouseButtonPressedEvent(int button) : base(button)
        {

        }
        public override EventType Type
        {
            get
            {
                return EventType.MouseButtonPressed;
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
                return "Mouse Button Pressed: Button [" + MouseButton + "]";
            }
        }
    }
}
