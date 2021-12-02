using System;
using Fang.Renderer;
using Slope.Editor;
using Slope.Project;
using Vex.Platform;

namespace Slope
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Start a new window
             */
            WindowCreateParams windowCreateParams = new WindowCreateParams(WindowState.Normal, "Vex Engine", 100, 100, 1280, 720, false,true);
            WindowUpdateParams windowUpdateParams = new WindowUpdateParams(1.0f / 60.0f, 1.0f / 60.0f, false);

            /*
             * Create editor
             */
            GUIEditor editor = new GUIEditor(windowCreateParams, windowUpdateParams);

            /*
             * Run editor
             */
            editor.Run();
        }
    }
}
