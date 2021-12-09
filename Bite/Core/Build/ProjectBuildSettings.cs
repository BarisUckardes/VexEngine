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
        /// <summary>
        /// The output folder of the build
        /// </summary>
        public readonly string OutputFolder;

        /// <summary>
        /// The target operationg system
        /// </summary>
        public readonly CompileOSType OsType;

        /// <summary>
        /// Build configuration
        /// </summary>
        public readonly CompileConfiguration Configuration;

        /// <summary>
        /// The architecture
        /// </summary>
        public readonly CompileArchitecture Architecture;

        /// <summary>
        /// Start world id
        /// </summary>
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
