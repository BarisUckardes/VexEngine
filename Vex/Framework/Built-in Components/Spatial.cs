using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Vex.Types;

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
        public Vector3 Rotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                m_Rotation = value;
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

        /// <summary>
        /// Returns the forward vector of this spatial based on (X,Y,Z rotations)
        /// </summary>
        public Vector3 Forward
        {
            get
            {
                return new Vector3(
                    -(float)(Math.Cos(m_Rotation.X * Math.PI / 180) * Math.Cos(m_Rotation.Y * Math.PI / 180)),
                    (float)Math.Sin(m_Rotation.Y * Math.PI / 180),
                    (float)(Math.Sin(m_Rotation.X * Math.PI / 180) * Math.Cos(m_Rotation.Y * Math.PI / 180)));
            }
        }

        /// <summary>
        /// Returns the backwward vector of this spatial based on (X,Y,Z rotations)
        /// </summary>
        public Vector3 Backward
        {
            get
            {
                return -Forward;
            }
        }

        /// <summary>
        /// Returns the right vector of this spatial based on (X,Y,Z rotations)
        /// </summary>
        public Vector3 Right
        {
            get
            {
                return Vector3.Cross(-Vector3.UnitY, Forward);
            }
        }

        /// <summary>
        /// Returns the left vector of this spatial based on (X,Y,Z rotations)
        /// </summary>
        public Vector3 Left
        {
            get
            {
                return -Right;
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

        [ExposeThis]
        Vector3 m_Position;
        [ExposeThis]
        Vector3 m_Rotation;
        [ExposeThis]
        Vector3 m_Scale;
        Vector3 m_Forward;
    }
}
