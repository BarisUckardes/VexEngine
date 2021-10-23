using OpenTK.Mathematics;
using Vex.Input;
using Vex.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Engine
{
    /// <summary>
    /// Engine game input module
    /// </summary>
    public sealed class GameInputModule : EngineModule
    {
        public GameInputModule()
        {
            m_PressedKeysBuffer = new List<Keys>();

            /*
             * Create empty input state
             */
            InputInterface.SetInputFrame(new InputFrame(m_PressedKeysBuffer.ToArray(),new Vector2(0,0)));
        }
        public override void OnAttach()
        {
            m_PressedKeysBuffer = new List<Keys>();
        }

        public override void OnUpdate()
        {
            /*
             * Create new input state for game
             */
            InputFrame frame = new InputFrame(m_PressedKeysBuffer.ToArray(),m_MousePosition);

            /*
             * Update input state to input system
             */
            InputInterface.SetInputFrame(frame);

            /*
             * Clear buffered keys
             */
            m_PressedKeysBuffer.Clear();
        }

        public override void OnDetach()
        {

        }

        public override void OnEvent(Event eventData)
        {

            if(eventData.Type == EventType.KeyPressed)
            {
                m_PressedKeysBuffer.Add(((KeyPressedEvent)eventData).KeyCode);
            }
            else if(eventData.Type == EventType.MouseMoved)
            {
                m_MousePosition = new Vector2(((MouseMovedEvent)eventData).X, ((MouseMovedEvent)eventData).Y);
            }

            /*
             * Mark this event handled
             */
            eventData.MarkHandled();
        }

        private Vector2 m_MousePosition;
        private List<Keys> m_PressedKeysBuffer;
    }
}
