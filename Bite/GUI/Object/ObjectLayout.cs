using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Bite.Core;

namespace Bite.GUI
{
    public abstract class ObjectLayout
    {
        public abstract void OnAttach();
        public abstract void OnDetach();
        public abstract void OnLayoutRender();

        protected VexObject Object
        {
            get
            {
                return m_Object;
            }
         
        }

        protected EditorSession Session
        {
            get
            {
                return m_Session;
            }
            
        }

        internal void SetObject(VexObject obj)
        {
            m_Object = obj;
        }
        internal void SetSession(EditorSession session)
        {
            m_Session = session;
        }

        private VexObject m_Object;
        private EditorSession m_Session;
    }
}
