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

        protected override object ReadAsset(IParser parser, AssetPool pool)
        {
            /*
             * Initialize
             */
            List<Shader> shaders = new List<Shader>();
            string category;
            string categoryName;
            
            /*
             * Move to category
             */
            parser.MoveNext();

            /*
             * Get category
             */
            category = GetParserValue(parser);

            /*
             * Move to category name
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get category name
             */
            categoryName = GetParserValue(parser);

            /*
             * Move to shaders sqeunce start
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Move to first squence element
             */
            parser.MoveNext();

            /*
             * Get shaders
             */
            while (parser.Current.GetType() != typeof(SequenceEnd))
            {
                /*
                 * Get id text
                 */
                string idText = GetParserValue(parser);

                /*
                 * Create id
                 */
                Guid id = Guid.Parse(idText);

                /*
                 * Try load shader
                 */
                Shader shader = pool.GetOrLoadAsset(id) as Shader;

                /*
                 * Add it to shaders
                 */
                shaders.Add(shader);

                /*
                 * Move to next shader entry
                 */
                parser.MoveNext();
            }

            /*
             * Create shader program
             */
            ShaderProgram program = new ShaderProgram(category, categoryName);
            program.LinkProgram(shaders);

            return program;
        }

        protected override void WriteAsset(IEmitter emitter, object targetObject)
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
            List<Shader> shaders = program.Shaders;

            /*
             * Iterate shaders
             */
            emitter.Emit(new Scalar(null, "Shaders"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for(int shaderIndex = 0;shaderIndex < shaders.Count;shaderIndex++)
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
