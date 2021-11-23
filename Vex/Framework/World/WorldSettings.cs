using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    public readonly struct WorldSettings
    {
        public WorldSettings(Type logicResolver, List<Type> graphicsResolvers)
        {
            LogicResolver = logicResolver;
            GraphicsResolvers = graphicsResolvers;
        }
        public readonly Type LogicResolver;
        public readonly List<Type> GraphicsResolvers;
    }
}
