using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using Vex.Extensions;
namespace Vex.Graphics
{
    public sealed class CubeTexture : AssetObject
    {
       
        public int FaceWidth
        {
            get
            {
                return m_FaceWidth;
            }
        }
        public int FaceHeight
        {
            get
            {
                return m_FaceHeight;
            }
        }
        public int Handle
        {
            get
            {
                return m_Handle;
            }
        }
        public void LoadFromFile(List<string> absolutePaths)
        {
            /*
             * Create new cubemap id
             */
            int cubemapID = GL.GenTexture();

            /*
             * Bind cube map
             */
            GL.BindTexture(TextureTarget.TextureCubeMap, cubemapID);

            /*
             * Iterate cubemap faces and load them
             */
            for (int faceIndex = 0; faceIndex < absolutePaths.Count; faceIndex++)
            {
                /*
                 * Get path
                 */
                string path = absolutePaths[faceIndex];

                /*
                * Load image
                */
                IImageFormat format;
                Image<Rgba32> image = Image.Load<Rgba32>(path, out format);
                Console.WriteLine("Image loaded: " + image.Width + " " + image.Height);
                /*
                 * Get as byte array
                 */
                List<byte> pixels = new List<byte>(3 * image.Width * image.Height);

                for (int y = 0; y < image.Height; y++)
                {
                    Span<Rgba32> RowSpan = image.GetPixelRowSpan(y);
                    for (int x = 0; x < image.Width; x++)
                    {
                        pixels.Add(RowSpan[x].R);
                        pixels.Add(RowSpan[x].G);
                        pixels.Add(RowSpan[x].B);
                        //pixels.Add(RowSpan[x].A);
                    }
                }

                /*
                 * Set cubemap texture
                 */
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + faceIndex,
                    0,
                    PixelInternalFormat.Rgb,
                    image.Width, image.Height,
                    0,
                    PixelFormat.Rgb,
                    (PixelType)TextureDataType.UnsignedByte,
                    pixels.ToArray());
                Console.WriteLine("Set bytes: " + pixels.Count);
            }

            /*
             * Set cubemap parameters
             */
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS,     (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            /*
             * Unbind cubemap
             */
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);

            m_Handle = cubemapID;
        }
        public override void Destroy()
        {

        }
        private int m_FaceWidth;
        private int m_FaceHeight;
        private int m_Handle;
    }
}
