using Bite.Module;
using System;
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
             * Initialize application create parameters
             */
            WindowCreateParams windowCreateParams = new WindowCreateParams(WindowState.Normal, "R Engine", 100, 100, 1280, 720, false);
            WindowUpdateParams windowUpdateParams = new WindowUpdateParams(1.0f / 60.0f, 1.0f / 60.0f, false);

            /*
             * Create application
             */ 
            Application application = new Application("Ro", windowCreateParams, windowUpdateParams, args);

            /*
             * Register modules
             */
            application.RegisterModule<TestModule>();
            application.RegisterModule<LogicModule>();
            application.RegisterModule<GameInputModule>();
            application.RegisterModule<GraphicsModule>();
            application.RegisterModule<BiteModule>();

            /*
             * Run
             */
            application.Run();

          
        }
    }
}
