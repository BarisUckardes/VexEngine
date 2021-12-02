using Vex.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    /// <summary>
    /// A gui layout for each component rendered under the entity object properties
    /// </summary>
    public abstract class ComponentLayout
    {
        /// <summary>
        /// Called when the entity first time clicked
        /// </summary>
        public abstract void OnAttach();

        /// <summary>
        /// Called when entity disappeared from the object observer window
        /// </summary>
        public abstract void OnDetach();

        /// <summary>
        /// Called when component tab not collapsed on the entity object properties
        /// </summary>
        public abstract void OnLayoutRender();


        /// <summary>
        /// Returns the target component of this component gui layout
        /// </summary>
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
