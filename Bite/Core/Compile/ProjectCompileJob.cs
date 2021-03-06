using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Threading;

namespace Bite.Core
{
    /// <summary>
    /// A long-term job which handles the project compilation
    /// </summary>
    public sealed class ProjectCompileJob : LongTermJob
    {
        public ProjectCompileJob(in ProjectCompileSettings settings) : base(settings)
        {

        }
        protected override void DoJobCore(object targetObject)
        {
            /*
             * Get target object as project compile settings
             */
            ProjectCompileSettings settings = (ProjectCompileSettings)targetObject;

            /*
            * Create code base files
            */
            Console.Write($"Compiling {settings.ProjectName} started with settings: [{settings.Configuration.ToString()},{settings.Architecture.ToString()}]");
            Process commandLineProcess = new Process();
            commandLineProcess.StartInfo.FileName = "cmd.exe";
            commandLineProcess.StartInfo.RedirectStandardInput = true;
            commandLineProcess.StartInfo.RedirectStandardOutput = false;
            commandLineProcess.StartInfo.CreateNoWindow = false;
            commandLineProcess.StartInfo.UseShellExecute = false;
            commandLineProcess.Start();

            /*
             * Build target project
             */
            commandLineProcess.StandardInput.WriteLine("cd " + settings.ProjectFolder);
            commandLineProcess.StandardInput.WriteLine($"dotnet build {settings.ProjectName}.csproj --configuration {settings.Configuration.ToString()} --arch {settings.Architecture.ToString()} --output {settings.OutputFolder}"); // builds

            commandLineProcess.StandardInput.Flush();
            commandLineProcess.StandardInput.Close();
            commandLineProcess.WaitForExit();
        }
    }
}
