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
            m_Worlds = new List<World>();
            m_Window = applicationWindow;

            /*
            * Create asset pool
            */
            m_AssetPool = new AssetPool(PlatformPaths.DomainDirectory);
        }

        /// <summary>
        /// Returns all the worlds in this session
        /// </summary>
        public IReadOnlyCollection<World> Worlds
        {
            get
            {
                return m_Worlds.AsReadOnly();
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
            set
            {
                m_SessionHasShutdownRequest = value;
            }
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
        public void RegisterWorld(World world)
        {
            m_Worlds.Add(world);
        }

        /// <summary>
        /// Removes a world fVexm this session
        /// </summary>
        /// <param name="world"></param>
        public void RemoveWorld(World world)
        {
            m_Worlds.Remove(world);
        }

        private AssetPool m_AssetPool;
        private WindowInterface m_Window;
        private List<World> m_Worlds;
        private bool m_SessionHasShutdownRequest;
    }
}
