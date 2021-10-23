using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// A mesh class desinged to optimize 2D sprite meshes
    /// </summary>
    public sealed class SpriteMesh : Mesh
    {
        public override VertexLayout Layout
        {
            get
            {
                List<VertexLayoutElement> elements = new List<VertexLayoutElement>();
                elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float2, "v_Position"));
                elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float2, "v_Uv"));
                return new VertexLayout(elements.ToArray());
            }
        }

        /// <summary>
        /// Return the sprite vertex data
        /// </summary>
        public SpriteVertex[] VertexData
        {
            get
            {
                return m_CpuVertexes;
            }
        }

        /// <summary>
        /// Return the triangle data
        /// </summary>
        public int[] TriangleData
        {
            get
            {
                return m_CpuTriangles;
            }
        }


        /// <summary>
        /// Set vertex data of this mesh
        /// </summary>
        /// <param name="vertexes"></param>
        public void SetVertexData(SpriteVertex[] vertexes)
        {
            /*
             * Set vertex data
             */
            SetVertexBufferData(vertexes);

            /*
             * Set Cpu vertexes
             */
            m_CpuVertexes = vertexes;
        }

        /// <summary>
        /// Set triangle data of this mesh
        /// </summary>
        /// <param name="triangles"></param>
        public void SetTriangleData(int[] triangles)
        {
            /*
             * Set Index data
             */
            SetIndexBufferData(triangles);

            /*
             * Set cpu triangles
             */
            m_CpuTriangles = triangles;
        }

        private int[] m_CpuTriangles;
        private SpriteVertex[] m_CpuVertexes;
    }
}
