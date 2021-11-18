using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security;
using Vex.Platform;

namespace EditorLauncher
{
    internal class EditorProcess
    {
        public EditorProcess(string processName,  string projectPath)
        {
            m_ProcessName = processName;
            m_ProjectPath = projectPath;
            m_Process = null;
        }

        public void CreateProcess()
        {
            /*
             * Create security key
             */
            SecureString password = new SecureString();
            password.AppendChar('b');

            /*
             * Start start info
             */
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Password = password;
            startInfo.Arguments = m_ProjectPath;
            startInfo.FileName = PlatformPaths.ProgramfilesDirectory +@"\Vex\Vex\Editor.exe";

            
            Console.WriteLine("[Editor Launcher]Editor launcher create process with project path: " + m_ProjectPath);
            Console.WriteLine("[Editor Launcher]Editor launcher target editor path: " + PlatformPaths.ProgramfilesDirectory + @"\Vex\Vex\Editor.exe");

            /*
             * Createa process
             */
            m_Process = System.Diagnostics.Process.Start(startInfo);
        }
        public void TerminateProcess()
        {
            /*
             * Terminate proceess
             */
            m_Process?.Kill();
        }
        public void WaitForExit()
        {
            m_Process.WaitForExit();
        }
        private System.Diagnostics.Process m_Process;
        private string m_ProcessName;
        private string m_ProjectPath;
    }
}
