using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    /// <summary>
    /// Project compile parameters
    /// </summary>
    public readonly struct ProjectCompileSettings
    {
        public ProjectCompileSettings(string projectFolder, string outputFolder,string projectName, CompileConfiguration configuration, CompileArchitecture architecture)
        {
            ProjectFolder = projectFolder;
            OutputFolder = outputFolder;
            ProjectName = projectName;
            Configuration = configuration;
            Architecture = architecture;
        }

        /// <summary>
        /// The folder which the target project is in
        /// </summary>
        public readonly string ProjectFolder;

        /// <summary>
        /// Target folder which the build result will be
        /// </summary>
        public readonly string OutputFolder;

        /// <summary>
        /// Target project file name
        /// </summary>
        public readonly string ProjectName;

        /// <summary>
        /// Target compile mode(Debug,Release...etc)
        /// </summary>
        public readonly CompileConfiguration Configuration;

        /// <summary>
        /// Target cpu architecture (x32,x64...etc)
        /// </summary>
        public readonly CompileArchitecture Architecture;
    }
}
