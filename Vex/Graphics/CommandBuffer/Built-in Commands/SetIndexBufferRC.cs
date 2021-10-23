using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Set index buffer render command class
    /// </summary>
    public sealed class SetIndexBufferRC : RenderCommand
    {
        public SetIndexBufferRC(in IndexBuffer indexBuffer)
        {
            m_Buffer = indexBuffer;
        }

        protected override void ExecuteImpl()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_Buffer.IndexBufferID);
        }

        private IndexBuffer m_Buffer;
    }
}
