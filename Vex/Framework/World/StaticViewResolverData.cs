using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    public readonly struct StaticViewResolverData
    {
        public StaticViewResolverData(Type view, List<Type> resolvers)
        {
            View = view;
            Resolvers = resolvers;
        }
        public readonly Type View;
        public readonly List<Type> Resolvers;

      
    }
}
