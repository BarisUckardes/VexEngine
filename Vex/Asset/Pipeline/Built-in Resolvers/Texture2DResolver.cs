using Vex.Engine;
using Vex.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using Vex.Framework;
using Vex.Extensions;
namespace Vex.Asset
{
    public sealed class Texture2DResolver : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(Texture2D);
            }
        }

        protected override object ReadAsset(IParser parser,AssetPool pool)
        {
            /*
             * Move on fVexm the width
             */
            parser.MoveNext();

            /*
             * Get width
             */
            string widthYaml = GetParserValue(parser);

            /*
             * Move to Height
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get height
             */
            string heightYaml = GetParserValue(parser);

            /*
             * Move to format
            */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get format
             */
            string formatYaml = GetParserValue(parser);

            /*
             * Move to Data
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get internal format
             */
            string internalFormatYaml = GetParserValue(parser);

            /*
             * Move to data
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get data
             */
            string dataYaml = GetParserValue(parser);

            /*
             * Create texture
             */
            Texture2D texture = new Texture2D(Convert.ToInt32(widthYaml), Convert.ToInt32(heightYaml), (TextureFormat)(Convert.ToInt32(formatYaml)),(TextureInternalFormat)(Convert.ToInt32(internalFormatYaml)));

            /*
             * Set texture data
             */
            texture.SetData(Convert.FromBase64String(dataYaml),true);


            return texture;
        }

        protected override void WriteAsset(IEmitter emitter, object engineObject)
        {
            /*
             *  Get Texture 2d
             */
            Texture2D texture = engineObject as Texture2D;

            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

            /*
             * Emit Width
             */
            emitter.Emit(new Scalar(null, "Width"));
            emitter.Emit(new Scalar(null, texture.Width.ToString()));

            /*
             * Emit Height
             */
            emitter.Emit(new Scalar(null, "Height"));
            emitter.Emit(new Scalar(null, texture.Height.ToString()));

            /*
             * Emit Format
             */
            emitter.Emit(new Scalar(null, "Format"));
            emitter.Emit(new Scalar(null, ((int)texture.Format).ToString()));

            /*
             * Emit internal format
             */
            emitter.Emit(new Scalar(null, "Internal Format"));
            emitter.Emit(new Scalar(null, ((int)texture.InternalFormat).ToString()));

            /*
            * Emit Data
            */
            string dataYaml =Convert.ToBase64String(texture.CpuData == null ? new byte[0] : texture.CpuData);
            emitter.Emit(new Scalar(null, "Data"));
            emitter.Emit(new Scalar(null, dataYaml));


            emitter.Emit(new MappingEnd());
        }

       
    }
}
