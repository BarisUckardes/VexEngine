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
        public abstract void OnAttach();
        public abstract void OnDetach();
        public abstract void OnRenderLayout();

        protected void RequestDetach()
        {
            m_DetachRequest = true;
        }

        private bool m_DetachRequest;
    }
}
