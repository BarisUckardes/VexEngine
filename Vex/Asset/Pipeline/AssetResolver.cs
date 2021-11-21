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
using Vex.Framework;

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
            object obj = ReadAsset(parser, m_TargetPool);
            
            /*
             * Move to document end
             */
            parser.MoveNext();
            parser.MoveNext();
            return obj;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            WriteAsset(emitter, value);
        }

        internal void SetTargetPool(AssetPool targetPool)
        {
            m_TargetPool = targetPool;
        }
        /// <summary>
        /// Gets the object via parser
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="pool"></param>
        /// <returns></returns>
        protected abstract object ReadAsset(IParser parser,AssetPool pool);

        /// <summary>
        /// Writes an object via emitter
        /// </summary>
        /// <param name="emitter"></param>
        /// <param name="engineObject"></param>
        protected abstract void WriteAsset(IEmitter emitter,object targetObject);



        private AssetPool m_TargetPool;
    }
}
