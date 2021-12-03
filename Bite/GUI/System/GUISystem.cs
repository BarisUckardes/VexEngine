using Bite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    /// <summary>
    /// Base class for all gui systems to implement
    /// </summary>
    public abstract class GUISystem
    {
        /// <summary>
        /// Called when the system first attached to the bite 
        /// </summary>
        public abstract void OnAttach();

        /// <summary>
        /// Called when the system removed from the bite
        /// </summary>
        public abstract void OnDetach();

        /// <summary>
        /// Called every frame
        /// </summary>
        public abstract void OnUpdate();


        /// <summary>
        /// Returns the target session of this gui system
        /// </summary>
        protected EditorSession Session
        {
            get
            {
                return m_Session;
            }
        }

        /// <summary>
        /// An internal session setter
        /// </summary>
        /// <param name="session"></param>
        internal void SetEditorSession(EditorSession session)
        {
            m_Session = session;
        }
        private EditorSession m_Session;
    }
}