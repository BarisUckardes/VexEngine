using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using Microsoft.Win32;
using Slope.Project;
using Vex.Platform;

namespace Slope.Editor
{
    internal sealed class CreateProjectWindow : GUILayout
    {
        public CreateProjectWindow(MainGUILayout mainLayout)
        {
            m_MainLayout = mainLayout;
        }
        public bool IsOpen
        {
            get
            {
                return !m_Open;
            }
        }
        public override void Finalize()

        {

        }

        public override void Initialize()
        {
  
        }

        public override void Render()
        {
            /*
             * Set window size
             */
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(512, 512));

            /*
             * Set window scale
             */
            ImGui.SetWindowFontScale(3.5f);

            /*
             * Render new project window
             */
            if(ImGui.Begin("New Project",ref m_Open,ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                /*
                 * Set window scale
                */
                ImGui.SetWindowFontScale(3.5f);

                /*
                 * Create header
                 */
                ImGui.Text("Project Name");
                ImGui.Separator();
                ImGui.Spacing();
               
                /*
                 * Create input text to get project name
                 */
                ImGui.InputText("", ref m_Input, 32);

                /*
                 * Create button to confirm project create
                 */
                if(ImGui.Button("Create"))
                {
                    string projectPath = PlatformPaths.Documents + @"\Vex_Projects";

                    ProjectBuilder builder = new ProjectBuilder(projectPath,m_Input,0,Guid.NewGuid());
                    builder.CreateProject();
                    m_Open = true;
                    m_MainLayout.AddNewProjectPath(builder.ProjectDirectoryWithProjectName);
                }

            }
            ImGui.End();
        }

        private MainGUILayout m_MainLayout;
        private string m_Input = string.Empty;
        private bool m_Open = false;
    }
}
