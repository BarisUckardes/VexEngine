using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    /// <summary>
    /// Internal struct for creating windw layout settings create parameters
    /// </summary>
    internal readonly struct WindowLayoutCreateParams
    {
        public WindowLayoutCreateParams(Type layoutType, Guid iD)
        {
            LayoutType = layoutType;
            ID = iD;
        }

        /// <summary>
        /// Target window layout type
        /// </summary>
        public readonly Type LayoutType;

        /// <summary>
        /// The id of the window layout
        /// </summary>
        public readonly Guid ID;

       
    }
}
