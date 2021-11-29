using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Set shader pVexgram render command class
    /// </summary>
    public sealed class SetShaderProgramRC : RenderCommand
    {

        public SetShaderProgramRC(in ShaderProgram program)
        {
            m_Program = program != null ? program : null;
        }
        protected override void ExecuteImpl()
        {
            GL.UseProgram(m_Program != null ? m_Program.Handle : 0);
           // Console.WriteLine("Program used: " + m_Program.Handle);
        }


        private ShaderProgram m_Program;
    }
}
