using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    internal class SetUniformVector2RC : RenderCommand
    {
        public SetUniformVector2RC(int programID, in string uniformName, in Vector2 value)
        {
            m_UniformName = uniformName;
            m_Value = value;
            m_ProgramID = programID;
        }
        protected override void ExecuteImpl()
        {
            int location = GL.GetUniformLocation(m_ProgramID, m_UniformName);
            GL.Uniform2(location, ref m_Value);
        }

        private string m_UniformName;
        private Vector2 m_Value;
        private int m_ProgramID;
    }
}
