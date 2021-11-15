using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Vex.Extensions;
namespace Vex.Graphics
{
    /// <summary>
    /// Texture class for encapsulating Texture2D operations
    /// </summary>
    public sealed class Texture2D : Texture
    {
        public static Texture2D LoadTextureFromPath(string path, bool flipVertically = true)
        {
            /*
             * Load image
             */
            IImageFormat format;
            Image<Rgba32> image = Image.Load<Rgba32>(path, out format);

            /*
             * Flip vertical
             */
            if (flipVertically)
            {
                image.Mutate(x => x.Flip(FlipMode.Vertical));
            }

            /*
             * Get as byte array
             */
            List<byte> pixels = new List<byte>(4 * image.Width * image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                Span<Rgba32> RowSpan = image.GetPixelRowSpan(y);
                for (int x = 0; x < image.Width; x++)
                {
                    pixels.Add(RowSpan[x].R);
                    pixels.Add(RowSpan[x].G);
                    pixels.Add(RowSpan[x].B);
                    pixels.Add(RowSpan[x].A);
                }
            }

            /*
             * Create texture
             */
            Texture2D texture = new Texture2D(image.Width, image.Height,
                format.DefaultMimeType.GetAsTextureFormat(),
                format.DefaultMimeType.GetAsTextureInternalFormat(image.PixelType.BitsPerPixel));

            texture.SetData(pixels.ToArray());

            /*
             * Dispose image data
             */
            image.Dispose();

            return texture;
        }

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
            GL.TexImage2D(TextureTarget.Texture2D, 0,(PixelInternalFormat)internalFormat, width, height, 0, (PixelFormat)format, PixelType.UnsignedByte, IntPtr.Zero);

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
            InternalFormat =internalFormat;
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
            GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)InternalFormat, m_Width, m_Height, 0, (PixelFormat)Format, PixelType.UnsignedByte, data);

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
