using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    
    /// <summary>
    /// Texture class for encapsulating Texture3D operations
    /// </summary>
    public sealed class Texture3D : Texture
    {
        public Texture3D(int width,int height,int depth,TextureFormat format)
        {
            /*
             * Create texture
             */
            uint handle = 0;
            GL.GenTextures(1, out handle);

            /*
             * Bind texture
             */
            GL.BindTexture(TextureTarget.Texture3D, handle);

            /*
             * Set empty data
             */
            GL.TexImage3D(TextureTarget.Texture3D, 0,
                TextureInternalFormatUtils.GetNative(TextureInternalFormatUtils.GetInternalFormatViaFormat(format)),
                width, height, depth,
                0,
                TextureFormatUtils.GetNative(format),
                PixelType.UnsignedByte,
                IntPtr.Zero);

            /*
             * Set texture parameters
             */
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            /*
             * Unbind texture
             */
            GL.BindTexture(TextureTarget.Texture3D, 0);

            /*
             * Set handle
             */
            Handle = handle;

            /*
             * Set local fields
             */
            m_Width = width;
            m_Height = height;
            m_Depth = depth;
            Format = format;
            InternalFormat = TextureInternalFormatUtils.GetInternalFormatViaFormat(format);

        }

        /// <summary>
        /// Returns the width of this texture
        /// </summary>
        public int Width
        {
            get
            {
                return m_Width;
            }
        }

        /// <summary>
        /// Returns the height of this texture
        /// </summary>
        public int Height
        {
            get
            {
                return m_Height;
            }
        }

        /// <summary>
        /// Returns the depth of this texture
        /// </summary>
        public int Depth
        {
            get
            {
                return m_Depth;
            }
        }

        /// <summary>
        /// Set texture data via byte array
        /// </summary>
        /// <param name="data"></param>
        public void SetData(byte[] data)
        {
            /*
             * Bind texture
             */
            GL.BindTexture(TextureTarget.Texture3D, Handle);

            /*
             * Set data
             */
            GL.TexImage3D(TextureTarget.Texture3D, 0, TextureInternalFormatUtils.GetNative(TextureInternalFormatUtils.GetInternalFormatViaFormat(Format)), m_Width, m_Height,m_Depth, 0, TextureFormatUtils.GetNative(Format), PixelType.UnsignedByte, data);

            /*
             * Unbind texture
             */
            GL.BindTexture(TextureTarget.Texture3D, 0);

            /*
             * Set cpu data
             */
            CpuData = data;
        }

        private int m_Width;
        private int m_Height;
        private int m_Depth;
    }
}
