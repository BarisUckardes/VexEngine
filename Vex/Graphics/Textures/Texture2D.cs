using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        public static Texture2D LoadTextureFromPath(string path,bool flipVertically = false)
        {
            /*
             * Validate path
             */
            if (!File.Exists(path))
                return null;

            /*
             * Load image
             */
            IImageFormat format;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Image<Rgba32> image = Image.Load<Rgba32>(path, out format);
            sw.Stop();
            Console.WriteLine("Texture load from disk took: " + sw.ElapsedMilliseconds.ToString());
            sw.Restart();

            /*
             * Flip vertical
             */
            if (flipVertically)
            {
                //image.Mutate(x => x.Flip(FlipMode.Vertical));
            }

            /*
             * Initialize pixel array
             */
            Rgba32[] pixels = new Rgba32[image.Width * image.Height];

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    pixels[x + y * image.Width] = image[x,y];
                }
            }

            sw.Stop();
            Console.WriteLine("Texture data mutation took " + sw.ElapsedMilliseconds.ToString());
            sw.Restart();

            /*
             * Create texture
             */
            Texture2D texture = new Texture2D(image.Width, image.Height,
                format.DefaultMimeType.GetAsTextureFormat(image.PixelType.AlphaRepresentation == null),
                format.DefaultMimeType.GetAsTextureInternalFormat(image.PixelType.BitsPerPixel, image.PixelType.AlphaRepresentation == PixelAlphaRepresentation.Associated),TextureDataType.UnsignedByte);

            texture.SetData(pixels,true);
            sw.Stop();
            Console.WriteLine("Texture gpu upload took " + sw.ElapsedMilliseconds.ToString());

            /*
             * Dispose image data
             */
            image.Dispose();

            return texture;
        }

        public Texture2D(int width, int height, TextureFormat format, TextureInternalFormat internalFormat,TextureDataType dataType,TextureWrapMode wrapS = TextureWrapMode.Repeat, TextureWrapMode wrapT = TextureWrapMode.Repeat, TextureMinFilter minFilter = TextureMinFilter.Nearest, TextureMagFilter magFilter = TextureMagFilter.Nearest)
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
            GL.TexImage2D(TextureTarget.Texture2D,0, (PixelInternalFormat)internalFormat, width, height, 0, (PixelFormat)format, (PixelType)dataType, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapT);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

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
            DataType = dataType;
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
        public void SetData<TData>(TData[] data,bool generateMipmaps) where TData :struct
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
             * Set texture parameters
             */
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, generateMipmaps ? (int)TextureMinFilter.LinearMipmapLinear : (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            /*
             * Unbind texture
             */
            GL.BindTexture(TextureTarget.Texture2D, 0);

            /*
             * Set cpu data
             */
            CpuData = data as byte[];
        }
        public void SetData(IntPtr dataPtr, bool generateMipmaps)
        {
            /*
             * Bind texture
             */
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            /*
             * Set data
             */
            GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)InternalFormat, m_Width, m_Height, 0, (PixelFormat)Format, PixelType.UnsignedByte, dataPtr);

            /*
             * Set texture parameters
             */
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, generateMipmaps ? (int)TextureMinFilter.LinearMipmapLinear : (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            /*
             * Unbind texture
             */
            GL.BindTexture(TextureTarget.Texture2D, 0);

            /*
             * Set cpu data
             */
            //CpuData = data as byte[];
        }

        internal void ResizeInternal(int width, int height)
        {
            /*
            * Bind texture
            */
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            /*
             * Set data
             */
            GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)InternalFormat, width, height, 0, (PixelFormat)Format, PixelType.UnsignedByte, IntPtr.Zero);

            /*
             * Set texture parameters
             */

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            /*
             * Unbind texture
             */
            GL.BindTexture(TextureTarget.Texture2D, 0);

            /*
             * Set dimensions
             */
            m_Width = width;
            m_Height = height;
        }

        private int m_Width;
        private int m_Height;
    }
}
