using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Platform;
using Vex.Threading;

namespace Bite.Core
{
    /// <summary>
    /// A job which runs project build sequence
    /// </summary>
    internal sealed class ProjectBuildJob : Job
    {
        public ProjectBuildJob(in ProjectBuildSettings settings) : base(settings)
        {

        }
        protected override void DoJobCore(object targetObject)
        {
            /*
             * Get build settings
             */
            ProjectBuildSettings buildSettings = (ProjectBuildSettings)targetObject;

            /*
            * Get user project path
            */
            string userGameCodePath = PlatformPaths.DomainRootDirectoy + @"\CodeBase\UserGameCode";

            /*
             * Create process
             */
            Process commandLineProcess = new Process();
            commandLineProcess.StartInfo.FileName = "cmd.exe";
            commandLineProcess.StartInfo.RedirectStandardInput = true;
            commandLineProcess.StartInfo.RedirectStandardOutput = false;
            commandLineProcess.StartInfo.CreateNoWindow = false;
            commandLineProcess.StartInfo.UseShellExecute = false;
            commandLineProcess.Start();

            /*
             * Create visual studio project
             */
            commandLineProcess.StandardInput.WriteLine("cd " + userGameCodePath);
            commandLineProcess.StandardInput.WriteLine($"dotnet publish -c Release -r {buildSettings.PlatformCommand} /p:IncludeNativeLibrariesForSelfExtract=true --self-contained {buildSettings.IsSelfContained.ToString()} --output ./PublishFolder"); // builds

            commandLineProcess.StandardInput.Flush();
            commandLineProcess.StandardInput.Close();
            commandLineProcess.WaitForExit();

            /*
             * Copy buid files
             */
            PlatformFile.CopyDirectory(userGameCodePath + @"\PublishFolder\", buildSettings.OutputFolder);

            /*
             * Create domain folder
             */
            Directory.CreateDirectory(buildSettings.OutputFolder + @"\Domain");

            /*
             * Copy domain files
             */
            PlatformFile.CopyDirectory(PlatformPaths.DomainDirectory, buildSettings.OutputFolder + @"\Domain");

            /*
             * Copy game executable
            */
            PlatformFile.CopyDirectory(PlatformPaths.ProgramfilesDirectory + @"\Vex\Vex\PlatformLaunchers\" + buildSettings.PlatformLauncherFolder, buildSettings.OutputFolder);

            /*
             * Create immediate world id file
             */
            File.WriteAllText(buildSettings.OutputFolder + @"\ImmediateWorld.vsettings", buildSettings.StartWorldID.ToString());
        }
    }
}
