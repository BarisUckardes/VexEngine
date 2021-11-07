using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Texture class for encapsulating Texture2D operations
    /// </summary>
    public sealed class Texture2D : Texture
    {
        public Texture2D(int width,int height,TextureFormat format,TextureInternalFormat internalFormat)
        {
            /*
             * Create texture
             */
            uint handle;
            GL.GenTextures(1, out handle);

            /*
             * Bind texture
             */
            GL.BindTexture(TextureTarget.Texture2D, handle);

            /*
             * Set empty data
             */
            GL.TexImage2D(TextureTarget.Texture2D, 0,TextureInternalFormatUtils.GetNative(internalFormat), width, height, 0, TextureFormatUtils.GetNative(format), PixelType.UnsignedByte, IntPtr.Zero);

            /*
             * Set texture parameters
             */
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            
            GL.BindTexture(TextureTarget.Texture2D, 0);

            /*
             * Set handle
             */
            Handle = handle;

            /*
             * Set loca field
             */
            m_Width = width;
            m_Height = height;
            Format = format;
            InternalFormat =TextureInternalFormatUtils.GetInternalFormatViaFormat(format);
        }

        /// <summary>
        /// Returns the width of this class
        /// </summary>
        public int Width
        {
            get
            {
                return m_Width;
            }
        }

        /// <summary>
        /// Returns the height of this class
        /// </summary>
        public int Height
        {
            get
            {
                return m_Height;
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
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            /*
             * Set data
             */
            GL.TexImage2D(TextureTarget.Texture2D, 0, TextureInternalFormatUtils.GetNative(InternalFormat), m_Width, m_Height, 0, TextureFormatUtils.GetNative(Format), PixelType.UnsignedByte, data);

            /*
             * Unbind texture
             */
            GL.BindTexture(TextureTarget.Texture2D, 0);

            /*
             * Set cpu data
             */
            CpuData = data;
        }

        private int m_Width;
        private int m_Height;
    }
}
