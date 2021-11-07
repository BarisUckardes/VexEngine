using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// An interface to window and native window
    /// </summary>
    public sealed class WindowInterface
    {
        public WindowInterface(string applicationTitle,WindowCreateParams windowCreateParams, WindowUpdateParams windowUpdateParams)
        {
            m_Window = new Window(applicationTitle,windowCreateParams,windowUpdateParams);
        }

        /// <summary>
        /// Retunrs whether this window exists or exiting
        /// </summary>
        public bool HasWindowExitRequest
        {
            get
            {
                return (!m_Window.Exists || m_Window.IsExiting);
            }
        }

        /// <summary>
        /// Return the buffered events
        /// </summary>
        public Event[] Events
        {
            get
            {
                return m_Window.Events;
            }
        }

        public Window LocalWindow
        {
            get
            {
                return m_Window;
            }
        }

        /// <summary>
        /// Updates the window input
        /// </summary>
        public void UpdateInput()
        {
            Profiling.Profiler.StartProfile();
            m_Window.UpdateInput();
            Profiling.Profiler.EndProfile();
        }

        /// <summary>
        /// Swaps the swapchain backbuffer
        /// </summary>
        public void Swapbuffers()
        {
            m_Window.SwapBuffers();
        }

        private Window m_Window;
    }
}
