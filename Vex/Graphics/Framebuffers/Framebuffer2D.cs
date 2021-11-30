using OpenTK.Graphics.OpenGL4;
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
            InternalFormat = internalFormat;

            /*
             * Create texture
             */
            CreateFramebuffer(new FramebufferAttachmentParams(width, height, 0, format, internalFormat));
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

        protected override void CreateFramebufferImpl(FramebufferAttachmentParams attachmentParams)
        {
            /*
             * Creat framebuffer
             */
            uint framebufferID = 0;
            GL.GenFramebuffers(1, out framebufferID);

            /*
             * bind framebuffer
             */
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebufferID);

            /*
             * Create texture
             */
            Texture2D backTexture = new Texture2D(attachmentParams.Width, attachmentParams.Height, attachmentParams.Format, attachmentParams.InternalFormat);

            /*
             * Set attachment
             */
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, backTexture.Handle, 0);

            /*
             * Set depth render buffer
             */
            int renderBufferID;
            GL.GenRenderbuffers(1, out renderBufferID);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderBufferID);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, attachmentParams.Width, attachmentParams.Height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, renderBufferID);

            /*
             * Unbind framebuffer
             */
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            /*
             * Unbind texture
             */
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

            /*
             * Set attachment
             */
            BackTexture = backTexture;

            /*
             * Set framebuffer id
             */
            FramebufferID = framebufferID;
        }

        private int m_Width;
        private int m_Height;
    }
}
