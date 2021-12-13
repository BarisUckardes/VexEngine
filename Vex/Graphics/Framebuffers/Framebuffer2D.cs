using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// 2D variant of the framebuffer class
    /// </summary>
    public sealed class Framebuffer2D : Framebuffer
    {
        /// <summary>
        /// Intermediate framebuffer
        /// </summary>
        public static Framebuffer2D IntermediateFramebuffer
        {
            get
            {
                return s_IntermediateFramebuffer;
            }
            internal set
            {
                s_IntermediateFramebuffer = value;
            }
        }
        private static Framebuffer2D s_IntermediateFramebuffer;

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
        /// Internal constructor which creates framebuffer as swapchain framebuffer
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        internal Framebuffer2D(int width,int height)
        {
            /*
             * Set dimensions
             */
            m_Width = width;
            m_Height = height;

            /*
             * Set as swapchain
             */
            FramebufferID = 0;
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

        public void Resize(int width,int height)
        {
            BackTexture?.Destroy();
            BackTexture = null;
            DepthTexture?.Destroy();
            GL.DeleteFramebuffer(FramebufferID);
            CreateFramebuffer(new FramebufferAttachmentParams(width, height, 0, Format, InternalFormat));
        }

        public Vector4 GetPixelColor(int x,int y)
        {
            GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
            Vector4 colorValue = new Vector4(1, 1, 1, 1);
            int pixelX = m_Width - x;
            int pixelY = m_Height - y;
            Console.WriteLine($"Pixels: [{pixelX},{pixelY}]");
            GL.ReadPixels(pixelX,pixelY, 1, 1, (PixelFormat)Format, PixelType.Float, ref colorValue);
            return colorValue;
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

        internal void ResizeForSwapchainInternal(int width,int height)
        {
            m_Width = width;
            m_Height = height;
        }

        private int m_Width;
        private int m_Height;
    }
}
