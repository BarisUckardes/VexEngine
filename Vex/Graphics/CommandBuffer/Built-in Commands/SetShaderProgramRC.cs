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

        public SetShaderProgramRC(in ShaderProgram pVexgram)
        {
            m_Program = pVexgram;
        }
        protected override void ExecuteImpl()
        {
            GL.UseProgram(m_Program.Handle);
        }


        private ShaderProgram m_Program;
    }
}
