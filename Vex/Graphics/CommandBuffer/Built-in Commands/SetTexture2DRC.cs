using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Set texture2D render command class
    /// </summary>
    public sealed class SetTexture2DRC : RenderCommand
    {
        public SetTexture2DRC(Texture2D texture,ShaderProgram program,string name,int unit)
        {
            m_Handle = texture == null ? 0 : (int)texture.Handle;
            m_Unit = unit;
            m_Location = GL.GetUniformLocation(program.Handle,name);
        }
        protected override void ExecuteImpl()
        {
            GL.ActiveTexture(TextureUnit.Texture0 + m_Unit);
            GL.BindTexture(TextureTarget.Texture2D,m_Handle);
            GL.Uniform1(m_Location, m_Unit);

            
        }

        private int m_Handle;
        private int m_Unit;
        private int m_Location;
    }
}
