using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Supported component field types
    /// </summary>
    public enum StaticComponentFieldType
    {
        /// <summary>
        /// Indicates whether this field type missing in the current runtime session
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// Indicates this field type is a raw data type
        /// </summary>
        Raw = 1,
        /// <summary>
        /// Indicates this field is component a reference
        /// </summary>
        Component = 2,
        /// <summary>
        /// Indicates this field is a asset reference
        /// </summary>
        Asset = 3
    }

    
}
