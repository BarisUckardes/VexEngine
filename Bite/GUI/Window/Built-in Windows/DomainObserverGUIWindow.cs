using Bite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
using Vex.Platform;
using Vex.Graphics;
using Vex.Asset;
using System.Numerics;
using ImGuiNET;
namespace Bite.GUI
{
    public class DomainObserverGUIWindow : WindowGUILayout
    {
        
        public override void OnInVisible()
        {
            
        }
        public override void OnVisible()
        {

        }
        public override void OnLayoutBegin()
        {
            m_Domain = Session.FileDomain;
            m_CurrentFolder = m_Domain.RootFolder;
            m_FolderIcon = Session.GetEditorResource("FolderIcon",AssetType.Texture2D) as Texture2D;
            m_Texture2DIcon = Session.GetEditorResource("Texture2DIcon", AssetType.Texture2D) as Texture2D;
            m_BackButtonIcon = Session.GetEditorResource("BackButtonIcon", AssetType.Texture2D) as Texture2D;
        }

        public override void OnLayoutFinalize()
        {
            m_Domain = null;
        }

        public override void OnRenderLayout()
        {
            RenderDomainView(m_CurrentFolder);
        }

        private void RenderDomainView(DomainFolderView folder)
        {
            /*
             * Render current folder path
             */
            if (folder.ParentFolder != null)
            {
                GUIRenderCommands.CreateImage(m_BackButtonIcon, new Vector2(16, 16));
                if (GUIEventCommands.IsCurrentItemClicked())
                    m_CurrentFolder = folder.ParentFolder;
            }
            GUILayoutCommands.StayOnSameLine();
            GUIRenderCommands.CreateText(folder.Name.Replace(PlatformPaths.DomainDirectory,""),"");

            /*
             * Draw sub folders
             */
            IReadOnlyCollection<DomainFolderView> subFolders = folder.SubFolders;
            for(int folderIndex = 0;folderIndex < subFolders.Count;folderIndex++)
            {
                /*
                 * Get sub folder
                 */
                DomainFolderView subFolder = subFolders.ElementAt(folderIndex);

                /*
                 * Draw Folder
                 */
                Vector2 folderAnchorPos = GUILayoutCommands.GetCursor();
                GUIRenderCommands.CreateImage(m_FolderIcon,new Vector2(128,128));

                /*
                 * Set click event on folder
                 */
                if(GUIEventCommands.IsCurrentItemClicked())
                {
                    m_CurrentFolder = subFolder;
                }

                /*
                 * Set folder text to the center of the folder icon
                 */
                float charSize = (float)subFolder.Name.Length;
                float textLengthInPixels = charSize * GUILayoutCommands.GetCurrentFontSize()* 1.3333f/2.0f;
                float halfTextSize = (float)textLengthInPixels / 2.0f;
                float offset =64.0f - halfTextSize;
                offset = offset < 0 ? 0 : offset;
                GUILayoutCommands.SetCursorPos(folderAnchorPos + new Vector2(offset, 128));
                GUIRenderCommands.CreateText(subFolder.Name, subFolder.ID.ToString());
                GUILayoutCommands.SetCursorPos(folderAnchorPos + new Vector2(128, 0));

            }

            /*
             * Draw files
             */
            IReadOnlyCollection<DomainFileView> files = folder.Files;
            for(int fileIndex = 0;fileIndex < files.Count;fileIndex++)
            {
                /*
                 * Get file
                 */
                DomainFileView file = files.ElementAt(fileIndex);

                /*
                 * Draw file
                 */
                Vector2 fileAnchor = GUILayoutCommands.GetCursor();
                if (file.Definition.Type == AssetType.Texture2D)
                {
                    GUIRenderCommands.CreateImage(m_Texture2DIcon,new Vector2(96,96));
                }

                /*
                 * Double click event
                 */
                if(GUIEventCommands.IsCurrentItemHavored() && GUIEventCommands.IsCurrentItemDoubleClicked())
                {
                    Console.WriteLine($"File {file.Definition.Name} is double clicked");
                }

                /*
                 * Set texture to center of the file icon
                 */
                float charSize = (float)file.Definition.Name.Length;
                float textLengthInPixels = charSize * GUILayoutCommands.GetCurrentFontSize() * 1.3333f / 2.0f;
                float halfTextSize = (float)textLengthInPixels / 2.0f;
                float offset = 48.0f - halfTextSize;
                offset = offset < 0 ? 0 : offset;
                GUILayoutCommands.SetCursorPos(fileAnchor + new Vector2(offset, 96));
                GUIRenderCommands.CreateText(file.Definition.Name, file.Definition.ID.ToString());
                GUILayoutCommands.SetCursorPos(fileAnchor + new Vector2(96, 0));
            }

            /*
             * Valdate if create menu required
             */
            if (GUIEventCommands.IsMouseRightButtonClicked())
            {
                ImGui.OpenPopup("Domain_Create_Asset");
            }


            /*
             * Render create asset popup
             */
            bool isCreateShader = false;
            if (ImGui.BeginPopup("Domain_Create_Asset"))
            {
                RenderAssetCreatePopup(ref isCreateShader);
                ImGui.EndPopup();
            }

            /*
             * Open create shader popup
             */
            if (isCreateShader)
                ImGui.OpenPopup("Domain_Create_Shader");

            /*
             * Render create shader popup
             */
            if (ImGui.BeginPopup("Domain_Create_Shader"))
            {
                RenderShaderCreatePopup();
                ImGui.EndPopup();
            }


        }
        private void RenderAssetCreatePopup(ref bool isCreateShader)
        {
           
            if (ImGui.BeginMenu("Create"))
            {
                if (ImGui.BeginMenu("Graphics"))
                {
                    if (ImGui.MenuItem("Shader"))
                    {
                        Console.WriteLine("Create shader popup");
                        isCreateShader = true;
                    }
                    if (ImGui.MenuItem("Shader Program"))
                    {

                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMenu();
            }
        }
        private void RenderShaderCreatePopup()
        {
            ImGui.Text("Create a shader");
            ImGui.Separator();
            ImGui.InputText("", ref m_CreateShaderNameInput, m_InputBuffer);
            ImGui.SameLine();
            if (ImGui.BeginCombo("##Combo", m_ShaderStage.ToString()))
            {
                if (ImGui.Selectable("Vertex"))
                {
                    m_ShaderStage = ShaderStage.Vertex;
                }
                if (ImGui.Selectable("Fragment"))
                {
                    m_ShaderStage = ShaderStage.Fragment;
                }
                if (ImGui.Selectable("Geometry"))
                {
                    m_ShaderStage = ShaderStage.Geometry;
                }
                ImGui.EndCombo();
            }

            if (ImGui.Button("Create"))
            {
                Console.WriteLine("Shader created");
                ImGui.CloseCurrentPopup();
                m_ShaderStage = ShaderStage.Vertex;
            }
        }

        private ShaderStage m_ShaderStage = ShaderStage.Vertex;
        private string m_CreateShaderNameInput = string.Empty;

        private uint m_InputBuffer = 24;
        private DomainFolderView m_CurrentFolder;
        private DomainView m_Domain;
        private Texture2D m_FolderIcon;
        private Texture2D m_Texture2DIcon;
        private Texture2D m_BackButtonIcon;
    }
}
