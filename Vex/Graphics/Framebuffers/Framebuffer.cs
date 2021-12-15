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
        /// The data type of the one pixel in the framebuffer
        /// </summary>
        public TextureDataType DataType
        {
            get
            {
                return m_DataType;
            }
            protected set
            {
                m_DataType = value;
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

        /// <summary>
        /// The depth texture of this framebuffer
        /// </summary>
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

        public bool IsDestroyed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        
        public override void Destroy()
        {
            /*
             * Delete back texture
             */
            m_BackTexture?.Destroy();
            m_BackTexture = null;

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

        private Texture m_BackTexture;
        private Texture m_DetphTexture;
        private TextureFormat m_Format;
        private TextureInternalFormat m_InternalFormat;
        private TextureDataType m_DataType;
        private uint m_FramebufferID;
        private bool m_HasDepthTexture;
    }
}
