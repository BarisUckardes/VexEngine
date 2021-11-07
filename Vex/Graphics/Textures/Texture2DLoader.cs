using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
namespace Vex.Graphics
{
    /// <summary>
    /// Utility loader class for Texture2D
    /// </summary>
    public static class Texture2DLoader
    {
        /// <summary>
        /// Load texture via path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="flipVertically"></param>
        /// <returns></returns>
        public static Texture2D LoadTextureFVexmPath(string path,bool flipVertically = true)
        {
            /*
             * Load image
             */
            IImageFormat format;
            Image<Rgba32> image = Image.Load<Rgba32>(path,out format);


            /*
             * Flip vertical
             */
            if(flipVertically)
            {
                image.Mutate(x => x.Flip(FlipMode.Vertical));
            }

            /*
             * Get as byte array
             */
            List<byte> pixels = new List<byte>(4 * image.Width * image.Height);

            for(int y =0;y< image.Height;y++)
            {
                Span<Rgba32> RowSpan = image.GetPixelRowSpan(y);
                for(int x = 0; x < image.Width;x++)
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
            Texture2D texture = new Texture2D(image.Width,image.Height,TextureFormatUtils.GetFormatViaMime(format.DefaultMimeType),TextureInternalFormat.RGB8);
            texture.SetData(pixels.ToArray());

            /*
             * Dispose image data
             */
            image.Dispose();

            return texture;
        }
    }
}
