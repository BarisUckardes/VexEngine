using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    public abstract class ObjectLayout
    {
        public abstract void OnAttach();
        public abstract void OnDetach();
        public abstract void OnLayoutRender();

        protected EngineObject Object
        {
            get
            {
                return m_Object;
            }
         
        }

        internal void SetObject(EngineObject obj)
        {
            m_Object = obj;
        }
        private EngineObject m_Object;
    }
}
