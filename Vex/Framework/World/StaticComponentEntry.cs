using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Component entry data
    /// </summary>
    public struct StaticComponentEntry
    {
        public StaticComponentEntry(int localOwnerEntityID,int localTypeID, string componentName, Guid comonentID)
        {
            LocalOwnerEntityID = localOwnerEntityID;
            LocalTypeID = localTypeID;
            ComponentName = componentName;
            ComonentID = comonentID;
        }

        /// <summary>
        /// Target local owner entity id
        /// </summary>
        public int LocalOwnerEntityID;

        /// <summary>
        /// Index mapped for local type list
        /// </summary>
        public int LocalTypeID;

        /// <summary>
        /// Component name in string
        /// </summary>
        public string ComponentName;

        /// <summary>
        /// Component id in guid
        /// </summary>
        public Guid ComonentID;

       
    }
}
