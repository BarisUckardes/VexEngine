using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
namespace Vex.Asset
{
    public sealed class WorldResolver : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(World);
            }
        }

        protected override object ReadAsset(IParser parser, AssetPool pool)
        {
            /*
             * Initialize variables
             */
            World world = null;

            return world;
        }

        protected override void WriteAsset(IEmitter emitter, object targetObject)
        {

        }
    }
}
