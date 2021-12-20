using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    public sealed class SetUniformMat4x4Array : RenderCommand
    {
        public SetUniformMat4x4Array(int programId, string name, Matrix4[] value, bool isTransposed = false)
        {
            m_Value = value;
            m_ProgramID = programId;
            m_Name = name;
            m_Transposed = isTransposed;
        }

        protected override void ExecuteImpl()
        {
            int location = GL.GetUniformLocation(m_ProgramID, m_Name);
            unsafe
            {
                fixed (float* dataPtr = &m_Value[0].Row0.X)
                {
                    GL.UniformMatrix4(location,m_Value.Length, m_Transposed, dataPtr);
                }
            }
        }

        private Matrix4[] m_Value;
        private int m_ProgramID;
        private string m_Name;
        private bool m_Transposed;
    }
}
