﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bite.Core;
using Bite.GUI;
using Vex.Platform;

namespace Bite
{
    public static class MainMenuButtons
    {
        [MainMenuItem("Project/Save Project")]
        public static void SaveProject()
        {

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
            * Create code base files
            */
            Console.WriteLine($"Paths: {userGameCodePath}--{userEditorCodePath}");
            Console.Write($"Compile started...");
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
            commandLineProcess.StandardInput.WriteLine($"dotnet build UserGameCode.csproj"); // builds
            commandLineProcess.StandardInput.WriteLine("cd " + userEditorCodePath);
            commandLineProcess.StandardInput.WriteLine($"dotnet build UserEditorCode.csproj"); // builds

            commandLineProcess.StandardInput.Flush();
            commandLineProcess.StandardInput.Close();
            commandLineProcess.WaitForExit();
            Console.WriteLine("Compile Finished.");
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
