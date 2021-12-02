using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    /// <summary>
    /// Utility class for the user to collect project properties
    /// </summary>
    public static class ProjectProperties
    {
        /// <summary>
        /// Returns the project name
        /// </summary>
        public static string ProjectName
        {
            get
            {
                return s_ProjectName;
            }
            internal set
            {
                s_ProjectName = value;
            }

        }
        private static string s_ProjectName;
    }
}
