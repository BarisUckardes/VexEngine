using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    internal readonly struct StaticComponentEntry
    {
        public StaticComponentEntry(int localOwnerEntityID, string componentName, Guid comonentID)
        {
            LocalOwnerEntityID = localOwnerEntityID;
            ComponentName = componentName;
            ComonentID = comonentID;
        }
        public readonly int LocalOwnerEntityID;
        public readonly string ComponentName;
        public readonly Guid ComonentID;

       
    }
}
