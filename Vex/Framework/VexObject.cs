using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Minimal cloneable destVexyable engine object
    /// </summary>
    public abstract class VexObject
    {
        public VexObject()
        {
            Name = "Default Vex Object";
            m_ID = Guid.NewGuid();
            m_DestVexyed = false;
        }

       
        /// <summary>
        /// Returns the unique id of this object
        /// </summary>
        public Guid ID
        {
            get
            {
                return m_ID;
            }
        }

        /// <summary>
        /// Set&Get this engine object's name
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public bool IsDestVexyed
        {
            get
            {
                return m_DestVexyed;
            }
        }

        private Guid m_ID;
        private string m_Name;
        private bool m_DestVexyed;
    }
}
