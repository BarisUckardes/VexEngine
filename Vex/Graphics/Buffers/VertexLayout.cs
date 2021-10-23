using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Represents the layout of the vertex buffer
    /// </summary>
    public sealed class VertexLayout
    {
        public VertexLayout(VertexLayoutElement[] elements)
        {
            /*
             * Set member fields
             */
            m_Elements = elements;

            /*
             * Calculate offsets and stride
             */
            CalculateOffsetsAndStride();
        }

        /// <summary>
        /// Returns the each layout element this vertex layout has
        /// </summary>
        public VertexLayoutElement[] Elements
        {
            get
            {
                return m_Elements;
            }
        }

        /// <summary>
        /// Returns the stride of this vertex layout
        /// </summary>
        public uint Stride
        {
            get
            {
                return m_Stride;
            }
        }

        /// <summary>
        /// Calulates the offsets and the stride of this vertex layout
        /// </summary>
        private void CalculateOffsetsAndStride()
        {
            uint offset = 0;
            m_Stride = 0;
            for(int i=0;i<m_Elements.Length;i++)
            {
                m_Elements[i].Offset = offset;
                offset += m_Elements[i].Size;
                m_Stride += m_Elements[i].Size;
            }
        }
        private VertexLayoutElement[] m_Elements;
        uint m_Stride;
    }
}
