using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
using ImGuiNET;
using Vex.Platform;

namespace Bite.GUI
{
    [WindowLayout("Build Project")]
    public sealed class BuildProjectGUIWindow : WindowGUILayout
    {
        public override void OnInVisible()
        {

        }

        public override void OnLayoutBegin()
        {

        }

        public override void OnLayoutFinalize()
        {

        }

        public override void OnRenderLayout()
        {
            /*
             * Render platforms
             */
            GUIRenderCommands.CreateText("Select target platform", "");
            ImGui.SetNextItemWidth(ImGui.CalcTextSize("Select target platform").X);
            if (GUIRenderCommands.CreateCombo("",m_SupportedPlatforms[m_SelectedPlatformIndex],"selectedPlatform"))
            {
                for (int platformIndex = 0; platformIndex < m_SupportedPlatforms.Length; platformIndex++)
                {
                    if (GUIRenderCommands.CreateSelectableItem(m_SupportedPlatforms[platformIndex], platformIndex.ToString()))
                    {

                    }
                }
                    GUIRenderCommands.FinalizeCombo();
            }

            /*
             * Render Architecture
             */
            GUIRenderCommands.CreateText("Selecte target architecture", "");
            ImGui.SetNextItemWidth(ImGui.CalcTextSize("Select target architecture").X);
            if (GUIRenderCommands.CreateCombo("", m_SupportedArchitectures[m_SelectedArchitectureIndex],"selectedArch"))
            {
                for(int architectureIndex = 0;architectureIndex <m_SupportedArchitectures.Length;architectureIndex++)
                {
                    if (GUIRenderCommands.CreateSelectableItem(m_SupportedArchitectures[architectureIndex],architectureIndex.ToString()))
                    {

                    }
                }

                GUIRenderCommands.FinalizeCombo();
            }
            /*
             * Render output folder
             */
            string outputFolder = PlatformPaths.DomainRootDirectoy + @"\TestBuild";
            
            /*
             * Render build button
             */
            if(GUIRenderCommands.CreateButton("Build","buld"))
            {
                TryBuild("win-x64",outputFolder);
            }
        }

        public override void OnVisible()
        {

        }
        private void TryBuild(string command,string outputFolder)
        {
            /*
             * Get user project path
             */
            string userGameCodePath = PlatformPaths.DomainRootDirectoy + @"\CodeBase\UserGameCode";

            /*
             * Create process
             */
            Process commandLineProcess = new Process();
            commandLineProcess.StartInfo.FileName = "cmd.exe";
            commandLineProcess.StartInfo.RedirectStandardInput = true;
            commandLineProcess.StartInfo.RedirectStandardOutput = false;
            commandLineProcess.StartInfo.CreateNoWindow = false;
            commandLineProcess.StartInfo.UseShellExecute = false;
            commandLineProcess.Start();

            /*
             * Create visual studio project
             */
            commandLineProcess.StandardInput.WriteLine("cd " + userGameCodePath);
            commandLineProcess.StandardInput.WriteLine($"dotnet publish -c Release -r {command} --output ./PublishFolder"); // builds

            commandLineProcess.StandardInput.Flush();
            commandLineProcess.StandardInput.Close();
            commandLineProcess.WaitForExit();

            /*
             * Copy to build folder
             */
            File.Copy(userGameCodePath + @"\PublishFolder\UserGameCode.dll", outputFolder +@"\UserGameCode.dll",true);

            /*
             * Copy vex game files
             */
           // File.Copy()
        }

        private int m_SelectedPlatformIndex;
        private int m_SelectedArchitectureIndex;
        private string[] m_SupportedPlatforms = new string[] { "Windows", "Linux","Mac","PS5","Android","IOS" };
        private string[] m_SupportedArchitectures = new string[] { "x86", "x64" };
    }
}
