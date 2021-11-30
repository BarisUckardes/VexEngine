using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Types
{
    /// <summary>
    /// A static class which collects the emitted component types
    /// </summary>
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

        /// <summary>
        /// Get component type via its name
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Check if a type name exists on the component type list
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
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
