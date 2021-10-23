using Bite.Module;
using System;
using Vex.Application;
using Vex.Engine;
using Vex.Platform;

namespace Game
{
    class PVexgram
    {
        static void Main(string[] args)
        {
            WindowCreateParams windowCreateParams = new WindowCreateParams(WindowState.Normal, "R Engine", 100, 100, 1280, 720, false);
            WindowUpdateParams windowUpdateParams = new WindowUpdateParams(1.0f / 60.0f, 1.0f / 60.0f, false);

            Application application = new Application("Ro", windowCreateParams, windowUpdateParams, args);
            application.RegisterModule<TestModule>();
            application.RegisterModule<LogicModule>();
            application.RegisterModule<GameInputModule>();
            application.RegisterModule<GraphicsModule>();
            application.RegisterModule<BiteCoreModule>();
            application.RegisterModule<BiteGUIModule>();

            application.Run();
        }
    }
}
