using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Application
{
    /// <summary>
    /// Utility class for command line arguments
    /// </summary>
    public static class CommandLineArguments
    {

        /// <summary>
        /// Returns the command line arguments
        /// </summary>
        public static string[] Arguments
        { 
            get
            {
                return s_CommandLineArguments;
            }
            internal set
            {
                s_CommandLineArguments = value;
            }
        }

        private static string[] s_CommandLineArguments;
    }
}
