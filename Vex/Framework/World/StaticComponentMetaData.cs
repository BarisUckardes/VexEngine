using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    internal readonly struct StaticComponentMetaData
    {
        public StaticComponentMetaData(int localComponentIndex, List<Tuple<string, string>> fieldEntries)
        {
            LocalComponentIndex = localComponentIndex;
            FieldEntries = fieldEntries;
        }

        public readonly int LocalComponentIndex;
        public readonly List<Tuple<string, string>> FieldEntries;

       
    }
}
