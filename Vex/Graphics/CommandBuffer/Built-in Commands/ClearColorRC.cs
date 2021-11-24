using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// A render command which clears the current framebuffer with specified color
    /// </summary>
    public sealed class ClearColorRC : RenderCommand
    {
        public ClearColorRC(Color4 color)
        {
            m_Color = color;
        }
        protected override void ExecuteImpl()
        {
            GL.ClearColor(m_Color.R, m_Color.G, m_Color.B, m_Color.A);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }


        private Color4 m_Color;
    }
}
