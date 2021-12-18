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
            List<float> floats = new List<float>();
            foreach(Vector4 vector in value)
            {
                floats.Add(vector.X);
                floats.Add(vector.Y);
                floats.Add(vector.Z);
                floats.Add(vector.W);
            }
            m_Value = floats.ToArray();
            m_ProgramID = programID;
        }
        protected override void ExecuteImpl()
        {
            int location = GL.GetUniformLocation(m_ProgramID, m_UniformName);
            GL.Uniform4(location,m_Value.Length,m_Value);
        }


        private string m_UniformName;
        private float[] m_Value;
        private int m_ProgramID;
    }
}

