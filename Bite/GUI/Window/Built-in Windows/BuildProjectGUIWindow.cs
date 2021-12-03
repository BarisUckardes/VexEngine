using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
using ImGuiNET;
using Vex.Asset;
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
            m_AllWorldAssets = Session.GetAssets(AssetType.World);
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
                        m_SelectedPlatformIndex = platformIndex;
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
                        m_SelectedArchitectureIndex = architectureIndex;
                    }
                }

                GUIRenderCommands.FinalizeCombo();
            }

            /*
             * Select startup world
             */
            ImGui.SetNextItemWidth(ImGui.CalcTextSize("Select target architecture").X);
            if (GUIRenderCommands.CreateCombo("",m_AllWorldAssets[m_SelectedWorldIndex].Name,"sltw"))
            {
                int index = 0;
                foreach(Asset asset in m_AllWorldAssets)
                {
                    if(GUIRenderCommands.CreateSelectableItem(asset.Name,asset.AssetID.ToString()))
                    {
                        m_SelectedWorldIndex = index;
                    }
                    index++;
                }
                GUIRenderCommands.FinalizeCombo();
            }
            /*
             * Render output folder
             */
            string outputFolder = PlatformPaths.DomainRootDirectoy + @"\TestBuild";

            /*
             * Create output folder
             */
            Directory.CreateDirectory(outputFolder);
            
            /*
             * Render build button
             */
            if(GUIRenderCommands.CreateButton("Build","buld"))
            {
                TryBuild(PlatformToBuildCommand(m_SupportedPlatforms[m_SelectedPlatformIndex]),outputFolder,m_AllWorldAssets[m_SelectedWorldIndex].AssetID);
            }
        }

        public override void OnVisible()
        {

        }
        private void TryBuild(string platformCommand,string outputFolder,Guid startWorldID,bool isSelfContained = true)
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
            commandLineProcess.StandardInput.WriteLine($"dotnet publish -c Release -r {platformCommand} /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true --output ./PublishFolder"); // builds

            commandLineProcess.StandardInput.Flush();
            commandLineProcess.StandardInput.Close();
            commandLineProcess.WaitForExit();

            /*
             * Copy buid files
             */
            PlatformFile.CopyDirectory(userGameCodePath + @"\PublishFolder\",outputFolder);

            /*
             * Create domain folder
             */
            Directory.CreateDirectory(outputFolder + @"\Domain");

            /*
             * Copy domain files
             */
            PlatformFile.CopyDirectory(PlatformPaths.DomainDirectory, outputFolder + @"\Domain");

            /*
             * Copy game executable
            */
            PlatformFile.CopyDirectory(PlatformPaths.ProgramfilesDirectory + @"\Vex\Vex\PlatformLaunchers\" + PlatformToLauncherFolder(m_SupportedPlatforms[m_SelectedPlatformIndex]), outputFolder);

            /*
             * Create immediate world id file
             */
            File.WriteAllText(outputFolder + @"\ImmediateWorld.vsettings", startWorldID.ToString());
         
        }

        private string PlatformToBuildCommand(string platform)
        {
            switch (platform)
	        {
                case "Windows":
                {
                        return "win-x64";
                        break;
                }
		        default:
                break;
	        }
            return "";
        }
        private string PlatformToLauncherFolder(string platform)
        {
            switch (platform)
	        {
                case "Windows":
                {
                        return "Windows";
                        break;
                }
		        default:
                break;
	        }
            return "";
        }
        private int m_SelectedPlatformIndex = 0;
        private int m_SelectedArchitectureIndex = 0;
        private int m_SelectedWorldIndex = 0;
        private string[] m_SupportedPlatforms = new string[] { "Windows", "Linux","Mac","PS5","Android","IOS" };
        private string[] m_SupportedArchitectures = new string[] { "x86", "x64" };
        private List<Asset> m_AllWorldAssets;
    }
}
