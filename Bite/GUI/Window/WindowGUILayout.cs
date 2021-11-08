using Bite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    public abstract class WindowGUILayout
    {
      
        public WindowGUILayout()
        {
            m_DetachRequest = false;
        }

        internal bool HasDetachRequest
        {
            get
            {
                return m_DetachRequest;
            }
        }
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
        public EditorSession Session
        {
            get
            {
                return m_Session;
            }
            internal set
            {
                m_Session = value;
            }
        }
        public abstract void OnVisible();
        public abstract void OnInVisible();
        public abstract void OnLayoutBegin();
        public abstract void OnLayoutFinalize();
        public abstract void OnRenderLayout();

        protected void RequestDetach()
        {
            m_DetachRequest = true;
        }

        private EditorSession m_Session;
        private bool m_DetachRequest;
        private bool m_Visibility;
    }
}
