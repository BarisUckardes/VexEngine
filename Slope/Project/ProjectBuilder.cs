using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Slope.Project
{
    internal sealed class ProjectBuilder
    {

        public ProjectBuilder(string directory, string projectName, int version, Guid iD)
        {
            m_Directory = directory;
            m_ProjectName = projectName;
            m_Version = version;
            m_ID = iD;
        }
        public void CreateProject()
        {
            /*
             * Initialize paths
             */
            string domainFolderPath = m_Directory + @"\Domain";
            string projectSettingsFolderPath = m_Directory + @"\Project Settings";
            string codeBaseFolderPath = m_Directory + @"CodeBase";

            /*
             * Create directory
             */
            Directory.CreateDirectory(m_Directory);

            /*
             * Create Domain folder
             */
            Directory.CreateDirectory(domainFolderPath);

            /*
             * Create project settings folder
             */
            Directory.CreateDirectory(projectSettingsFolderPath);

            /*
             * Create code base folder
             */
            Directory.CreateDirectory(codeBaseFolderPath);

            /*
             * Create code base files
             */
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "hell.txt";
            startInfo.UseShellExecute = true;
            process.StartInfo = startInfo;
            process.Start();
            System.Threading.Thread.Sleep(1000);
            process.Kill();

            /*
             * Create project files
             */
           // ProjectFileContent projectFileContent;

        }
        private string m_Directory;
        private string m_ProjectName;
        private int m_Version;
        private Guid m_ID;

       
    }
}
