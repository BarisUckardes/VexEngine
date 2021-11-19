using OpenTK.Mathematics;
using Vex.Input;
using Vex.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Profiling;

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
        }
        public override void OnAttach()
        {
            m_PressedKeysBuffer = new List<Keys>();
        }

        public override void OnUpdate()
        {
           
        }

        public override void OnDetach()
        {

        }

        public override void OnEvent(PlatformEvent eventData)
        {
            
            if(eventData.Type == PlatformEventType.KeyPressed)
            {
                m_PressedKeysBuffer.Add(((PlatformKeyPressedEvent)eventData).KeyCode);
            }
            else if(eventData.Type == PlatformEventType.MouseMoved)
            {
                m_MousePosition = new Vector2(((PlatformMouseMovedEvent)eventData).X, ((PlatformMouseMovedEvent)eventData).Y);
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
