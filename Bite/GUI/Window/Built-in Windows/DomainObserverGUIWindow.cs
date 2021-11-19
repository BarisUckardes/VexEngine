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
using System.IO;

namespace Bite.GUI
{
    [WindowLayout("Domain Observer")]
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
            /*
             * Get file domain
             */
            m_Domain = Session.FileDomain;

            /*
             * Get root folder
             */
            m_CurrentFolder = m_Domain.RootFolder;

            /*
             * Get resources
             */
            m_FolderIcon = Session.GetEditorResource("FolderIcon",AssetType.Texture2D) as Texture2D;
            m_Texture2DIcon = Session.GetEditorResource("Texture2DIcon", AssetType.Texture2D) as Texture2D;
            m_BackButtonIcon = Session.GetEditorResource("BackButtonIcon", AssetType.Texture2D) as Texture2D;
            m_ShaderIcon = Session.GetEditorResource("ShaderFileIcon", AssetType.Texture2D) as Texture2D;
            m_ShaderProgramIcon = Session.GetEditorResource("ShaderProgramFileIcon", AssetType.Texture2D) as Texture2D;

            /*
             * Initialize
             */
            m_SelectedObject = null;
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

            bool isClickedEmpty = true;
            bool isClicked = false;

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
             * Folder Rename event
             */
            if(m_SelectedObject != null)
            {
                if(ImGui.IsKeyPressed((int)Vex.Input.Keys.F2) && m_SelectedObject.GetType() == typeof(DomainFolderView))
                {
                    GUIRenderCommands.SignalPopupCreate("Domain_Folder_Rename");
                }

            }


            /*
             * Render folder rename popup
             */
            if(GUIRenderCommands.CreatePopup("Domain_Folder_Rename"))
            {
                CreateFolderRenamePopup(m_SelectedObject as DomainFolderView);
                GUIRenderCommands.FinalizePopup();
            }

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
                 * Get folder anchor position
                 */
                Vector2 folderAnchorPos = GUILayoutCommands.GetCursor();

                /*
                * Draw image
                */
                GUIRenderCommands.CreateImage(m_FolderIcon, subFolder.FolderPath, new Vector2(128, 128));

                /*
                 * Set cursor back to its position
                 */
                GUILayoutCommands.SetCursorPos(folderAnchorPos);

                /*
                 * Validate and draw selecteable
                 */
                if (m_SelectedObject == subFolder)
                {
                    ImGui.Selectable("##" + subFolder.FolderPath, m_SelectedObject == subFolder, ImGuiSelectableFlags.None, new Vector2(128, 128));
                }

                /*
                 * Set click event on folder
                 */
                if (GUIEventCommands.IsCurrentItemDoubleClicked() && GUIEventCommands.IsCurrentItemHavored())
                {
                    m_CurrentFolder = subFolder;
                    m_SelectedObject = null;
                    Console.WriteLine("Double clikced: " + subFolder.Name);
                    isClickedEmpty = false;
                }
                else if(GUIEventCommands.IsMouseLeftButtonClicked() && GUIEventCommands.IsCurrentItemHavored())
                {
                    //m_CurrentFolder = subFolder;
                    m_SelectedObject = subFolder;
                    Console.WriteLine("Folder clicked: " + subFolder.Name);
                    isClickedEmpty = false;
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
                 * Try load file
                 */
                file.TryLoad(Session);

                /*
                 * Get anchor
                 */
                Vector2 fileAnchor = GUILayoutCommands.GetCursor();

                /*
                 * Draw image
                 */
                if (file.AssetType == AssetType.Texture2D)
                {
                    GUIRenderCommands.CreateImage(m_Texture2DIcon, new Vector2(128, 128));
                }
                else if (file.AssetType == AssetType.Shader)
                {
                    GUIRenderCommands.CreateImage(m_ShaderIcon, new Vector2(128, 128));
                }
                else if (file.AssetType == AssetType.ShaderProgram)
                {
                    GUIRenderCommands.CreateImage(m_ShaderProgramIcon, new Vector2(128, 128));
                }

                /*
                 * Set anchor back
                 */
                GUILayoutCommands.SetCursorPos(fileAnchor);

                /*
                 * Validate and draw selecteable
                 */
                if (m_SelectedObject == file)
                {
                    ImGui.Selectable("##" + file.AssetAbsolutePath, m_SelectedObject == file, ImGuiSelectableFlags.None, new Vector2(128, 128));
                }

                /*
                 * Double click event
                 */
                if(GUIEventCommands.IsCurrentItemHavored() && GUIEventCommands.IsCurrentItemDoubleClicked())
                {
                    Console.WriteLine($"File {file.AssetName} is double clicked");

                    /*
                     * Validate loaded
                     */
                    file.TryLoad(Session);

                    /*
                     * Signal object
                     */
                    GUIObject.SignalNewObject(file.TargetAssetObject);

                    isClickedEmpty = false;
                }
                else if (GUIEventCommands.IsMouseLeftButtonClicked() && GUIEventCommands.IsCurrentItemHavored())
                {
                    m_SelectedObject = file;
                    Console.WriteLine("File clicked: " + file.AssetName);

                    isClickedEmpty = false;
                }

                /*
                 * Set texture to center of the file icon
                 */
                float charSize = ImGui.CalcTextSize(file.AssetName).X;
                float textLengthInPixels = charSize * GUILayoutCommands.GetCurrentFontSize() * 1.3333f / 2.0f;
                float halfTextSize = (float)textLengthInPixels / 2.0f;
                float offset = 48.0f - halfTextSize;
                offset = offset < 0 ? 0 : offset;
                GUILayoutCommands.SetCursorPos(fileAnchor + new Vector2(offset, 128));
                GUIRenderCommands.CreateText(file.AssetName, file.AssetID.ToString());
                GUILayoutCommands.SetCursorPos(fileAnchor + new Vector2(128, 0));
            }

            /*
             * Valdate if create menu required
             */
            if (GUIEventCommands.IsMouseRightButtonClicked())
            {
                GUIRenderCommands.SignalPopupCreate("Domain_Create_Asset");
            }


            /*
             * Render create asset popup
             */
            bool isCreateShader = false;
            bool isFolderCreate = false;
            bool isShaderProgramCreate = false;
            if (GUIRenderCommands.CreatePopup("Domain_Create_Asset"))
            {
                RenderAssetCreatePopup(ref isCreateShader,ref isFolderCreate,ref isShaderProgramCreate);
                GUIRenderCommands.FinalizePopup();
            }

            /*
             * Open create shader popup
             */
            if (isCreateShader)
                GUIRenderCommands.SignalPopupCreate("Domain_Create_Shader");

            /*
             * Open create folder popup
             */
            if (isFolderCreate)
                GUIRenderCommands.SignalPopupCreate("Domain_Create_Folder");

            /*
             * Open create shader program popup
             */
            if (isShaderProgramCreate)
                GUIRenderCommands.SignalPopupCreate("Domain_Create_ShaderProgram");

            /*
             * Render create shader popup
             */
            if (GUIRenderCommands.CreatePopup("Domain_Create_Shader"))
            {
                RenderShaderCreatePopup();
                GUIRenderCommands.FinalizePopup();
            }
            else if(GUIRenderCommands.CreatePopup("Domain_Create_Folder"))
            {
                RenderFolderCreatePopup();
                GUIRenderCommands.FinalizePopup();
            }
            else if(GUIRenderCommands.CreatePopup("Domain_Create_ShaderProgram"))
            {
                CreateShaderProgramCreatePopup();
                GUIRenderCommands.FinalizePopup();
            }

            /*
            * Validate clikc to void
            */
            if (GUIEventCommands.IsMouseLeftButtonClicked() && isClickedEmpty)
            {
                Console.WriteLine("Clicked to empty space");
                m_SelectedObject = false;
            }

        }
        private void RenderAssetCreatePopup(ref bool isCreateShader,ref bool isFolderCreate,ref bool isShaderProgramCreate)
        {
            if (GUIRenderCommands.CreateMenu("Create",""))
            {
                if (GUIRenderCommands.CreateMenu("Graphics",""))
                {
                    if (GUIRenderCommands.CreateMenuItem("Shader",""))
                    {
                        Console.WriteLine("Create shader popup");
                        isCreateShader = true;
                    }
                    if (GUIRenderCommands.CreateMenuItem("Shader Program",""))
                    {
                        isShaderProgramCreate = true;
                    }
                    GUIRenderCommands.FinalizeMenu();
                }
                if(GUIRenderCommands.CreateMenu("Misc",""))
                {
                    if(GUIRenderCommands.CreateMenuItem("Folder"," "))
                    {
                        isFolderCreate = true;
                    }
                    GUIRenderCommands.FinalizeMenu();
                }
                GUIRenderCommands.FinalizeMenu();
            }
        }
        private void RenderFolderCreatePopup()
        {
            /*
             * Render text
             */
            GUIRenderCommands.CreateText("Create folder", " ");
            GUIRenderCommands.CreateTextInput(" ", " ", ref m_CreateFolderNameInput);
            if(GUIRenderCommands.CreateButton("Create",""))
            {
                /*
                 * Create new physical folder
                 */
                string pathAndName = m_CurrentFolder.FolderPath +@"\"+ m_CreateFolderNameInput;
                Directory.CreateDirectory(pathAndName);

                /*
                 * Create new domain folder view
                 */
                m_CurrentFolder.CreateNewSubFolder(m_CreateFolderNameInput);

                /*
                 * Terminate popup
                 */
                GUIRenderCommands.TerminateCurrentPopup();
            }
        }
        private void RenderShaderCreatePopup()
        {
            /*
             * Render header
             */
            GUIRenderCommands.CreateText("Create a shader","");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateTextInput("","", ref m_CreateShaderNameInput, m_InputBuffer);
            GUILayoutCommands.StayOnSameLine();

            /*
             * Render combo selection of shader stage
             */
            if (GUIRenderCommands.CreateCombo("##Combo", m_ShaderStage.ToString(),""))
            {
                if (GUIRenderCommands.CreateSelectableItem("Vertex"," "))
                {
                    m_ShaderStage = ShaderStage.Vertex;
                }
                if (GUIRenderCommands.CreateSelectableItem("Fragment",""))
                {
                    m_ShaderStage = ShaderStage.Fragment;
                }
                if (GUIRenderCommands.CreateSelectableItem("Geometry",""))
                {
                    m_ShaderStage = ShaderStage.Geometry;
                }
                GUIRenderCommands.FinalizeCombo();
            }

            /*
             * Render create button
             */
            if (GUIRenderCommands.CreateButton("Create"," "))
            {
                Session.CreateShaderDomainContent(m_CurrentFolder,m_CreateShaderNameInput,m_ShaderStage,"");
                Console.WriteLine("Shader created");
                GUIRenderCommands.TerminateCurrentPopup();
                m_ShaderStage = ShaderStage.Vertex;
            }
        }
        private void CreateShaderProgramCreatePopup()
        {
            GUIRenderCommands.CreateText("Create a shader", "");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateText("Name", " ");
            GUIRenderCommands.CreateTextInput("", "n", ref m_CreateShaderProgramNameInput);
            GUIRenderCommands.CreateText("Category"," ");
            GUIRenderCommands.CreateTextInput("", "c", ref m_CreateShaderProgramCategoryInput);
            GUIRenderCommands.CreateText("Category Name", " ");
            GUIRenderCommands.CreateTextInput("", "cn", ref m_CreateShaderProgramCategoryNameInput);

            if(GUIRenderCommands.CreateButton("Create"," "))
            {
                Session.CreateShaderProgramContent(m_CurrentFolder, m_CreateShaderProgramNameInput, m_CreateShaderProgramCategoryInput, m_CreateShaderProgramCategoryNameInput);
                GUIRenderCommands.TerminateCurrentPopup();
            }
        }
        private void CreateFolderRenamePopup(DomainFolderView folder)
        {
            GUIRenderCommands.CreateText("Folder rename", "frnm");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateTextInput("", "frnmi", ref m_FolderRenameInput);
            if(GUIRenderCommands.CreateButton("Apply","applyFrnmi"))
            {
                folder.Rename(m_FolderRenameInput,Session);
                GUIRenderCommands.TerminateCurrentPopup();
            }
        }
        private ShaderStage m_ShaderStage = ShaderStage.Vertex;
        private string m_CreateShaderNameInput = string.Empty;

        private string m_CreateFolderNameInput = string.Empty;

        private string m_CreateShaderProgramCategoryInput = string.Empty;
        private string m_CreateShaderProgramCategoryNameInput = string.Empty;
        private string m_CreateShaderProgramNameInput = string.Empty;

        private string m_FolderRenameInput = string.Empty;

        private uint m_InputBuffer = 24;
        private DomainFolderView m_CurrentFolder;
        private DomainView m_Domain;
        private Texture2D m_FolderIcon;
        private Texture2D m_Texture2DIcon;
        private Texture2D m_BackButtonIcon;
        private Texture2D m_ShaderIcon;
        private Texture2D m_ShaderProgramIcon;
        private object m_SelectedObject;
    }
}
