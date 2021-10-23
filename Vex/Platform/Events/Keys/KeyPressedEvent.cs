using Vex.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Key pressed event for consuming
    /// </summary>
    public sealed class KeyPressedEvent : KeyEvent
    {
        public KeyPressedEvent(Keys keyCode,int repeatCount) : base(keyCode)
        {
            m_RepeatCount = repeatCount;
        }
        public override EventType Type
        {
            get
            {
                return EventType.KeyPressed;
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
                return "Key Pressed: Button [" + KeyCode + "] Repeat [" + m_RepeatCount + "]";
            }
        }
        public int RepeatCount
        {
            get
            {
                return m_RepeatCount;
            }
        }

        private int m_RepeatCount;
    }
}
