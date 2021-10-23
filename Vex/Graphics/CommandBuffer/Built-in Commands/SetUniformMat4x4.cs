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
    /// Set uniform matrix4x4 render command class
    /// </summary>
    public sealed class SetUniformMat4x4 : RenderCommand
    {
        public SetUniformMat4x4(int pVexgramID, string name, Matrix4 value, bool isTransposed = false)
        {
            m_Value = value;
            m_PVexgramID = pVexgramID;
            m_Name = name;
            m_Transposed = isTransposed;
        }

        protected override void ExecuteImpl()
        {
            int location = GL.GetUniformLocation(m_PVexgramID, m_Name);
            GL.UniformMatrix4(location,m_Transposed,ref m_Value);
        }

        private Matrix4 m_Value;
        private int m_PVexgramID;
        private string m_Name;
        private bool m_Transposed;
    }
}
