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
        public ProjectBuildSettings(string platformCommand, string outputFolder, Guid startWorldID, bool isSelfContained, string platformLauncherFolder)
        {
            PlatformCommand = platformCommand;
            OutputFolder = outputFolder;
            StartWorldID = startWorldID;
            IsSelfContained = isSelfContained;
            PlatformLauncherFolder = platformLauncherFolder;
        }

        /// <summary>
        /// Which platform build will target
        /// </summary>
        public readonly string PlatformCommand;

        /// <summary>
        /// Target output folder which build will be exported
        /// </summary>
        public readonly string OutputFolder;

        /// <summary>
        /// Primary start world of the build
        /// </summary>
        public readonly Guid StartWorldID;

        /// <summary>
        /// Is build will include .net core runtime libraries with it
        /// </summary>
        public readonly bool IsSelfContained;

        /// <summary>
        /// Which folder will be targeted as game launcher
        /// </summary>
        public readonly string PlatformLauncherFolder;
    }
}
