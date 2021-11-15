using System;
using EditorLauncher;
namespace EditorLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Get start arguments
             */
            
            /*
             * Create new process
             */
            Process process = new Process("Test process", args[0], args);
            process.CreateProcess();
            process.WaitForExit();
        }
    }
}
