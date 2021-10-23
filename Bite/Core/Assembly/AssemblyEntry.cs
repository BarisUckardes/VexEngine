using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    public readonly struct AssemblyEntry
    {
        public AssemblyEntry(string assemlyName, Assembly targetAssembly)
        {
            AssemlyName = assemlyName;
            TargetAssembly = targetAssembly;
        }

        public readonly string AssemlyName;
        public readonly Assembly TargetAssembly;

      
    }
}
