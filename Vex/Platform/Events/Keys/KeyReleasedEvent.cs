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
    public sealed class KeyReleasedEvent : KeyEvent
    {
        public KeyReleasedEvent(Keys keyCode) : base(keyCode)
        {

        }
        public override EventType Type
        {
            get
            {
                return EventType.KeyReleased;
            }
        }

        public override EventCategory Category
        {
            get
            {
                return EventCategory.CategoryInput;
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
