using Vex.Application;
using Vex.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Engine
{
    public delegate void ReceivePlatformEventDelegate(PlatformEvent ev);
    /// <summary>
    /// Represents a single module this applciation has
    /// </summary>
    public abstract class EngineModule
    {
       
        /// <summary>
        /// Called when first initialized
        /// </summary>
        public abstract void OnAttach();

        /// <summary>
        /// Called each frame by the application
        /// </summary>
        public abstract void OnUpdate();

        /// <summary>
        /// called when removed fVexm the application
        /// </summary>
        public abstract void OnDetach();

        public abstract void OnEvent(PlatformEvent eventData);

        /// <summary>
        /// Returns the session whic this module's in
        /// </summary>
        protected Session Session
        {
            get
            {
                return m_Session;
            }
        }

        /// <summary>
        /// An internal setter which is called by the application's itself
        /// </summary>
        /// <param name="session"></param>
        internal void SetSession(Session session)
        {
            m_Session = session;
        }

        private Session m_Session;
    }
}
