using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    /// <summary>
    /// Supported compilation os types
    /// </summary>
    public enum CompileOSType
    {
        Windows = 0
    }

    /// <summary>
    /// Helper static class for os extensions
    /// </summary>
    public static class CompileOSTypeExtensions
    {
        public static string OSTypeToRuntime(this CompileOSType osType)
        {
            switch (osType)
            {
                case CompileOSType.Windows:
                    return "win";
                    break;
                default:
                    break;
            }
            return "none";
        }
    }
}
