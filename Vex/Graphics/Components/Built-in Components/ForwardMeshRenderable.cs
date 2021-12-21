using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Types;

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

        [ExposeThis]
        public Texture2D ColorTexture
        {
            get
            {
                return m_ColorTexture;
            }
            set
            {
                m_ColorTexture = value;
            }
        }


        [ExposeThis]
        public Texture2D NormalTexture
        {
            get
            {
                return m_NormalTexture;
            }
            set
            {
                m_NormalTexture = value;
            }
        }

        [ExposeThis]
        private Texture2D m_ColorTexture;
        [ExposeThis]
        private Texture2D m_NormalTexture;
        [ExposeThis]
        private StaticMesh m_Mesh;
    }
}
