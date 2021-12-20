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
    /// Set uniform vector4 render command class
    /// </summary>
    public sealed class SetUniformVector4RC : RenderCommand
    {
        public SetUniformVector4RC(int programID,in string uniformName,in Vector4 value)
        {
            m_UniformName = uniformName;
            m_Value = value;
            m_ProgramID = programID;
        }
        protected override void ExecuteImpl()
        {
            int location = GL.GetUniformLocation(m_ProgramID, m_UniformName);
            GL.Uniform4(location, m_Value.X,m_Value.Y,m_Value.Z,m_Value.W);
        }


        private string m_UniformName;
        private Vector4 m_Value;
        private int m_ProgramID;
    }
}
