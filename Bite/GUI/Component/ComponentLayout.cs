using Vex.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    public abstract class ComponentLayout
    {
        public abstract void OnAttach();
        public abstract void OnDetach();
        public abstract void OnLayoutRender();

        public Component TargetComponent
        {
            get
            {
                return m_TargetComponent;
            }
            internal set
            {
                m_TargetComponent = value;
            }
        }
        private Component m_TargetComponent;
    }
}
