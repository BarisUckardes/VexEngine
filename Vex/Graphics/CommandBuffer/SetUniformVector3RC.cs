using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    internal class SetUniformVector3RC : RenderCommand
    {
        public SetUniformVector3RC(int programID, in string uniformName,in Vector3 value)
        {
            m_UniformName = uniformName;
            m_Value = value;
            m_ProgramID = programID;
        }
        protected override void ExecuteImpl()
        {
            int location = GL.GetUniformLocation(m_ProgramID, m_UniformName);
            GL.Uniform3(location,ref m_Value);
        }


        private string m_UniformName;
        private Vector3 m_Value;
        private int m_ProgramID;
    }
}
