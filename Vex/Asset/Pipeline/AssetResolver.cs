using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using Vex.Application;

namespace Vex.Asset
{
    /// <summary>
    /// Base asset resolver class for yaml conversions
    /// </summary>
    public abstract class AssetResolver : IYamlTypeConverter
    {
        protected static string GetParserValue(IParser parser)
        {
            return (parser.Current as Scalar).Value;
        }

        public AssetResolver()
        {
           // m_Pool = ApplicationSession.CurrentSession.AssetPool;
        }

        /// <summary>
        /// The expected object type of this resolver
        /// </summary>
        public abstract Type ExpectedAssetType { get; }

     
        public bool Accepts(Type type)
        {
            return type == ExpectedAssetType;
        }

        public object ReadYaml(IParser parser, Type type)
        {
            /*
             * Move to first
             */
            parser.MoveNext();

            /*
             * Get object via parser
             */
            object obj = GetObject(parser,m_Pool);
            
            /*
             * Move to document end
             */
            parser.MoveNext();
            parser.MoveNext();
            return obj;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            GetYaml(emitter, value);
        }

        /// <summary>
        /// Gets the object via parser
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="pool"></param>
        /// <returns></returns>
        public abstract object GetObject(IParser parser,in AssetPool pool);

        /// <summary>
        /// Writes an object via emitter
        /// </summary>
        /// <param name="emitter"></param>
        /// <param name="engineObject"></param>
        public abstract void GetYaml(IEmitter emitter, object engineObject);


        private AssetPool m_Pool;
    }
}
