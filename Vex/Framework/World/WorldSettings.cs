using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Startup world settings for intialization of a world
    /// </summary>
    public readonly struct WorldSettings
    {
        public WorldSettings(Type logicResolver, List<Type> graphicsResolvers)
        {
            LogicResolver = logicResolver;
            GraphicsResolvers = graphicsResolvers;
        }

        /// <summary>
        /// Target logic resolver
        /// </summary>
        public readonly Type LogicResolver;

        /// <summary>
        /// List of graphics resolvers in this world
        /// </summary>
        public readonly List<Type> GraphicsResolvers;
    }
}
