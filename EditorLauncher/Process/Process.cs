using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security;
namespace EditorLauncher
{
    internal class Process
    {
        public Process(string processName, string executablePath, string[] args)
        {
            m_ProcessName = processName;
            m_ExecutablePath = executablePath;
            m_Args = args;
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
            startInfo.FileName = m_ExecutablePath;

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
        private string m_ExecutablePath;
        private string[] m_Args;
    }
}
