using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{

    /// <summary>
    /// A command which will be executed when application started and application finalized
    /// </summary>
    public abstract class CoreCommand
    {
        /// <summary>
        /// Called when first application started
        /// </summary>
        public abstract void OnAttach();

        /// <summary>
        /// Called when application finalized
        /// </summary>
        public abstract void OnDetach();


        /// <summary>
        /// Returns the session
        /// </summary>
        protected EditorSession EditorSession
        {
            get
            {
                return m_Session;
            }
          
        }

        /// <summary>
        /// An internal sett for editor session reference
        /// </summary>
        /// <param name="session"></param>
        internal void SetEditorSession(EditorSession session)
        {
            m_Session = session;
        }

      
        private EditorSession m_Session;
    }
}
