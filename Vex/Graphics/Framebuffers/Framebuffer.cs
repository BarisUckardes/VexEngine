using OpenTK.Graphics.OpenGL4;
using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Base class for all the framebuffers
    /// </summary>
    public abstract class Framebuffer : EngineObject,IDisposableGraphicsObject
    {
        public Framebuffer()
        {
            m_FramebufferID = 0;
        }

        /// <summary>
        /// Native framebuffer ID
        /// </summary>
        public uint FramebufferID
        {
            get
            {
                return m_FramebufferID;
            }
            protected set
            {
                m_FramebufferID = value;
            }
        }

        /// <summary>
        /// Is this framebuffer the swapchain's backbuffer?
        /// </summary>
        public bool IsSwapchain
        {
            get
            {
                return m_FramebufferID == 0;
            }
        }

        /// <summary>
        /// Returns the texture format
        /// </summary>
        public TextureFormat Format
        {
            get
            {
                return m_Format;
            }
            protected set
            {
                m_Format = value;
            }
        }

        /// <summary>
        /// Returns the back texture
        /// </summary>
        public Texture BackTexture
        {
            get
            {
                return m_BackTexture;
            }
        }

        public bool IsDisposed { get; set; }

        internal override void DestVexyInternal()
        {
            /*
             * Dipse framebuffer
             */
            Dispose();
        }
        public void Dispose()
        {
            /*
             * Delete frame buffer
             */
        }

        /// <summary>
        /// Creates a texture1D via attachment parameters
        /// </summary>
        /// <param name="backTextureAttachment"></param>
        protected void CreateAndAttachTexture1D(FramebufferAttachmentParams backTextureAttachment)
        {
            /*
             * bind framebuffer
             */
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_FramebufferID);

            /*
             * Create texture
             */
            Texture1D backTexture = new Texture1D(backTextureAttachment.Width, backTextureAttachment.Format);

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
            m_BackTexture = backTexture;
        }

        /// <summary>
        /// Creates a texture2D via attachment parameters
        /// </summary>
        /// <param name="backTextureAttachment"></param>
        protected void CreateAndAttachTexture2D(FramebufferAttachmentParams backTextureAttachment)
        {
            /*
             * bind framebuffer
             */
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_FramebufferID);

            /*
             * Create texture
             */
            Texture2D backTexture = new Texture2D(backTextureAttachment.Width, backTextureAttachment.Height, backTextureAttachment.Format);

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
            GL.BindTexture(TextureTarget.Texture2D, 0);

            /*
             * Set attachment
             */
            m_BackTexture = backTexture;
        }


        /// <summary>
        /// Creates a texture3D via attachment parameters
        /// </summary>
        /// <param name="backTextureAttachment"></param>
        protected void CreateAndAttachTexture3D(FramebufferAttachmentParams backTextureAttachment)
        {
            /*
             * bind framebuffer
             */
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_FramebufferID);

            /*
             * Create texture
             */
            Texture3D backTexture = new Texture3D(backTextureAttachment.Width, backTextureAttachment.Height,backTextureAttachment.Depth, backTextureAttachment.Format);

            /*
             * Set attachment
             */
            GL.FramebufferTexture3D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture3D, backTexture.Handle,0, 0);

            /*
             * Unbind framebuffer
             */
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            /*
             * Unbind texture
             */
            GL.BindTexture(TextureTarget.Texture3D, 0);

            /*
             * Set attachment
             */
            m_BackTexture = backTexture;
        }


        private Texture m_BackTexture;
        private TextureFormat m_Format;
        private uint m_FramebufferID;
    }
}
