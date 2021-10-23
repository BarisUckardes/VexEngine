using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Utility class for path operation
    /// </summary>
    public static class Paths
    {
        /// <summary>
        /// Returns the application executable path
        /// </summary>
        public static string ExecutablePath
        {
            get
            {
                return System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            }
        }

        /// <summary>
        /// Retunrs the application executable directory
        /// </summary>
        public static string ExecutableDirectory
        {
            get
            {
                return System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            }
        }

        /// <summary>
        /// Returns the desktop directory
        /// </summary>
        public static string DesktopDirectory
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            }
        }
    }
}
