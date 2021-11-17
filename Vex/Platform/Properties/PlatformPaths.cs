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
    public static class PlatformPaths
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
        public static string ProjectDirectory
        {
            get
            {
                return System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            }
        }

        /// <summary>
        /// Returns the program files
        /// </summary>
        public static string ProgramfilesDirectory
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            }
        }

        /// <summary>
        /// Returns the local application data folder
        /// </summary>
        public static string LocalApplicationData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
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

        /// <summary>
        /// Returns the domain directory of the current session
        /// </summary>
        public static string DomainDirectory
        {
            get
            {
                return ProjectDirectory + @"\Domain";
            }
        }
        
    }
}
