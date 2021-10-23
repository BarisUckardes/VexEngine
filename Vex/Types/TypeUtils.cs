using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Types
{
    /// <summary>
    /// Misc utils for type operations
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        /// Returns the size of the specified type
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static unsafe int GetTypeSize<TType>() where TType : unmanaged
        {
            return sizeof(TType);
        }
    }
}
