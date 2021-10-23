using Vex.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Vex.Asset
{
    public sealed class ShaderResolver : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(Shader);
            }
        }

        public override object GetObject(IParser parser, in AssetPool pool)
        {


            /*
             * Move to stage type
             */
            parser.MoveNext();

            /*
             * Get stage type
             */
            ShaderStage stage = (ShaderStage)(Convert.ToInt32(GetParserValue(parser)));

            /*
             * Move to shader source
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get shader source
             */
            string shaderSource = GetParserValue(parser);


            return new Shader(stage, shaderSource);

        }

        public override void GetYaml(IEmitter emitter, object engineObject)
        {
            /*
             * Get shader
             */
            Shader shader = engineObject as Shader;

            /*
             * Start mapping
             */
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

            /*
             * Emit shader stage type
             */
            emitter.Emit(new Scalar(null, "Stage"));
            emitter.Emit(new Scalar(null, ((int)shader.Type).ToString()));

            /*
             * Emit shader source
             */
            emitter.Emit(new Scalar(null, "Source"));
            emitter.Emit(new Scalar(null, shader.Source));

            /*
             * End mapping
             */
            emitter.Emit(new MappingEnd());
        }
    }
}
