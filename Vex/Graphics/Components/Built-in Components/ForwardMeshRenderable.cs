using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Custom sprite renderable component
    /// </summary>
    public sealed class ForwardMeshRenderable : RenderableComponent
    {
        public StaticMesh Mesh
        {
            get
            {
                return m_Mesh;
            }
            set
            {
                m_Mesh = value;
            }
        }
       

        public override bool ShouldTick
        {
            get
            {
                return false;
            }
        }

        private StaticMesh m_Mesh;
    }
}
