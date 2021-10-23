using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Utility class for texture format conversions
    /// </summary>
    public static class TextureFormatUtils
    {
        /// <summary>
        /// Returns the native pixel format via texture format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static PixelFormat GetNative(TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.RGB:
                    return PixelFormat.Rgb;
                    break;
                case TextureFormat.RGBA:
                    return PixelFormat.Rgba;
                    break;
                case TextureFormat.DepthComponent:
                    return PixelFormat.DepthComponent;
                    break;
                case TextureFormat.UnsignedShort:
                    return PixelFormat.UnsignedShort;
                    break;
                case TextureFormat.UnsignedInt:
                    return PixelFormat.UnsignedInt;
                    break;
                case TextureFormat.ColorIndex:
                    return PixelFormat.ColorIndex;
                    break;
                case TextureFormat.Alpha:
                    return PixelFormat.Alpha;
                    break;
                default:
                    break;
            }

            return PixelFormat.Rgb;
        }

        /// <summary>
        /// Returns the texture format via mime 
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public static TextureFormat GetFormatViaMime(string mime)
        {
            switch(mime)
            {
                case "image/jpeg":
                {
                   return TextureFormat.RGBA;
                   break;
                }
                case "image/png":
                {
                   return TextureFormat.RGBA;
                }

            }

            return TextureFormat.RGB;
        }

        

    }
}
