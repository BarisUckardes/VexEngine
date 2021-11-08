using Vex.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Key released event for consuming
    /// </summary>
    public sealed class PlatformKeyReleasedEvent : PlatformKeyEvent
    {
        public PlatformKeyReleasedEvent(Keys keyCode) : base(keyCode)
        {

        }
        public override PlatformEventType Type
        {
            get
            {
                return PlatformEventType.KeyReleased;
            }
        }

        public override PlatformEventCategory Category
        {
            get
            {
                return PlatformEventCategory.CategoryInput;
            }
        }

        public override string AsString
        {
            get
            {
                return "Key Released: Button [" + KeyCode + "]";
            }
        }

    }
}
