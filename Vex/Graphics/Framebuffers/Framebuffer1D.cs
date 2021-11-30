using OpenTK.Graphics.OpenGL4;
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
            CreateFramebuffer(new FramebufferAttachmentParams(width, 0, 0, format,internalFormat));
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

        protected override void CreateFramebufferImpl(FramebufferAttachmentParams attachmentParams)
        {
            /*
             * bind framebuffer
             */
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferID);

            /*
             * Create texture
             */
            Texture1D backTexture = new Texture1D(attachmentParams.Width, attachmentParams.Format, attachmentParams.InternalFormat);

            /*
             * Set attachment
             */
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, backTexture.Handle, 0);

            /*
             * Unbind framebuffer
             */
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            /*
             * Unbind texture
             */
            GL.BindTexture(TextureTarget.Texture1D, 0);

            /*
             * Set attachment
             */
            BackTexture = backTexture;
        }
        private int m_Width;
    }
}
