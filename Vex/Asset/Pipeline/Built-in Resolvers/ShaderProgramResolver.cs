using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Vex.Graphics;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Vex.Asset
{
    public class ShaderProgramResolver : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(ShaderProgram);
            }
        }

        protected override VexObject ReadAsset(IParser parser, AssetPool pool)
        {
            throw new NotImplementedException();
        }

        protected override void WriteAsset(IEmitter emitter, VexObject targetObject)
        {
            /*
             * Get Shader program
             */
            ShaderProgram program = targetObject as ShaderProgram;

            /*
             * Start mapping
             */
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

            /*
             * Emit category
             */
            emitter.Emit(new Scalar(null, "Category"));
            emitter.Emit(new Scalar(null, program.Category));

            /*
             * Emit category name
             */
            emitter.Emit(new Scalar(null, "Category Name"));
            emitter.Emit(new Scalar(null, program.CategoryName));

            /*
             * Write Each shader
             */
            Shader[] shaders = program.Shaders;

            /*
             * Iterate shaders
             */
            emitter.Emit(new Scalar(null, "Shaders"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for(int shaderIndex = 0;shaderIndex < shaders.Length;shaderIndex++)
            {
                /*
                 * Get shader 
                 */
                Shader shader = shaders[shaderIndex];
                emitter.Emit(new Scalar(null, (shader == null) ? Guid.Empty.ToString() : shader.ID.ToString()));
            }
            emitter.Emit(new SequenceEnd());

            /*
             * End Mapping
             */
            emitter.Emit(new MappingEnd());
        }
    }
}
