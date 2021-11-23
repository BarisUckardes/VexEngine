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
    public sealed class SpriteRenderable : RenderableComponent
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
        public Texture2D SpriteTexture
        {
            get
            {
                return m_Texture;
            }
            set
            {
                m_Texture = value;
            }
        }

        public override bool ShouldTick
        {
            get
            {
                return false;
            }
        }

        private Texture2D m_Texture;
        private StaticMesh m_Mesh;
    }
}
