using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    public struct StaticComponentEntry
    {
        public StaticComponentEntry(int localOwnerEntityID,int localTypeID, string componentName, Guid comonentID)
        {
            LocalOwnerEntityID = localOwnerEntityID;
            LocalTypeID = localTypeID;
            ComponentName = componentName;
            ComonentID = comonentID;
        }
        public int LocalOwnerEntityID;
        public int LocalTypeID;
        public string ComponentName;
        public Guid ComonentID;

       
    }
}
