using Bite.Core;
using Bite.GUI;
using Bite.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Console.WriteLine("Editor not satisfied with requied argument");
                return;
            }
            Console.WriteLine("Editor intialized with target project path: " + args[0] );

            /*
             * Get arg0 as target project directory
             */
            string targetProjectDirectory = args[0];

            /*
             * Initialize application create parameters
             */
            WindowCreateParams windowCreateParams = new WindowCreateParams(WindowState.Normal, "Vex Engine", 100, 100, 1280, 720, false);
            WindowUpdateParams windowUpdateParams = new WindowUpdateParams(1.0f / 60.0f, 1.0f / 60.0f, false);

            /*
             * Create application
             */ 
            Application application = new Application("Vex", windowCreateParams, windowUpdateParams,targetProjectDirectory, args);

            /*
             * Create bite commands
             */
            List<CoreCommand> coreCommands = new List<CoreCommand>();
            coreCommands.Add(new DomainCoreCommand());
            coreCommands.Add(new ProjectLoaderCommand());
            coreCommands.Add(new EditorResourcesLoaderCommand());
            coreCommands.Add(new EditorGUILayoutLoaderCommand());
            

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
            modules.Add(new GameInputModule());
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
        }
    }
}
