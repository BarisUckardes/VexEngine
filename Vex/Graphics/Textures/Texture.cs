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
    /// Base texture class for all textures
    /// </summary>
    public abstract class Texture : AssetObject
    {
       

        /// <summary>
        /// Returns the cpu side data
        /// </summary>
        public byte[] CpuData
        {
            get
            {
                return m_CpuData;
            }
            protected set
            {
                m_CpuData = value;
            }
        }

        /// <summary>
        /// Returns the format
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
        /// Returns the graphics api handle
        /// </summary>
        public uint Handle
        {
            get
            {
                return m_Handle;
            }
            protected set
            {
                m_Handle = value;
            }
        }

        public bool IsDestroyed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        private byte[] m_CpuData;
        private TextureFormat m_Format;
        private TextureInternalFormat m_InternalFormat;
        private uint m_Handle;
    }
}
