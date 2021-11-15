using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using YamlDotNet.Core;

namespace Vex.Asset
{
    public class ShaderProgramResolver : AssetResolver
    {
        public override Type ExpectedAssetType => throw new NotImplementedException();

        protected override VexObject ReadAsset(IParser parser, AssetPool pool)
        {
            throw new NotImplementedException();
        }

        protected override void WriteAsset(IEmitter emitter, VexObject targetObject)
        {
            throw new NotImplementedException();
        }
    }
}
