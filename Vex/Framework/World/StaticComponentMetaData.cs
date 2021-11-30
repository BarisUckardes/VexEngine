using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Metadata necessary for creating a static component data
    /// </summary>
    public readonly struct StaticComponentMetaData
    {
        public StaticComponentMetaData(int localComponentIndex, List<Tuple<string,string, string>> fieldEntries)
        {
            LocalComponentIndex = localComponentIndex;
            FieldEntries = fieldEntries;
        }

        /// <summary>
        /// The index of the target component
        /// </summary>
        public readonly int LocalComponentIndex;

        /// <summary>
        /// List of field data
        /// </summary>
        public readonly List<Tuple<string, string,string>> FieldEntries;

       
    }
}
