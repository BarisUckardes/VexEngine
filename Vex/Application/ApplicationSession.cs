using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Asset;
using Vex.Framework;
using Vex.Platform;
namespace Vex.Application
{
    /// <summary>
    /// Reprensets the single session in an application's lifetime
    /// </summary>d
    public sealed class ApplicationSession
    {
        
        public ApplicationSession(WindowInterface applicationWindow)
        {
            /*
             * Initialize local fields
             */
            m_Window = applicationWindow;

            /*
            * Create asset pool
            */
            m_AssetPool = new AssetPool(PlatformPaths.DomainDirectory);

            /*
             * Set dummy singleton
             */
            World.Session = this;
        }

        /// <summary>
        /// Returns all the worlds in this session
        /// </summary>
        public World CurrentWorld
        {
            get
            {
                return m_CurrentWorld;
            }
        }

        /// <summary>
        /// Returns the asset pool of this session
        /// </summary>
        public AssetPool AssetPool
        {
            get
            {
                return m_AssetPool;
            }
        }

        public WindowInterface Window
        {
            get
            {
                return m_Window;
            }
        }
        public string WindowTitle
        {
            get
            {
                return m_Window.LocalWindow.WindowTitle;
            }
            set
            {
                m_Window.LocalWindow.WindowTitle = value;
            }
        }
        public bool HasShutdownRequest
        {
            get
            {
                return m_SessionHasShutdownRequest;
            }
          
        }
        public string ShutdownRequestMessage
        {
            get
            {
                return m_ShutdownReasonMessage;
            }
        }
        public bool PlayActive
        {
            get
            {
                return m_PlayActive;
            }
            set
            {
                m_PlayActive = value;
            }
        }

        public void SetShutdownRequest(string reason = "undefined")
        {
            m_SessionHasShutdownRequest = true;
            m_ShutdownReasonMessage = reason;

        }
        /// <summary>
        /// Shutdowns the entire session
        /// </summary>
        public void Shutdown()
        {

        }

        /// <summary>
        /// Registers a world into this session
        /// </summary>
        /// <param name="world"></param>
        public void SetCurrentWorld(World world)
        {
            m_CurrentWorld = world;
        }

        /// <summary>
        /// Removes a world fVexm this session
        /// </summary>
        /// <param name="world"></param>
        public void ClearCurrentWorld()
        {

        }

        private AssetPool m_AssetPool;
        private WindowInterface m_Window;
        private World m_CurrentWorld;
        private bool m_SessionHasShutdownRequest;
        private bool m_PlayActive;
        private string m_ShutdownReasonMessage;
    }
}
