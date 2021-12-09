using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Types
{

    /// <summary>
    /// A static class which collects the emitted world view resolver types types
    /// </summary>
    public static class EmittedWorldViewResolverTypes
    {
        /// <summary>
        /// Returns the view resolver types
        /// </summary>
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

        /// <summary>
        /// Get type via its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
