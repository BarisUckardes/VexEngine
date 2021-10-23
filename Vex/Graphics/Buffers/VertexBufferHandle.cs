using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Represents the vertex buffer handle package
    /// </summary>
    public readonly struct VertexBufferHandle
    {
        public VertexBufferHandle(uint vertexBufferID = 0, uint vertexArrayID = 0)
        {
            VertexBufferID = vertexBufferID;
            VertexArrayID = vertexArrayID;
        }

        /// <summary>
        /// The vertex buffer id
        /// </summary>
        public readonly uint VertexBufferID;
        /// <summary>
        /// The vertex array id
        /// </summary>
        public readonly uint VertexArrayID;
    }
}
