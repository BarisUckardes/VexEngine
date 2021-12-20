using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Set uniform float render command class
    /// </summary>
    public sealed class SetUniformFloatRC : RenderCommand
    {
        public SetUniformFloatRC(in int programId,in string name,in float value)
        {
            m_Name = name;
            m_ProgramID = programId;
            m_Value = value;
        }
        protected override void ExecuteImpl()
        {
            int location = GL.GetUniformLocation(m_ProgramID, m_Name);
            GL.Uniform1(location, m_Value);
        }

        private int m_ProgramID;
        private float m_Value;
        private string m_Name;
    }
}
