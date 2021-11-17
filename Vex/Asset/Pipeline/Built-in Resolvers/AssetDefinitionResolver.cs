using Vex.Engine;
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
    public sealed class AssetDefinitionResolver : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(AssetDefinition);
            }
        }

        protected override object ReadAsset(IParser parser,AssetPool pool)
        {
            /*
             * Move on fVexm the name
             */
            parser.MoveNext();

            /*
             * Get name
             */
            string nameYaml = GetParserValue(parser);

            /*
             * Move to ID
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get id
             */
            string idYaml = GetParserValue(parser);

            /*
             * Move to type
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get type
             */
            string typeText = GetParserValue(parser);

        
            return new AssetDefinition(nameYaml,Guid.Parse(idYaml),(AssetType)(Convert.ToInt32(typeText)));
        }

        protected override void WriteAsset(IEmitter emitter, object engineObject)
        {
            AssetDefinition definition = engineObject as AssetDefinition;

            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));
            /*
             * Emit name
             */
            emitter.Emit(new Scalar(null, "Name"));
            emitter.Emit(new Scalar(null, definition.Name));

            /*
             * Emit name
             */
            emitter.Emit(new Scalar(null, "ID"));
            emitter.Emit(new Scalar(null, definition.ID.ToString()));

            /*
             * Emit name
             */
            emitter.Emit(new Scalar(null, "Type"));
            emitter.Emit(new Scalar(null, ((int)definition.Type).ToString()));

            emitter.Emit(new MappingEnd());

        }
    }
}
