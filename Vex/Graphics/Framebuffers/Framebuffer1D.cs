using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Framebuffer class for encapsualting Framebuffer1D operations
    /// </summary>
    public sealed class Framebuffer1D : Framebuffer
    {
        public Framebuffer1D(int width,TextureFormat format,TextureInternalFormat internalFormat)
        {
            /*
             * Set local fields
             */
            m_Width = width;
            Format = format;

            /*
             * Create framebuffer
             */
            CreateAndAttachTexture1D(new FramebufferAttachmentParams(width, 0, 0, format,internalFormat));
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
        private int m_Width;
    }
}
