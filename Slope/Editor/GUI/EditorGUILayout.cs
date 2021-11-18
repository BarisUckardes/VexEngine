using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
using ImGuiNET;

namespace Slope.Editor
{
    internal sealed class EditorGUILayout
    {
        public EditorGUILayout(string[] existingProjectPaths)
        {
            m_ExistingProjectPaths = existingProjectPaths;
        }
        public bool ExitCall
        {
            get
            {
                return m_ExitCall;
            }
        }
        public void Initialize()
        {

        }
        public void Finalize()
        {

        }
        bool open = false;
        public void Render()
        {
            /*
             * Create main window strecteh to background
             */
            ImGuiWindowFlags flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;
            ImGui.SetNextWindowPos(ImGui.GetMainViewport().Pos);
            ImGui.SetNextWindowSize(ImGui.GetMainViewport().Size);
            ImGui.SetNextWindowViewport(ImGui.GetMainViewport().ID);

            /*
             * Create background window
             */
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(0, 0));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            if (ImGui.Begin("Main Window", ref open, flags))
            {
                /*
                * Create project editor view
                */
                ImGui.SetWindowFontScale(5.0f);
                ImGui.Text("Projects");
                ImGui.Separator();

                /*
                 * Vertical list
                 */
                ImGui.SetWindowFontScale(2.5f);
                for(int projectIndex = 0;projectIndex < m_ExistingProjectPaths.Length;projectIndex++)
                {
                    ImGui.Selectable(m_ExistingProjectPaths[projectIndex]);
                    if(ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left) && ImGui.IsItemHovered())
                    {
                        /*
                         * Execute project
                         */
                        Console.WriteLine("Execute project... " + m_ExistingProjectPaths[projectIndex]);

                        /*
                         * Create new process
                         */
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
                        startInfo.FileName = @"C:\Program Files\Vex\Vex\EditorLauncher.exe";
                        startInfo.Arguments = m_ExistingProjectPaths[projectIndex];

                        /*
                         * Createa process
                         */
                         System.Diagnostics.Process.Start(startInfo);
                        m_ExitCall = true;

                    }
                }

                /*
                 * Set buttons
                 */
                if(ImGui.Button("Create New Project"))
                {

                }
            }
            ImGui.PopStyleVar();
            ImGui.End();

            
        }

        private bool m_ExitCall;
        private string[] m_ExistingProjectPaths;
    }
}
