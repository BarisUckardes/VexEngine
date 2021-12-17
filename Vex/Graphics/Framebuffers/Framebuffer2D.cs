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

        public Framebuffer2D(int width,int height,in List<FramebufferAttachmentCreateParams> attachmentParams,bool hasDepthTexture)
        {
            m_Width = width;
            m_Height = height;
            HasDepthTexture = true;

            /*
             * Set attachment create parameters
             */
            AttachmentCreateParameters = attachmentParams;

            /*
             * Create texture
             */
            Invalidate();
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
            /*
             * Set new dimensions
             */
            m_Width = width;
            m_Height = height;

            /*
             * Invalidate
             */
            Invalidate();
        }

        public TValue GetPixelColor<TValue>(int attachmentIndex,int x,int y,TextureDataType dataType) where TValue : struct
        {
            /*
             * Set framebuffer read color attachment 0
             */
            GL.ReadBuffer(ReadBufferMode.ColorAttachment0 + attachmentIndex);

            /*
             * Create return value
             */
            TValue value = new TValue();

            /*
             * Mirror Y-Axis on openg
             */
            int pixelX = x;
            int pixelY = m_Height - y;

            /*
             * Try read
             */
            GL.ReadPixels(pixelX,pixelY, 1, 1, (PixelFormat)Attachments[0].Format,(PixelType)Attachments[0].DataType, ref value);
            return value;
        }
       
        private void Invalidate()
        {
            /*
             * Validate former framebuffer and back textures
             */
            DestroyAttachments();

            if (DepthTexture != null)
                DepthTexture.Destroy();
            DepthTexture = null;

            if(FramebufferID != 0)
                GL.DeleteFramebuffer(FramebufferID);

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
             * Create attachments
             */
            List<FramebufferAttachment> attachments = new List<FramebufferAttachment>();
            foreach(FramebufferAttachmentCreateParams attachmentParameters in AttachmentCreateParameters)
            {
                /*
                 * Create texture
                 */
                Texture2D colorAttachment = new Texture2D(Width, Height, attachmentParameters.Format, attachmentParameters.InternalFormat, attachmentParameters.DataType);
                colorAttachment.Name = attachmentParameters.Name;

                /*
                 * Create attachment
                 */
                FramebufferAttachment attachment = new FramebufferAttachment(colorAttachment,attachmentParameters.Format, attachmentParameters.InternalFormat, attachmentParameters.DataType, Width, Height,0);
                attachments.Add(attachment);
            }

            /*
             * Set attachment
             */
            List<DrawBuffersEnum> attachmentSlots = new List<DrawBuffersEnum>();
            for(int attachmentIndex = 0;attachmentIndex < attachments.Count; attachmentIndex++)
            {
                /*
                 * Get attachment
                 */
                FramebufferAttachment attachment = attachments[attachmentIndex];

                /*
                 * Get attachment slot
                 */
                OpenTK.Graphics.OpenGL4.FramebufferAttachment attachmentSlot = OpenTK.Graphics.OpenGL4.FramebufferAttachment.ColorAttachment0 + attachmentIndex;

                /*
                 * Register attachment slot
                 */
                attachmentSlots.Add(DrawBuffersEnum.ColorAttachment0 + attachmentIndex);

                /*
                 * Setup attachment
                 */
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachmentSlot, TextureTarget.Texture2D, attachment.Texture.Handle, 0);
            }

            /*
             * Specify buffers for drawing
             */
            GL.DrawBuffers(attachmentSlots.Count, attachmentSlots.ToArray());

            if(HasDepthTexture)
            {
                /*
                * Create depth texture
                */
                Texture2D depthTexture = new Texture2D(m_Width, m_Height, TextureFormat.DepthComponent, TextureInternalFormat.DepthComponent, TextureDataType.Float);

                /*
                 * Set depth buffer
                 */
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, OpenTK.Graphics.OpenGL4.FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthTexture.Handle, 0);

                /*
                 * Set depth attachment
                */
                DepthTexture = depthTexture;
            }

            /*
             * Unbind framebuffer
             */
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            /*
             * Set framebuffer id
             */
            FramebufferID = framebufferID;

            /*
             * Set attachments
             */
            Attachments = attachments;
        }

        private int m_Width;
        private int m_Height;
    }
}
