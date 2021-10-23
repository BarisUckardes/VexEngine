using Vex.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    public sealed class KeyCharEvent : KeyEvent
    {
        public KeyCharEvent(Keys key) : base(key)
        {

        }
        public override EventType Type
        {
            get
            {
                return EventType.KeyChar;
            }
        }

        public override EventCategory Category
        {
            get
            {
                return EventCategory.CategoryKeyboard;
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
