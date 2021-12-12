using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    public sealed class ClearDepthRC : RenderCommand
    {
        public ClearDepthRC(float clearValue)
        {
            m_ClearValue = clearValue;
        }
        protected override void ExecuteImpl()
        {
            GL.ClearDepth(m_ClearValue);
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

        private float m_ClearValue;
    }
}
