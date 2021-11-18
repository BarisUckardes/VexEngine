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
        /// Returns the program files
        /// </summary>
        public static string ProgramfilesDirectory
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            }
        }
        public static string Documents
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
                return s_DomainRootDirectory + @"\Domain";
            }
        }

        /// <summary>
        /// Returns the domain root directory of the current session
        /// </summary>
        public static string DomainRootDirectoy
        {
            get
            {
                return s_DomainRootDirectory;
            }
            internal set
            {
                s_DomainRootDirectory = value;

            }
        }

        private static string s_DomainRootDirectory;
    }
}
