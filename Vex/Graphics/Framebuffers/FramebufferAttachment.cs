using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    public sealed class FramebufferAttachment
    {
        public FramebufferAttachment(Texture2D texture, TextureFormat format, TextureInternalFormat internalFormat, TextureDataType dataType, int width, int height, int depth)
        {
            m_Texture = texture;
            m_Format = format;
            m_InternalFormat = internalFormat;
            m_DataType = dataType;
            m_Width = width;
            m_Height = height;
            m_Depth = depth;
        }

        public Texture2D Texture
        {
            get
            {
                return m_Texture;
            }
        }

        public TextureFormat Format
        {
            get
            {
                return m_Format;
            }
        }

        public TextureInternalFormat InternalFormat
        {
            get
            {
                return m_InternalFormat;
            }
        }

        public TextureDataType DataType
        {
            get
            {
                return m_DataType;
            }
        }

        public int Width
        {
            get
            {
                return m_Width;
            }
        }

        public int Height
        {
            get
            {
                return m_Height;
            }
        }
        public int Depth
        {
            get
            {
                return m_Depth;
            }
        }

        public void DestoryAttachment()
        {
            m_Texture?.Destroy();
            m_Texture = null;
        }
        private Texture2D m_Texture;
        private TextureFormat m_Format;
        private TextureInternalFormat m_InternalFormat;
        private TextureDataType m_DataType;
        private int m_Width;
        private int m_Height;
        private int m_Depth;
    }
}
