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
            m_EventBuffer = new List<Event>();
            m_Events = new List<Event>();
            m_Width = windowCreateParams.Width;
            m_Height = windowCreateParams.Height;
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
        public Event[] Events
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
            m_Events = new List<Event>(m_EventBuffer);
            m_EventBuffer.Clear();

        }

        protected override void OnResize(ResizeEventArgs e)
        {
            WindowResizeEvent ev = new WindowResizeEvent((uint)e.Width,(uint)e.Height);
            m_EventBuffer.Add(ev);

            m_Width = (int)ev.Width;
            m_Height = (int)ev.Height;
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            MouseMovedEvent ev = new MouseMovedEvent(e.Position.X, e.Position.Y);
            m_EventBuffer.Add(ev);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            MouseButtonPressedEvent ev = new MouseButtonPressedEvent((int)e.Button);
            m_EventBuffer.Add(ev);
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            MouseButtonReleasedEvent ev = new MouseButtonReleasedEvent((int)e.Button);
            m_EventBuffer.Add(ev);
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            KeyPressedEvent ev = new KeyPressedEvent((Keys)e.Key,e.IsRepeat ? 1 : 0);
            m_EventBuffer.Add(ev);
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            KeyReleasedEvent ev = new KeyReleasedEvent((Keys)e.Key);
            m_EventBuffer.Add(ev);
        }
        protected override void OnTextInput(TextInputEventArgs e)
        {
            KeyCharEvent ev = new KeyCharEvent((Keys)e.Unicode);
            m_EventBuffer.Add(ev);
        }


        private List<Event> m_Events;
        private List<Event> m_EventBuffer;
        private WindowInputState m_InputState;
        private int m_Width;
        private int m_Height;
    }
}
