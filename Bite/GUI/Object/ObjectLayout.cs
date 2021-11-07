using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;

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

        internal void SetObject(VexObject obj)
        {
            m_Object = obj;
        }
        private VexObject m_Object;
    }
}
