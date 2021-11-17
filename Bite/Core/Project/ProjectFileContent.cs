using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    public readonly struct ProjectFileContent
    {
        public ProjectFileContent(string projectName, int projectVersion, Guid projectID)
        {
            ProjectName = projectName;
            ProjectVersion = projectVersion;
            ProjectID = projectID;
        }
     

        public readonly string ProjectName;
        public readonly int ProjectVersion;
        public readonly Guid ProjectID;
    }
}
