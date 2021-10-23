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
    public sealed class Session
    {
        /// <summary>
        /// Returns the current session
        /// </summary>
        public static Session CurrentSession
        {
            get
            {
                return s_Session;
            }
        }

        private static Session s_Session;

        public Session(WindowInterface applicationWindow)
        {
            /*
             * Set current
             */
            SetCurrent();

            /*
             * Initialize local fields
             */
            m_Worlds = new List<World>();
            m_Window = applicationWindow;

            /*
            * Create asset pool
            */
            m_AssetPool = new AssetPool(Paths.ExecutableDirectory);

           
        }

        /// <summary>
        /// Returns all the worlds in this session
        /// </summary>
        public World[] Worlds
        {
            get
            {
                return m_Worlds.ToArray();
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

        /// <summary>
        /// Sets this session as the current session of this application
        /// </summary>
        internal void SetCurrent()
        {
            s_Session = this;
        }


        private AssetPool m_AssetPool;
        private WindowInterface m_Window;
        private List<World> m_Worlds;
    }
}
