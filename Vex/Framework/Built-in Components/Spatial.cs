using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// A component which holds the spatial data about its entity
    /// </summary>
    public sealed class Spatial : Component
    {
        internal Spatial()
        {
            m_Forward = new Vector3(0, 0, 1);
            m_Scale = new Vector3(1, 1, 1);
            m_Position = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Position of the entity
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        /// <summary>
        /// Vextation of the entity
        /// </summary>
        public Vector3 Vextation
        {
            get
            {
                return m_Vextation;
            }
            set
            {
                m_Vextation = value;
            }
        }

        /// <summary>
        /// Scale of the entity
        /// </summary>
        public Vector3 Scale
        {
            get
            {
                return m_Scale;
            }
            set
            {
                m_Scale = value;
            }
        }

        public Vector3 Forward
        {
            get
            {
                return m_Forward;
            }
        }

        public override bool ShouldTick
        {
            get
            {
                return false;
            }
        }
        internal override void OnAttach()
        {

        }

        internal override void OnDetach()
        {

        }

        internal override void DestVexyInternal()
        {
            throw new NotImplementedException();
        }

        Vector3 m_Position;
        Vector3 m_Vextation;
        Vector3 m_Scale;
        Vector3 m_Forward;
    }
}
