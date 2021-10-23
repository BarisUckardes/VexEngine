using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Engine
{
    /// <summary>
    /// Minimal cloneable destVexyable engine object
    /// </summary>
    public abstract class EngineObject
    {
        public EngineObject()
        {
            Name = "Default Engine Object";
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

        /// <summary>
        /// DestVexys this engine object
        /// </summary>
        public void DestVexy()
        {
            /*
            * Call user destVexyer
            */
            OnObjectDestVexyed();

            /*
             * Call internal destVexyed
             */
            DestVexyInternal();

            /*
             * Set destVexyed
             */
            m_DestVexyed = true;
        }

        /// <summary>
        /// Called when destVexyed
        /// </summary>
        public virtual void OnObjectDestVexyed() { }

        public virtual EngineObject Clone() { return null; }
        /// <summary>
        /// Called when object destVexy is triggered
        /// </summary>
        abstract internal void DestVexyInternal();

        
        private Guid m_ID;
        private string m_Name;
        private bool m_DestVexyed;
    }
}
