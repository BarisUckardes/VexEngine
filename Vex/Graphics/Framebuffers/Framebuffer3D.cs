using OpenTK.Graphics.OpenGL4;
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
        public Framebuffer3D(int width,int height,int depth,TextureFormat format,TextureInternalFormat internalFormat,TextureDataType dataType)
        {
            m_Width = width;
            m_Height = height;
            m_Depth = depth;
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

        //protected override void CreateFramebufferImpl(FramebufferAttachmentParams attachmentParams)
        //{

        //    /*
        //     * bind framebuffer
        //     */
        //    GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferID);

        //    /*
        //     * Create texture
        //     */
        //    Texture3D backTexture = new Texture3D(attachmentParams.Width, attachmentParams.Height, attachmentParams.Depth, attachmentParams.Format, attachmentParams.InternalFormat);

        //    /*
        //     * Set attachment
        //     */
        //    GL.FramebufferTexture3D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture3D, backTexture.Handle, 0, 0);

        //    /*
        //     * Unbind framebuffer
        //     */
        //    GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        //    /*
        //     * Unbind texture
        //     */
        //    GL.BindTexture(TextureTarget.Texture3D, 0);

        //    /*
        //     * Set attachment
        //     */
        //    BackTexture = backTexture;
        //}
        private int m_Width;
        private int m_Height;
        private int m_Depth;
    }
}
