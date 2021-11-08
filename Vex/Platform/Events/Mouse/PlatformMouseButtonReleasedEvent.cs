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
    public sealed class PlatformMouseButtonReleasedEvent : PlatformMouseButtonEvent
    {
        public PlatformMouseButtonReleasedEvent(int button) : base(button)
        {

        }
        public override PlatformEventType Type
        {
            get
            {
                return PlatformEventType.MouseButtonReleased;
            }
        }

        public override PlatformEventCategory Category
        {
            get
            {
                return PlatformEventCategory.CategoryMouseButton;
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
