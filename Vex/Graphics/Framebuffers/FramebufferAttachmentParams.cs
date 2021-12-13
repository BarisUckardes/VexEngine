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
    public readonly struct FramebufferAttachmentParams
    {
        public FramebufferAttachmentParams(int width, int height, int depth, TextureFormat format,TextureInternalFormat internalFormat,TextureDataType dataType)
        {
            Width = width;
            Height = height;
            Depth = depth;
            Format = format;
            InternalFormat = internalFormat;
            DataType = dataType;
        }

        /// <summary>
        /// Target width
        /// </summary>
        public readonly int Width;

        /// <summary>
        /// Target height
        /// </summary>
        public readonly int Height;

        /// <summary>
        /// Target depth
        /// </summary>
        public readonly int Depth;

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
