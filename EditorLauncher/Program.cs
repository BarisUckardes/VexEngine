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
                Console.WriteLine("Editor launcher received no editor project");
                return;
            }

            Console.WriteLine("Editor launcher is created with project path:" + args[0]);

            /*
             * Create new process
             */
            Process process = new Process("Test process", args[0]);
            process.CreateProcess();
            process.WaitForExit();
            Console.WriteLine("Ediot launcher quit");
        }
    }
}
