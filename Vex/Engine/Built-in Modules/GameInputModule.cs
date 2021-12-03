using Vex.Input;
using Vex.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Profiling;
using System.Numerics;

namespace Vex.Engine
{
    /// <summary>
    /// Engine game input module
    /// </summary>
    public sealed class GameInputModule : EngineModule
    {
        public override void OnAttach()
        {
            m_PressedKeys = new List<Keys>();
            m_ReleasedKeys = new List<Keys>();
            m_DownKeys = new List<Keys>();
        }

        public override void OnUpdate(bool active)
        {
            if(active)
            {
                /*
                * Update these keys to game input
                */
                GameInput.SetKeyEvents(new List<Keys>(m_PressedKeys),new List<Keys>(m_ReleasedKeys),m_DownKeys);

                /*
                 * Clear one time key buffers
                 */
                m_PressedKeys.Clear();
                m_ReleasedKeys.Clear();
            }
        }

        public override void OnDetach()
        {

        }

        public override void OnEvent(PlatformEvent eventData)
        {
            
            if(eventData.Type == PlatformEventType.KeyPressed)
            {
                /*
                 * Get key code
                 */
                Keys keyCode = ((PlatformKeyPressedEvent)eventData).KeyCode;

                /*
                 * Register it to the pressed keys
                 */
                m_PressedKeys.Add(keyCode);

                /*
                 * Validate uniqueness and register it to the down keys
                 */
                if(!m_DownKeys.Contains(keyCode))
                    m_DownKeys.Add(keyCode);
            }
            else if(eventData.Type == PlatformEventType.KeyReleased)
            {
                /*
                 * Get key code
                 */
                Keys keyCode = ((PlatformKeyReleasedEvent)eventData).KeyCode;

                /*
                 * Register it to the released keys
                 */
                m_ReleasedKeys.Add(keyCode);

                /*
                 * Try remove it from the down keys
                 */
                m_DownKeys.Remove(keyCode);
            }
            else if (eventData.Type == PlatformEventType.MouseMoved)
            {
                /*
                 * Set mouse position
                 */
                m_MousePosition = new Vector2(((PlatformMouseMovedEvent)eventData).X, ((PlatformMouseMovedEvent)eventData).Y);
            }

            /*
             * Mark this event handled
             */
            eventData.MarkHandled();
        }

        private List<Keys> m_PressedKeys;
        private List<Keys> m_ReleasedKeys;
        private List<Keys> m_DownKeys;
        private Vector2 m_MousePosition;
    }
}
