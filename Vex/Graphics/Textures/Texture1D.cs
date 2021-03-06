using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Texture class for encapsulating Texture1D operations
    /// </summary>
    public sealed class Texture1D : Texture
    {
        public Texture1D(int width,TextureFormat format,TextureInternalFormat internalFormat)
        {
            /*
             * Create texture
             */
            uint handle;
            GL.GenTextures(1, out handle);

            /*
             * Bind texture
             */
            GL.BindTexture(TextureTarget.Texture1D, handle);

            /*
             * Set empty data
             */
            GL.TexImage1D(TextureTarget.Texture1D, 0,
                (PixelInternalFormat)internalFormat,
                width, 0,
                (PixelFormat)format, PixelType.UnsignedByte,
                IntPtr.Zero);

            /*
             * Unbind texture
             */
            GL.BindTexture(TextureTarget.Texture1D, 0);


            /*
             * Set handle
             */
            Handle = handle;

            /*
             * Set local field
             */
            m_Width = width;
            Format = format;
            InternalFormat = internalFormat;
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
        /// Set texture data via byte array
        /// </summary>
        /// <param name="data"></param>
        public void SetData(byte[] data)
        {
            /*
             * Bind texture
             */
            GL.BindTexture(TextureTarget.Texture1D, Handle);

            /*
             * Set data
             */
            GL.TexImage1D(TextureTarget.Texture1D, 0,(PixelInternalFormat)InternalFormat, m_Width, 0, (PixelFormat)Format, PixelType.UnsignedByte, data);

            /*
             * Unbind texture
             */
            GL.BindTexture(TextureTarget.Texture1D, 0);

            /*
             * Set cpu data
             */
            CpuData = data;
        }
        private int m_Width;
    }
}
