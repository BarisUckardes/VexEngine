using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;
namespace Vex.Extensions
{
    public static class TextureFormatExtensions
    {
        public static TextureFormat GetAsTextureFormat(this string text)
        {
            switch (text)
            {
                case "image/jpeg":
                    {
                        return TextureFormat.Rgb;
                    }
                case "image/png":
                    {
                        return TextureFormat.Rgba;
                    }
            }

            return TextureFormat.None;
        }
        public static TextureInternalFormat GetAsTextureInternalFormat(this string text,int bits)
        {
            if (text == "image/jpeg" && bits == 8)
            {
                switch (bits)
                {
                    case 8:
                        {
                            return TextureInternalFormat.Rgb8;
                            break;
                        }
                    case 16:
                        {
                            return TextureInternalFormat.Rgb16f;
                            break;
                        }
                    case 32:
                        {
                            return TextureInternalFormat.Rgb32f;
                            break;
                        }
                }
            }
            else if (text == "image/png")
            {
                switch (bits)
                {
                    case 8:
                        {
                            return TextureInternalFormat.Rgba8;
                            break;
                        }
                    case 16:
                        {
                            return TextureInternalFormat.Rgba16f;
                            break;
                        }
                    case 32:
                        {
                            return TextureInternalFormat.Rgba32f;
                            break;
                        }
                }
            }

            return TextureInternalFormat.None;
        }
    }
}
