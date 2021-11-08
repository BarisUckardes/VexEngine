using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Utility class for internal format conversions
    /// </summary>
    public static class TextureInternalFormatUtils
    {
        /// <summary>
        /// Returns the native internal pixel format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static PixelInternalFormat GetNative(TextureInternalFormat format)
        {
            switch (format)
            {
                case TextureInternalFormat.DepthComponent:
                    return PixelInternalFormat.DepthComponent;
                    break;
                case TextureInternalFormat.Alpha:
                    return PixelInternalFormat.Alpha;
                    break;
                case TextureInternalFormat.RGB:
                    return PixelInternalFormat.Rgb;
                    break;
                case TextureInternalFormat.RGBA:
                    return PixelInternalFormat.Rgba;
                    break;
                case TextureInternalFormat.RGB8:
                    return PixelInternalFormat.Rgb8;
                    break;
                case TextureInternalFormat.RGB16:
                    return PixelInternalFormat.Rgb16;
                    break;
                case TextureInternalFormat.RGB32:
                    return PixelInternalFormat.Rgb32f;
                    break;
                default:
                    return PixelInternalFormat.Rgb;
                    break;
            }


        }

        /// <summary>
        /// Returns the internal texture format via mime
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public static TextureInternalFormat GetInternalFormatViaMime(string mime,int bits)
        {
            if (mime == "image/jpeg" && bits == 8)
            {
                switch (bits)
                {
                    case 8:
                        {
                            return TextureInternalFormat.RGB8;
                            break;
                        }
                    case 16:
                        {
                            return TextureInternalFormat.RGB16;
                            break;
                        }
                    case 32:
                        {
                            return TextureInternalFormat.RGB32;
                            break;
                        }
                }
            }
            else if(mime == "image/png")
            {
                switch (bits)
                {
                    case 8:
                        {
                            return TextureInternalFormat.RGBA;
                            break;
                        }
                    case 16:
                        {
                            return TextureInternalFormat.RGBA;
                            break;
                        }
                    case 32:
                        {
                            return TextureInternalFormat.RGBA;
                            break;
                        }
                }
            }

            return TextureInternalFormat.RGB8;
        }


        /// <summary>
        /// Returns the internal texture format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static TextureInternalFormat GetInternalFormatViaFormat(TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.RGB:
                    return TextureInternalFormat.RGB;
                    break;
                case TextureFormat.RGBA:
                    return TextureInternalFormat.RGBA;
                    break;
                case TextureFormat.DepthComponent:
                    return TextureInternalFormat.DepthComponent;
                    break;
                case TextureFormat.UnsignedShort:
                    return TextureInternalFormat.RGB16;
                    break;
                case TextureFormat.UnsignedInt:
                    return TextureInternalFormat.RGB16;
                    break;
                case TextureFormat.ColorIndex:
                    return TextureInternalFormat.RGB;
                    break;
                case TextureFormat.Alpha:
                    return TextureInternalFormat.Alpha;
                    break;
                default:
                    return TextureInternalFormat.RGB;
                    break;
            }
        }
    }
}
