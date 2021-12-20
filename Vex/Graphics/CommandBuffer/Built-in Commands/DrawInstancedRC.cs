using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    public sealed class DrawInstancedRC : RenderCommand
    {
        public DrawInstancedRC(int triangleCount,int instanceCount)
        {
            m_TriangleCount = triangleCount;
            m_InstanceCount = instanceCount;
        }
        protected override void ExecuteImpl()
        {
            GL.DrawElementsInstanced(PrimitiveType.Triangles,m_TriangleCount,DrawElementsType.UnsignedByte,IntPtr.Zero,m_InstanceCount);
        }

        private int m_TriangleCount;
        private int m_InstanceCount;
    }
}
