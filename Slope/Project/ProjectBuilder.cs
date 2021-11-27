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
        public string ProjectDirectoryWithProjectName
        {
            get
            {
                return m_ProjectDirectoryWithProjectName;
            }
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

            /*
             * Create visual studio project
             */
            commandLineProcess.StandardInput.WriteLine("cd " + codeBaseFolderPath);
            commandLineProcess.StandardInput.WriteLine($"dotnet new sln --name {m_ProjectName}.sln"); // creates solution
            commandLineProcess.StandardInput.WriteLine("dotnet new classlib --output UserGameCode/"); // creates user game project
            commandLineProcess.StandardInput.WriteLine("dotnet new classlib --output UserEditorCode/"); // creates user editor  project
            commandLineProcess.StandardInput.WriteLine("dotnet sln add UserGameCode/UserGameCode.csproj"); // adds user game project to solution
            commandLineProcess.StandardInput.WriteLine("dotnet sln add UserEditorCode/UserEditorCode.csproj"); // adds user editor project to solution
            

            commandLineProcess.StandardInput.Flush();
            commandLineProcess.StandardInput.Close();
            commandLineProcess.WaitForExit();

            /*
             * Modify usergamecode&usereditorcode project files so they can refer to vex.dll & bite.dll
             */
            string userGameCodeProjectFilePath = codeBaseFolderPath + @"\UserGameCode\UserGameCode.csproj";
            string userEditorCodeProjectFilePath = codeBaseFolderPath + @"\UserEditorCode\UserEditorCode.csproj";

            string vexDllPath = PlatformPaths.ProgramfilesDirectory + @"\Vex\Vex\Vex.dll";
            string biteDllPath = PlatformPaths.ProgramfilesDirectory + @"\Vex\Vex\Bite.dll";

            string vexDllEntry = @$"<ItemGroup>  <Reference Include=""Vex""> <HintPath>{vexDllPath} </HintPath> </Reference> </ItemGroup>";
            string biteDllEntry  = @$"<ItemGroup>  <Reference Include=""Bite""> <HintPath>{biteDllPath} </HintPath> </Reference> </ItemGroup>";

            string userGameCodeProjectFileContent = File.ReadAllText(userGameCodeProjectFilePath);
            string userEditorCodeProjectFileContent = File.ReadAllText(userEditorCodeProjectFilePath);

            int userGameCodeProjectEndIndex = userGameCodeProjectFileContent.LastIndexOf("</Project>");
            int userEditorCodeProjectEndIndex = userEditorCodeProjectFileContent.LastIndexOf("</Project>");

            File.WriteAllText(userGameCodeProjectFilePath, userGameCodeProjectFileContent.Insert(userGameCodeProjectEndIndex, vexDllEntry));
            File.WriteAllText(userEditorCodeProjectFilePath, userEditorCodeProjectFileContent.Insert(userEditorCodeProjectEndIndex, biteDllEntry));

            /*
             * Write project file contents
             */
            ProjectFileContent content = new ProjectFileContent(m_ProjectName, m_Version, m_ID);
            ISerializer serializer = new SerializerBuilder().WithTypeConverter(new ProjectFileResolver()).Build();
            string projectFileYamlText = serializer.Serialize(content);
            File.WriteAllText(projectSettingsFolderPath + @"\" + "Project.vproject",projectFileYamlText);

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
                File.WriteAllText(localApplicationSlopeProjectsPath, projectDirectoryWithProjectName);
            }
            else // already exists append anew line
            {
                File.AppendAllText(localApplicationSlopeProjectsPath, "\n" + projectDirectoryWithProjectName);
                m_ProjectDirectoryWithProjectName = projectDirectoryWithProjectName;
            }

        }
        private string m_ProjectDirectoryWithProjectName;
        private string m_Directory;
        private string m_ProjectName;
        private int m_Version;
        private Guid m_ID;

       
    }
}
