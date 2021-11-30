﻿using OpenTK.Graphics.OpenGL4;
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
        /// Returns the internal texture format
        /// </summary>
        public TextureInternalFormat InternalFormat
        {
            get
            {
                return m_InternalFormat;
            }
            protected set
            {
                m_InternalFormat = value;
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
            protected set
            {
                m_BackTexture = value;
            }
        }

        public Texture DepthTexture
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



        public bool IsDestroyed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Creates a framebuffer and allocates space on gpu
        /// </summary>
        /// <param name="attachmentParams"></param>
        protected void CreateFramebuffer(in FramebufferAttachmentParams attachmentParams)
        {
            CreateFramebufferImpl(attachmentParams);
        }

        /// <summary>
        /// Framebuffer create implementation
        /// </summary>
        /// <param name="attachmentParams"></param>
        protected abstract void CreateFramebufferImpl(FramebufferAttachmentParams attachmentParams);

      
        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        private Texture m_BackTexture;
        private Texture m_DetphTexture;
        private TextureFormat m_Format;
        private TextureInternalFormat m_InternalFormat;
        private uint m_FramebufferID;
    }
}
