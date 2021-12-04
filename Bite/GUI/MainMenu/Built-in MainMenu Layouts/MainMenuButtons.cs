using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bite.Core;
using Bite.GUI;
using Vex.Platform;
using Vex.Threading;

namespace Bite
{
    public static class MainMenuButtons
    {
        [MainMenuItem("Project/Save Project")]
        public static void SaveProject()
        {

        }
        [MainMenuItem("Project/Build")]
        public static void BuildProject()
        {
            GUIWindow.CreateWindow(typeof(BuildProjectGUIWindow));
        }
        [MainMenuItem("Project/Exit")]
        public static void ExitProject()
        {
            EditorCommands.SendEditorShutdownRequest();
        }

        [MainMenuItem("Code/Open Solution")]
        public static void OpenSolution()
        {
            Process commandLineProcess = new Process();
            commandLineProcess.StartInfo.FileName = "cmd.exe";
            commandLineProcess.StartInfo.RedirectStandardInput = true;
            commandLineProcess.StartInfo.RedirectStandardOutput = true;
            commandLineProcess.StartInfo.CreateNoWindow = false;
            commandLineProcess.StartInfo.UseShellExecute = false;
            commandLineProcess.Start();

            /*
             * Create visual studio project
             */
            commandLineProcess.StandardInput.WriteLine("start "+PlatformPaths.DomainRootDirectoy + @$"\CodeBase\{ProjectProperties.ProjectName}.sln"); // builds

            commandLineProcess.StandardInput.Flush();
            commandLineProcess.StandardInput.Close();
        }
        [MainMenuItem("Code/Compile")]
        public static void CompieSolution()
        {
            /*
             * Create paths
             */
            string userGameCodePath = PlatformPaths.DomainRootDirectoy + @"\CodeBase\UserGameCode";
            string userEditorCodePath = PlatformPaths.DomainRootDirectoy + @"\CodeBase\UserEditorCode";

            /*
             * Create job list for job batching
             */
            List<Job> compileJobs = new List<Job>();

            /*
             * Create compile job for UserGameCode
             */
            ProjectCompileJob userGameCodeJob = new ProjectCompileJob(new ProjectCompileSettings(userGameCodePath, userGameCodePath + @"\EditorBuild\", "UserGameCode", CompileConfiguration.Debug, CompileArchitecture.x64));
            compileJobs.Add(userGameCodeJob);

            /*
             * Create compile job for UserEditorCode
             */
            ProjectCompileJob userEditorCodeJob = new ProjectCompileJob(new ProjectCompileSettings(userEditorCodePath, userEditorCodePath + @"\EditorBuild\", "UserEditorCode", CompileConfiguration.Debug, CompileArchitecture.x64));
            compileJobs.Add(userEditorCodeJob);

            /*
             * Create job batch
             */
            JobBatch compileBatch = new JobBatch(compileJobs, OnProjectEditorCompilationFinished);
            compileBatch.ExecuteAll();

        }
        private static void OnProjectEditorCompilationFinished()
        {
            EditorCommands.SendEditorShutdownRequest();
        }

        [MainMenuItem("Windows/World Observer")]
        public static void CreteWorldObserver()
        {
            GUIWindow.CreateWindow(typeof(WorldObserverGUIWindow));
        }

        [MainMenuItem("Windows/Object Observer")]
        public static void CreateObjectObserver()
        {
            GUIWindow.CreateWindow(typeof(ObjectObserverGUIWindow));
        }

        [MainMenuItem("Windows/Domain Observer")]
        public static void CreateDomainObserver()
        {
            GUIWindow.CreateWindow(typeof(DomainObserverGUIWindow));
        }

        [MainMenuItem("Windows/Game Observer")]
        public static void CreateGameObserver()
        {
            GUIWindow.CreateWindow(typeof(GameObserverGUIWindow));
        }

        [MainMenuItem("Windows/Performance Observer")]
        public static void CreatePerformanceObserver()
        {
            GUIWindow.CreateWindow(typeof(PerformanceObserverGUIWindow));
        }


    }
}
