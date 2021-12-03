using Bite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    /// <summary>
    /// Base class for all window gui layouts to implement
    /// </summary>
    public abstract class WindowGUILayout
    {
      
        public WindowGUILayout()
        {
            m_DetachRequest = false;
            m_ID = Guid.NewGuid();
        }


        /// <summary>
        /// Returns whether this window is visible or not
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return m_Visibility;
            }
            internal set
            {
                m_Visibility = value;
            }
        }
      
        /// <summary>
        /// Returns the unique id of the window
        /// </summary>
        public Guid ID
        {
            get
            {
                return m_ID;
            }
            internal set
            {
                m_ID = value;
            }
        }
       
        /// <summary>
        /// Called when window state change to visible from invisible
        /// </summary>
        public abstract void OnVisible();

        /// <summary>
        /// Called when window state changed to invisible from visible
        /// </summary>
        public abstract void OnInVisible();

        /// <summary>
        /// Called when the first time this window layout spawned
        /// </summary>
        public abstract void OnLayoutBegin();

        /// <summary>
        /// Called when this window layout is terminated
        /// </summary>
        public abstract void OnLayoutFinalize();

        /// <summary>
        /// Called each frame
        /// </summary>
        public abstract void OnRenderLayout();


        /// <summary>
        /// Returns the target editor session
        /// </summary>
        protected EditorSession Session
        {
            get
            {
                return m_Session;
            }
        }

        /// <summary>
        /// Sets a detach request
        /// </summary>
        protected void RequestDetach()
        {
            m_DetachRequest = true;
        }

        /// <summary>
        /// Returns whether this window has detach request
        /// </summary>
        internal bool HasDetachRequest
        {
            get
            {
                return m_DetachRequest;
            }
        }

        /// <summary>
        /// An internal setter for target editor session
        /// </summary>
        /// <param name="session"></param>
        internal void SetEditorSession(EditorSession session)
        {
            m_Session = session;
        }

        private EditorSession m_Session;
        private bool m_DetachRequest;
        private bool m_Visibility;
        private Guid m_ID;
    }
}
