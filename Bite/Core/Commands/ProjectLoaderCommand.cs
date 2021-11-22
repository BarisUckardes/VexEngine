using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Platform;
using YamlDotNet.Serialization;
namespace Bite.Core
{
    public sealed class ProjectLoaderCommand : CoreCommand
    {
        public override void OnAttach()
        {
            /*
             * Get project settings path
             */
            string expectedProjectSettingsPath = PlatformPaths.DomainRootDirectoy + @"\Project Settings";

            /*
             * Validate path
             */
            if(!Directory.Exists(expectedProjectSettingsPath))
            {
                Console.WriteLine("Not found: " + expectedProjectSettingsPath);
                EditorSession.ShutdownRequest("Project files load failed");
                return;
            }

            /*
             * Get project file path
             */
            string projectFilePath = expectedProjectSettingsPath + @"\Project.vproject";

            /*
             * Validate file
             */
            if(!File.Exists(projectFilePath))
            {
                Console.WriteLine("Not found: " + projectFilePath);
                EditorSession.ShutdownRequest("Project files load failed");
                return;
            }

            /*
             * Load project file as text
             */
            string projectFileYamlText = File.ReadAllText(projectFilePath);

            /*
             * Get project file content
             */
            IDeserializer deserializer = new DeserializerBuilder().WithTypeConverter(new ProjectFileResolver()).Build();
            ProjectFileContent content = deserializer.Deserialize<ProjectFileContent>(projectFileYamlText);

            /*
             * Set window title
             */
            EditorSession.ApplicationWindowTitle = content.ProjectName;

            /*
             * Set project file
             */
            EditorSession.ProjectFile = content;
        }

        public override void OnDetach()
        {
            /*
             * Unload project data
             */
        }
    }
}
