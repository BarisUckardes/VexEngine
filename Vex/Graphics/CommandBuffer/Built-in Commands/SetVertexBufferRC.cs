using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Set vertex buffer render command class
    /// </summary>
    public class SetVertexBufferRC : RenderCommand
    {

        public SetVertexBufferRC(VertexBuffer vertexBuffer)
        {
            m_Buffer = vertexBuffer;
        }
        protected override void ExecuteImpl()
        {
            /*
             * Get vertex handle
             */
            VertexBufferHandle handle = m_Buffer == null ? new VertexBufferHandle(0,0) : m_Buffer.Handle;

            /*
             * Set vertex array
             */
            GL.BindVertexArray(handle.VertexArrayID);

            /*
             * Set vertex buffer
             */
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle.VertexBufferID);
        }

        private VertexBuffer m_Buffer;
    }
}
