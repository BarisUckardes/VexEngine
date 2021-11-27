using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Types
{
    public static class EmittedComponentTypes
    {

        public static List<Type> ComponentTypes
        {
            get
            {
                return s_ComponentTypes;
            }
            internal set
            {
                s_ComponentTypes = value;
            }
        }
        private static List<Type> s_ComponentTypes;
    }
}
