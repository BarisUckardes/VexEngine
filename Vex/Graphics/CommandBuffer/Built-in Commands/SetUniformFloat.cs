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
    public sealed class SetUniformFloat : RenderCommand
    {
        public SetUniformFloat(in int pVexgramID,in string name,in float value)
        {
            m_Name = name;
            m_PVexgramID = pVexgramID;
            m_Value = value;
        }
        protected override void ExecuteImpl()
        {
            int location = GL.GetUniformLocation(m_PVexgramID, m_Name);
            GL.Uniform1(location, m_Value);
        }

        private int m_PVexgramID;
        private float m_Value;
        private string m_Name;
    }
}
