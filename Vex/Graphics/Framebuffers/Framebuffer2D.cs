using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// 2D variant of the framebuffer class
    /// </summary>
    public sealed class Framebuffer2D : Framebuffer
    {
        public Framebuffer2D(int width,int height,TextureFormat format,TextureInternalFormat internalFormat)
        {
            m_Width = width;
            m_Height = height;
            Format = format;
            CreateAndAttachTexture2D(new FramebufferAttachmentParams(width, height, 0, format, internalFormat));
        }

        public Framebuffer2D()
        {

        }

        /// <summary>
        /// Returns the width of this framebuffer
        /// </summary>

        public int Width
        {
            get
            {
                return m_Width;
            }
        }

        /// <summary>
        /// Returns the height of this framebuffer
        /// </summary>
        public int Height
        {
            get
            {
                return m_Height;
            }
        }

        private int m_Width;
        private int m_Height;
    }
}
