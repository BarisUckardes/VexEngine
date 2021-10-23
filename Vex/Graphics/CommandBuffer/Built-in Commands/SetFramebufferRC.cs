using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// A render command which sets a framebuffer as current
    /// </summary>
    public sealed class SetFramebufferRC : RenderCommand
    {
        public SetFramebufferRC(Framebuffer framebuffer)
        {
            m_Framebuffer = framebuffer;
        }
        protected override void ExecuteImpl()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_Framebuffer.FramebufferID);
        }

        private Framebuffer m_Framebuffer;
    }
}
