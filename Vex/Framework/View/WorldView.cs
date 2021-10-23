using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Represents a single view of a world
    /// </summary>
    public abstract class WorldView : EngineObject
    {

        /// <summary>
        /// Returns the world of this view
        /// </summary>
        public World World
        {
            get
            {
                return m_World;
            }
            internal set
            {
                m_World = value;
            }
        }
        private World m_World;
    }
}
