using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    /// <summary>
    /// Represetns the project file content
    /// </summary>
    public readonly struct ProjectFileContent
    {
        public ProjectFileContent(string projectName, int projectVersion, Guid projectID)
        {
            ProjectName = projectName;
            ProjectVersion = projectVersion;
            ProjectID = projectID;
        }
     
        /// <summary>
        /// Name of the project
        /// </summary>
        public readonly string ProjectName;

        /// <summary>
        /// Production version of the project
        /// </summary>
        public readonly int ProjectVersion;

        /// <summary>
        /// Unique project id
        /// </summary>
        public readonly Guid ProjectID;
    }
}
