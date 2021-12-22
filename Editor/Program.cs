using Bite.Core;
using Bite.GUI;
using Bite.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Vex.Application;
using Vex.Engine;
using Vex.Platform;
using Vex.Profiling;
namespace Game
{
    class PVexgram
    {
        static void Main(string[] args)
        {
            /*
             * Validate project start
             */
            if(args.Length == 0)
            {
                Console.WriteLine("Editor not satisfied with an argument");
                return;
            }

            /*
             * Get arg0 as target project directory
             */
            string targetProjectDirectory = args[0];

            /*
             * Create additonal library paths
             */
            List<string> additonalLibraries = new List<string>() { targetProjectDirectory + @"\CodeBase\UserGameCode\EditorBuild\UserGameCode.dll", targetProjectDirectory + @"\CodeBase\UserEditorCode\EditorBuild\UserEditorCode.dll" };

            /*
             * Create unique temp session
             */
            string tempSessionID = Guid.NewGuid().ToString();

            /*
             * Create new temp session for editor
             */
            string tempSessionPath = PlatformPaths.LocalApplicationData + @"\Vex\TempSession\"+ tempSessionID + @"\";

            /*
             * Create temp session mode
             */
            Directory.CreateDirectory(tempSessionPath);

            /*
             * Copy addiotnal libraries for session
             */
            List<string> tempLibraryPaths = new List<string>();
            for (int libraryIndex = 0; libraryIndex < additonalLibraries.Count; libraryIndex++)
            {
                /*
                 * Get path
                 */
                string libraryPath = additonalLibraries[libraryIndex];

                /*
                 * Validate file
                 */
                if (!File.Exists(libraryPath))
                    continue;

                /*
                 * Copy to temp folder
                 */
                string copyLocation = tempSessionPath + Path.GetFileName(libraryPath);
                File.Copy(libraryPath, copyLocation, true);
                tempLibraryPaths.Add(copyLocation);
            }
            
            /*
             * Initialize application create parameters
             */
            WindowCreateParams windowCreateParams = new WindowCreateParams(WindowState.Normal, "Vex Engine", 100, 100, 1280, 720, false,false);
            WindowUpdateParams windowUpdateParams = new WindowUpdateParams(1.0f / 60.0f, 1.0f / 60.0f, false);

            /*
             * Create application
             */ 
            Application application = new Application("Vex", windowCreateParams, windowUpdateParams, CultureInfo.InvariantCulture, tempLibraryPaths, targetProjectDirectory, args,false,false);

            /*
             * Create bite commands
             */
            List<CoreCommand> coreCommands = new List<CoreCommand>();
            coreCommands.Add(new LastWindowPosSizeLoader());
            coreCommands.Add(new DomainCoreCommand());
            coreCommands.Add(new ProjectLoaderCommand());
            coreCommands.Add(new EditorResourcesLoaderCommand());
            coreCommands.Add(new LastWorldLoaderCommand());

            

            /*
             * Creat bite gui systems
             */
            List<GUISystem> guiSystems = new List<GUISystem>();
            guiSystems.Add(new MainMenuGUISystem());
            guiSystems.Add(new WindowGUISystem());
            guiSystems.Add(new ObjectGUISystem());
            guiSystems.Add(new ComponentGUISystem());

            /*
             * Create required modules
             */
            List<EngineModule> modules = new List<EngineModule>();
            modules.Add(new TestModule());
            modules.Add(new LogicModule());
            modules.Add(new PlatformInputModule());
            modules.Add(new GraphicsModule());
            modules.Add(new BiteModule(coreCommands,guiSystems));

            /*
             * Set application modules
             */
            application.RegisterModules(modules);

            /*
             * Run
             */
            application.Run();

            /*
             * Delete temp session files
             */
            Directory.Delete(tempSessionPath, true);
        }
    }
}
