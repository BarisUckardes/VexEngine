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
    public sealed class PlatformKeyPressedEvent : PlatformKeyEvent
    {
        public PlatformKeyPressedEvent(Keys keyCode,int repeatCount) : base(keyCode)
        {
            m_RepeatCount = repeatCount;
        }
        public override PlatformEventType Type
        {
            get
            {
                return PlatformEventType.KeyPressed;
            }
        }

        public override PlatformEventCategory Category
        {
            get
            {
                return PlatformEventCategory.Input;
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
