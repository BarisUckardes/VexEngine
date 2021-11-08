using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Vex.Input;
using Vex.Engine;

namespace Vex.Platform
{
    /// <summary>
    /// Main window class for handling native window
    /// </summary>
    public class Window : GameWindow
    {
        public Window(string applicationTitle,WindowCreateParams windowCreateParams,WindowUpdateParams windowUpdateParams) : base(
            new GameWindowSettings()
            {
                IsMultiThreaded = windowUpdateParams.IsMultiThreaded,
                RenderFrequency = windowUpdateParams.RenderFrequency,
                UpdateFrequency = windowUpdateParams.UpdateFrequency
            },
            new NativeWindowSettings()
            {
                API = ContextAPI.OpenGL,
                WindowState = WindowStateUtils.GetNativeWindowState(windowCreateParams.State),
                Location = new OpenTK.Mathematics.Vector2i(windowCreateParams.OffsetX, windowCreateParams.OffsetY),
                Size = new OpenTK.Mathematics.Vector2i(windowCreateParams.Width, windowCreateParams.Height),
                StartFocused = windowCreateParams.StartsFocused,
                Title = applicationTitle
            }
            )
        {
            /*
             * Initialize 
             */
            m_EventBuffer = new List<PlatformEvent>();
            m_Events = new List<PlatformEvent>();
            m_Width = windowCreateParams.Width;
            m_Height = windowCreateParams.Height;

            /*
            * Set global size
            */
            PlatformWindowProperties.Size = new System.Numerics.Vector2(windowCreateParams.Width, windowCreateParams.Height);
        }

        /// <summary>
        /// Returns the one time input state
        /// </summary>
        public WindowInputState InputState
        {
            get
            {
                return m_InputState;
            }
        }
        public PlatformEvent[] Events
        {
            get
            {
                return m_Events.ToArray();
            }
        }

        public int Width
        {
            get
            {
                return m_Width;
            }
        }
        public int Height
        {
            get
            {
                return m_Height;
            }
        }

        /// <summary>
        /// Updates the native window input events and creates a one-time inout state
        /// </summary>
        public void UpdateInput()
        {
            /*
             * PVexcess window events
             */
            ProcessEvents();

            /*
             * Create new input state
             */
            m_InputState = new WindowInputState(KeyboardState.GetSnapshot());

            /*
             * Set events
             */
            m_Events = new List<PlatformEvent>(m_EventBuffer);
            m_EventBuffer.Clear();

        }

        public void SetApplicationEventDelegate(ReceivePlatformEventDelegate del)
        {
            m_ApplicationEventDelegate = del;
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            /*
             * Get window reisze event
             */
            WindowResizeEvent ev = new WindowResizeEvent((uint)e.Width,(uint)e.Height);

            /*
             * Set local size
             */
            m_Width = (int)ev.Width;
            m_Height = (int)ev.Height;

            /*
             * Set global size
             */
            PlatformWindowProperties.Size = new System.Numerics.Vector2(e.Width, e.Height);

            /*
             * Inform application
             */
            m_ApplicationEventDelegate(ev);
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            /*
             * Get platform mouse moved evvent
             */
            PlatformMouseMovedEvent ev = new PlatformMouseMovedEvent(e.Position.X, e.Position.Y);

            /*
            * Inform application
            */
            m_ApplicationEventDelegate(ev);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            /*
             * Get platform mouse donw event
             */
            PlatformMouseButtonPressedEvent ev = new PlatformMouseButtonPressedEvent((int)e.Button);

            /*
            * Inform application
            */
            m_ApplicationEventDelegate(ev);
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            /*
             * Get platform mouse up event
             */
            PlatformMouseButtonReleasedEvent ev = new PlatformMouseButtonReleasedEvent((int)e.Button);

            /*
            * Inform application
            */
            m_ApplicationEventDelegate(ev);
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            /*
             * Get platform on key down event
             */
            PlatformKeyPressedEvent ev = new PlatformKeyPressedEvent((Keys)e.Key,e.IsRepeat ? 1 : 0);

            /*
            * Inform application
            */
            m_ApplicationEventDelegate(ev);
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            /*
             * Get Platform key released event
             */
            PlatformKeyReleasedEvent ev = new PlatformKeyReleasedEvent((Keys)e.Key);

            /*
            * Inform application
            */
            m_ApplicationEventDelegate(ev);
        }
        protected override void OnTextInput(TextInputEventArgs e)
        {
            /*
             * Get platform on key char event
             */
            PlatformKeyCharEvent ev = new PlatformKeyCharEvent((Keys)e.Unicode);

            /*
            * Inform application
            */
            m_ApplicationEventDelegate(ev);
        }


        private List<PlatformEvent> m_Events;
        private List<PlatformEvent> m_EventBuffer;
        private WindowInputState m_InputState;
        private ReceivePlatformEventDelegate m_ApplicationEventDelegate;
        private int m_Width;
        private int m_Height;
    }
}
