using Vex.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    public sealed class PlatformKeyCharEvent : PlatformKeyEvent
    {
        public PlatformKeyCharEvent(Keys key) : base(key)
        {

        }
        public override PlatformEventType Type
        {
            get
            {
                return PlatformEventType.KeyChar;
            }
        }

        public override PlatformEventCategory Category
        {
            get
            {
                return PlatformEventCategory.CategoryKeyboard;
            }
        }

        public override string AsString
        {
            get
            {
                return "Key Char: " + KeyCode.ToString();
            }
        }
    }
}
