using System;
using EditorLauncher;
using Vex.Platform;


namespace EditorLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Validate project path
             */
            if(args.Length == 0)
            {
                Console.WriteLine("[Editor Launcher]Editor launcher received no editor project");
                return;
            }

            /*
             *  Get arg0 as project path
             */
            string projectPath = args[0];

            Console.Clear();
           // while(true)
           // {
                /*
                * Create new process
                */
                EditorProcess process = new EditorProcess("Test process", projectPath);

                /*
                 * Create new process
                */
                process.CreateProcess();

                /*
                 * Wait for it to exit
                 */
                process.WaitForExit();
           // }

            /*
             * Signal exit
             */
            Console.WriteLine("[Editor Launcher]Editor launcher quit");
        }
    }
}
