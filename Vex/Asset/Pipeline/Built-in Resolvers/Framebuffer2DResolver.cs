using Vex.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using Vex.Framework;

namespace Vex.Asset
{
    public sealed class Framebuffer2DResolver : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(Framebuffer2D);
            }
        }

        protected override object ReadAsset(IParser parser, AssetPool pool)
        {
            /*
             * Move to width
             */
            parser.MoveNext();

            /*
             * Get width
             */
            int width = Convert.ToInt32(GetParserValue(parser));

            /*
             * Move to height
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get height
             */
            int height = Convert.ToInt32(GetParserValue(parser));

            /*
             * Move to format
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get format
             */
            TextureFormat format = (TextureFormat)(Convert.ToInt32(GetParserValue(parser)));

        

            /*
             * Create framebuffer
             */
            Framebuffer2D framebuffer = new Framebuffer2D(width, height, format,TextureInternalFormat.Rgb8,TextureDataType.Byte);
            return framebuffer;
        }

        protected override void WriteAsset(IEmitter emitter, object engineObject)
        {
            /*
             * Get framebuffer2D
             */
            Framebuffer2D framebuffer = engineObject as Framebuffer2D;

            /*
             * Emit mapping start
             */
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

            /*
             * Emit width
             */
            emitter.Emit(new Scalar(null, "Width"));
            emitter.Emit(new Scalar(null, framebuffer.Width.ToString()));

            /*
             * Emit Height
             */
            emitter.Emit(new Scalar(null, "Height"));
            emitter.Emit(new Scalar(null, framebuffer.Height.ToString()));

            /*
             * Emit format
             */
            emitter.Emit(new Scalar(null, "Format"));
            emitter.Emit(new Scalar(null,((int)framebuffer.Format).ToString()));

            /*
             * Emit mapping end
             */
            emitter.Emit(new MappingEnd());
        }
    }
}
