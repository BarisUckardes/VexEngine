using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// A sturct used for creating framebuffer attachments
    /// </summary>
    public readonly struct FramebufferAttachmentCreateParams
    {
        public FramebufferAttachmentCreateParams(string name,TextureFormat format,TextureInternalFormat internalFormat,TextureDataType dataType)
        {
            Name = name;
            Format = format;
            InternalFormat = internalFormat;
            DataType = dataType;
        }

        /// <summary>
        /// Name of the texture
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Target texture format
        /// </summary>
        public readonly TextureFormat Format;

        /// <summary>
        /// Target texture internal format
        /// </summary>
        public readonly TextureInternalFormat InternalFormat;

        /// <summary>
        /// Target texture data type
        /// </summary>
        public readonly TextureDataType DataType;

    }
}
