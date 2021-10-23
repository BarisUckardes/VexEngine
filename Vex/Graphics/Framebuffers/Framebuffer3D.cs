using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Framebuffer class for encapsulating Framebuffer3D operations
    /// </summary>
    public sealed class Framebuffer3D : Framebuffer
    {
        public Framebuffer3D(int width,int height,int depth,TextureFormat format)
        {
            m_Width = width;
            m_Height = height;
            m_Depth = depth;
            Format = format;
            CreateAndAttachTexture3D(new FramebufferAttachmentParams(width, height, depth, format));
        }


        /// <summary>
        /// Returns the framebuffer width
        /// </summary>
        public int Width
        {
            get
            {
                return m_Width;
            }
        }

        /// <summary>
        /// Returns the framebuffer height
        /// </summary>
        public int Height
        {
            get
            {
                return m_Height;
            }
        }

        /// <summary>
        /// Returns the framebuffer depth
        /// </summary>
        public int Depth
        {
            get
            {
                return m_Depth;
            }
        }


        private int m_Width;
        private int m_Height;
        private int m_Depth;
    }
}
