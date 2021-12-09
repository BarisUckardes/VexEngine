using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    /// <summary>
    /// Supported cpu architectures
    /// </summary>
    public enum CompileArchitecture
    {
        x32 = 0,
        x64 = 1
    }

    /// <summary>
    /// Helper extension class for compile architecture
    /// </summary>
    public static class CompileArchitectureExtensions
    {
        public static string ToRuntimeString(this CompileArchitecture architecture)
        {
            switch (architecture)
            {
                case CompileArchitecture.x32:
                    return "x32";
                    break;
                case CompileArchitecture.x64:
                    return "x64";
                    break;
                default:
                    break;
            }
            return "none";
        }
    }
}
