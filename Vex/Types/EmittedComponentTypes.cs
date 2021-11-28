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
        public static Type GetTypeViaName(string typeName)
        {
            /*
             * Iterate each component type and validate a match
             */
            foreach (Type type in s_ComponentTypes)
                if (type.Name == typeName)
                    return type;
            return null;
        }
        public static bool IsExist(string typeName)
        {
            /*
             * Iterate each componen type and validate a match
             */
            foreach (Type type in s_ComponentTypes)
                if (type.Name == typeName)
                    return true;
            return false;
        }
        private static List<Type> s_ComponentTypes;
    }
}
