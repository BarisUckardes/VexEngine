using System;
using Fang.Renderer;
using Slope.Editor;
using Slope.Project;
using Vex.Platform;
using Vex.Threading;
namespace Slope
{
    public class YoJob : Job
    {
        protected override void DoJobCore(object targetObject)
        {
            Console.WriteLine("Started: " + (targetObject as string));
            System.Threading.Thread.Sleep(3000);
            Console.WriteLine("Finished");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Start a new window
             */
            WindowCreateParams windowCreateParams = new WindowCreateParams(WindowState.Normal, "Vex Engine", 100, 100, 1280, 720, false, true);
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
