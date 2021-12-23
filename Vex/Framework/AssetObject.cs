using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Asset;

namespace Vex.Framework
{
    public abstract class AssetObject : VexObject
    {
        public abstract void Destroy();
    }
}
