using OpenTK.Graphics.OpenGL4;
using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;

namespace Vex.Graphics
{
    /// <summary>
    /// Base class for all the framebuffers
    /// </summary>
    public abstract class Framebuffer : AssetObject
    {
        public Framebuffer()
        {
            m_FramebufferID = 0;
            m_Attachments = new List<FramebufferAttachment>();
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
        /// Returns the back texture
        /// </summary>
        public List<FramebufferAttachment> Attachments
        {
            get
            {
                return new List<FramebufferAttachment>(m_Attachments);
            }
            protected set
            {
                m_Attachments = value;
            }
        }

        /// <summary>
        /// The depth texture of this framebuffer
        /// </summary>
        public Texture2D DepthTexture
        {
            get
            {
                return m_DetphTexture;
            }
            protected set
            {
                m_DetphTexture = value;
            }
        }

        /// <summary>
        /// Whether this framebuffer has depth texture or not
        /// </summary>
        public bool HasDepthTexture
        {
            get
            {
                return m_HasDepthTexture;
            }
            protected set
            {
                m_HasDepthTexture = true;
            }
        }

        protected List<FramebufferAttachmentCreateParams> AttachmentCreateParameters
        {
            get
            {
                return m_AttachmentCreateParameters;
            }
            set
            {
                m_AttachmentCreateParameters = value;
            }
        }

        public bool IsDestroyed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        
        public override void Destroy()
        {
            /*
             * Delete back texture
             */
            DestroyAttachments();

            /*
             * Delete depth texture
             */
            m_DetphTexture?.Destroy();
            m_DetphTexture = null;

            /*
             * Delete framebuffer
             */
            GL.DeleteFramebuffer(m_FramebufferID);
        }

        /// <summary>
        /// Destroys and frees all the framebuffer attachments
        /// </summary>
        protected void DestroyAttachments()
        {
            foreach (FramebufferAttachment attachment in m_Attachments)
                attachment.DestoryAttachment();
            m_Attachments.Clear();
        }

        private List<FramebufferAttachment> m_Attachments;
        private List<FramebufferAttachmentCreateParams> m_AttachmentCreateParameters;
        private Texture2D m_DetphTexture;
        private uint m_FramebufferID;
        private bool m_HasDepthTexture;
    }
}
