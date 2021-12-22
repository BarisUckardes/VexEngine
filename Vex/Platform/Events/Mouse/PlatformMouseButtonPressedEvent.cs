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
    public sealed class PlatformMouseButtonPressedEvent : PlatformMouseButtonEvent
    {

        public PlatformMouseButtonPressedEvent(int button) : base(button)
        {

        }
        public override PlatformEventType Type
        {
            get
            {
                return PlatformEventType.MouseButtonPressed;
            }
        }

        public override PlatformEventCategory Category
        {
            get
            {
                return PlatformEventCategory.MouseButton;
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
