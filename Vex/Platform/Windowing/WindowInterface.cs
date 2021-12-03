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
        public WindowInterface(string applicationTitle,WindowCreateParams windowCreateParams, WindowUpdateParams windowUpdateParams,bool intermediateFramebufferAsSwapchain)
        {
            m_Window = new Window(applicationTitle,windowCreateParams,windowUpdateParams, intermediateFramebufferAsSwapchain);
        }

        /// <summary>
        /// Retunrs whether this window exists or exiting
        /// </summary>
        public bool HasWindowExitRequest
        {
            get
            {
                return m_Window.HasWindowCloseRequest;
            }
        }

        /// <summary>
        /// Return the buffered events
        /// </summary>
        public PlatformEvent[] Events
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
            m_Window.UpdateInput();
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
