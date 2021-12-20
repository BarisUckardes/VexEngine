using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    public sealed class SetUniformVector4ArrayRC : RenderCommand
    {
        public SetUniformVector4ArrayRC(int programID, in string uniformName, in Vector4[] value)
        {
            m_UniformName = uniformName;
            m_Value = value;
            m_ProgramID = programID;
        }
        protected override void ExecuteImpl()
        {
            int location = GL.GetUniformLocation(m_ProgramID, m_UniformName);
            unsafe
            {
                fixed(float* dataPtr = &m_Value[0].X)
                {
                    GL.Uniform4(location, m_Value.Length, dataPtr);
                }
            }
            
        }
        private string m_UniformName;
        private Vector4[] m_Value;
        private int m_ProgramID;
    }
}

