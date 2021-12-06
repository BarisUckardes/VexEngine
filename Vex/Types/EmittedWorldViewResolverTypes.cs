using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Types
{
    public static class EmittedWorldViewResolverTypes
    {
        public static List<Type> Types
        {
            get
            {
                return s_Types;
            }
            internal set
            {
                s_Types = value;
            }
        }
        public static Type GetViaName(string name)
        {
            foreach (Type type in s_Types)
                if (type.Name == name)
                    return type;
            return null;
        }
        private static List<Type> s_Types;
    }
}
