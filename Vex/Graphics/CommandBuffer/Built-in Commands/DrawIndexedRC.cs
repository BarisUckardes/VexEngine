using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Indexed draw call command class
    /// </summary>
    public sealed class DrawIndexedRC : RenderCommand
    {
        public DrawIndexedRC(in int count)
        {
            m_Count = count;
        }
        protected override void ExecuteImpl()
        {
            GL.DrawElements(BeginMode.Triangles, m_Count, DrawElementsType.UnsignedInt, 0);
        }

        private int m_Count;
    }
}
