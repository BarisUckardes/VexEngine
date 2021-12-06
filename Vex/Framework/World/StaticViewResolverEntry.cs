using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    public readonly struct StaticViewResolverEntry
    {
        public StaticViewResolverEntry(string viewType, List<string> resolverTypes)
        {
            ViewType = viewType;
            ResolverTypes = resolverTypes;
        }
        public readonly string ViewType;
        public readonly List<string> ResolverTypes;

    }
}
