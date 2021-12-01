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
            * Initialize application create parameters
            */
            WindowCreateParams windowCreateParams = new WindowCreateParams(WindowState.Normal, "Vex Engine", 100, 100, 1280, 720, true);
            WindowUpdateParams windowUpdateParams = new WindowUpdateParams(1.0f / 60.0f, 1.0f / 60.0f, false);

            /*
             * Create application
             */
            Application application = new Application("Vex", windowCreateParams, windowUpdateParams, CultureInfo.InvariantCulture, new List<string>() { Assembly.GetEntryAssembly().Location + @"\UserGameCode.dll" }, "", args);


            /*
             * Create required modules
             */
            List<EngineModule> modules = new List<EngineModule>();
            modules.Add(new LogicModule());
            modules.Add(new GameInputModule());
            modules.Add(new GraphicsModule());

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
