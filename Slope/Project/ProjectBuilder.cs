using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Bite.Core;
using YamlDotNet.Serialization;
using Vex.Platform;

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
            string projectDirectoryWithProjectName = m_Directory + @"\" + m_ProjectName;
            string domainFolderPath = projectDirectoryWithProjectName + @"\Domain";
            string projectSettingsFolderPath = projectDirectoryWithProjectName + @"\Project Settings";
            string codeBaseFolderPath = projectDirectoryWithProjectName + @"\CodeBase";
           
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
            Process commandLineProcess = new Process();
            commandLineProcess.StartInfo.FileName = "cmd.exe";
            commandLineProcess.StartInfo.RedirectStandardInput = true;
            commandLineProcess.StartInfo.RedirectStandardOutput = true;
            commandLineProcess.StartInfo.CreateNoWindow = true;
            commandLineProcess.StartInfo.UseShellExecute = false;
            commandLineProcess.Start();

            commandLineProcess.StandardInput.WriteLine("cd " + codeBaseFolderPath);
            commandLineProcess.StandardInput.WriteLine("dotnet new sln --name mysolution.sln");
            commandLineProcess.StandardInput.WriteLine("dotnet new classlib --output UserGameCode/");
            commandLineProcess.StandardInput.WriteLine("dotnet new classlib --output UserEditorCode/");

            commandLineProcess.StandardInput.Flush();
            commandLineProcess.StandardInput.Close();
            commandLineProcess.WaitForExit();

            /*
             * Write project file contents
             */
            ProjectFileContent content = new ProjectFileContent(m_ProjectName, m_Version, m_ID);
            ISerializer serializer = new SerializerBuilder().WithTypeConverter(new ProjectFileResolver()).Build();
            string projectFileYamlText = serializer.Serialize(content);
            File.WriteAllText(projectSettingsFolderPath + @"\" + m_ProjectName + ".vproject",projectFileYamlText);

            /*
             * Write new project to temp
             */
            string localApplicationPath = PlatformPaths.LocalApplicationData;
            string localApplicationVexPath = localApplicationPath + @"\Vex";
            string localApplicationSlopePath = localApplicationVexPath + @"\Slope";
            string localApplicationSlopeProjectsPath = localApplicationSlopePath + @"\validatedProjectes.txt";

            /*
             * Validate vex path
             */
            if(!Directory.Exists(localApplicationVexPath))
            {
                Directory.CreateDirectory(localApplicationVexPath);
            }

            /*
             * Validate slope path
             */
            if(!Directory.Exists(localApplicationSlopePath))
            {
                Directory.CreateDirectory(localApplicationSlopePath);
            }

            /*
             * Validate slope projects
             */
            if(!File.Exists(localApplicationSlopeProjectsPath))
            {
                File.WriteAllText(localApplicationSlopeProjectsPath, m_Directory);
            }
            else // already exists append anew line
            {
                File.AppendAllText(localApplicationSlopeProjectsPath, "\n" + m_Directory);
            }

        }
        private string m_Directory;
        private string m_ProjectName;
        private int m_Version;
        private Guid m_ID;

       
    }
}
