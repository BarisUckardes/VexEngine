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
    public sealed class SetShaderPVexgramRC : RenderCommand
    {

        public SetShaderPVexgramRC(in ShaderPVexgram pVexgram)
        {
            m_Program = pVexgram;
        }
        protected override void ExecuteImpl()
        {
            GL.UseProgram(m_Program.Handle);
        }


        private ShaderPVexgram m_Program;
    }
}
