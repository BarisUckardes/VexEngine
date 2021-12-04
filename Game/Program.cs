using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Vex.Application;
using Vex.Engine;
using Vex.Platform;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Get executable root directory
             */
            string rootDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            /*
            * Initialize application create parameters
            */
            WindowCreateParams windowCreateParams = new WindowCreateParams(WindowState.Fullscreen, "Vex Engine", 100, 100, 1280, 720, true,true);
            WindowUpdateParams windowUpdateParams = new WindowUpdateParams(1.0f / 60.0f, 1.0f / 60.0f, false);

            /*
             * Create application
             */
            Application application = new Application("Vex", windowCreateParams, windowUpdateParams, CultureInfo.InvariantCulture, new List<string>() { rootDirectory + @"\UserGameCode.dll" }, rootDirectory, args,true,true);

            
            /*
             * Create required modules
             */
            List<EngineModule> modules = new List<EngineModule>();
            modules.Add(new GameInputModule());
            modules.Add(new LogicModule());
            modules.Add(new GraphicsModule());

            /*
             * Set application modules
             */
            application.RegisterModules(modules);

            /*
             * Run
             */
            application.Run();

            Console.ReadKey();
        }
    }
}
