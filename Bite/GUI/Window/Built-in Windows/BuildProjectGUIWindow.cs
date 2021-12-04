using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bite.Core;
using Fang.Commands;
using ImGuiNET;
using Vex.Asset;
using Vex.Platform;
using Vex.Threading;

namespace Bite.GUI
{
    /// <summary>
    /// A gui window which handles the build settings and actions
    /// </summary>
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
             * Render build button
             */
            if(GUIRenderCommands.CreateButton("Build","buld"))
            {
               
                string outputFolder = PlatformPaths.DomainRootDirectoy + @"\TestBuild";
                CompileOSType osType = (CompileOSType)m_SelectedPlatformIndex;
                CompileConfiguration configuration = CompileConfiguration.Release;
                CompileArchitecture architecture = (CompileArchitecture)m_SelectedArchitectureIndex;
                Guid startWorldID = m_AllWorldAssets[m_SelectedWorldIndex].AssetID;

                ProjectBuildSettings settings = new ProjectBuildSettings(outputFolder, osType, configuration, architecture, startWorldID);

                /*
                 * Create new project build job
                 */
                m_BuildFinishJob = new ProjectBuildJob(settings);
                m_BuildFinishJob.SetOnFinishDelegate(OnBuildFinished);

                /*
                 * Execute job with the given parameters
                 */
                m_BuildFinishJob.ExecuteJob();
            }
        }

        public override void OnVisible()
        {

        }

        /// <summary>
        /// Called when build job on the other thread is finished
        /// </summary>
        private void OnBuildFinished()
        {
            Console.WriteLine($"\nBuilding project finished.{((CompileOSType)m_SelectedPlatformIndex).OSTypeToRuntime() + "-" + ((CompileArchitecture)m_SelectedArchitectureIndex).ToRuntimeString() }");
            m_BuildFinishJob = null;
        }

        /// <summary>
        /// Converts platform to dotnet cli build command mime
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts platform to source platform launcher game
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
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

        private List<Asset> m_AllWorldAssets;
        private Job m_BuildFinishJob;
        private string[] m_SupportedPlatforms = new string[] { "Windows"};
        private string[] m_SupportedArchitectures = new string[] { "x86", "x64" };
        private int m_SelectedPlatformIndex = 0;
        private int m_SelectedArchitectureIndex = 0;
        private int m_SelectedWorldIndex = 0;
    }
}
