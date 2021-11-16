using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Vex.Graphics
{
    /// <summary>
    /// Set viewport render command class
    /// </summary>
    public sealed class SetViewportRC : RenderCommand
    {
        public SetViewportRC(in Vector2 offset,in Vector2 size)
        {
            m_Offset = offset;
            m_Size = size;
        }
        protected override void ExecuteImpl()
        {
            GL.Viewport((int)m_Offset.X, (int)m_Offset.Y, (int)m_Size.X, (int)m_Size.Y);
        }


        private Vector2 m_Offset;
        private Vector2 m_Size;
    }
}
