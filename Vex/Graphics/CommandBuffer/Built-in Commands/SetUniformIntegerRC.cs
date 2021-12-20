using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    public sealed class SetUniformIntegerRC : RenderCommand
    {
        public SetUniformIntegerRC(int programID,int value,string name)
        {
            m_ProgramID = programID;
            m_Value = value;
            m_Name = name;
        }
        protected override void ExecuteImpl()
        {
            int location = GL.GetUniformLocation(m_ProgramID, m_Name);
            GL.Uniform1(location, m_Value);
        }

        private int m_ProgramID;
        private int m_Value;
        private string m_Name;
    }
}
