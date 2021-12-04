using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    /// <summary>
    /// Parameters for project build
    /// </summary>
    internal readonly struct ProjectBuildSettings
    {
        public readonly string OutputFolder;
        public readonly CompileOSType OsType;
        public readonly CompileConfiguration Configuration;
        public readonly CompileArchitecture Architecture;
        public readonly Guid StartWorldID;

        public ProjectBuildSettings(string outputFolder, CompileOSType osType, CompileConfiguration configuration, CompileArchitecture architecture, Guid startWorldID)
        {
            OutputFolder = outputFolder;
            OsType = osType;
            Configuration = configuration;
            Architecture = architecture;
            StartWorldID = startWorldID;
        }
    }
}
