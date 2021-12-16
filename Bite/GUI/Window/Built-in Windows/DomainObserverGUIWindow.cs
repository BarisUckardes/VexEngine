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
using Vex.Framework;

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
             * Get texture2d resources
             */
            m_FolderIcon = Session.GetEditorResource("FolderIcon",AssetType.Texture2D) as Texture2D;
            m_BackButtonIcon = Session.GetEditorResource("BackButtonIcon", AssetType.Texture2D) as Texture2D;
            m_ShaderIcon = Session.GetEditorResource("ShaderFileIcon", AssetType.Texture2D) as Texture2D;
            m_ShaderProgramIcon = Session.GetEditorResource("ShaderProgramFileIcon", AssetType.Texture2D) as Texture2D;
            m_MaterialIcon = Session.GetEditorResource("MaterialFileIcon", AssetType.Texture2D) as Texture2D;
            m_Texture2DIcon = Session.GetEditorResource("Texture2DFileIcon", AssetType.Texture2D) as Texture2D;
            m_ComputerPathIcon = Session.GetEditorResource("ComputerPathIcon", AssetType.Texture2D) as Texture2D;
            m_StaticMeshFileIcon = Session.GetEditorResource("MeshFileIcon", AssetType.Texture2D) as Texture2D;
            m_WorldFileIcon = Session.GetEditorResource("WorldFileIcon", AssetType.Texture2D) as Texture2D;
            m_DeleteIcon = Session.GetEditorResource("DeleteIcon", AssetType.Texture2D) as Texture2D;
            m_EditIcon = Session.GetEditorResource("EditIcon", AssetType.Texture2D) as Texture2D;


            /*
             * Get texture format resources
             */
            m_TextureFormatEnumNames = Enum.GetNames(typeof(TextureFormat));
            m_TextureInternalFormatEnumNames = Enum.GetNames(typeof(TextureInternalFormat));
            m_SelectedTextureFormat = TextureFormat.None;
            m_SelectedTextureInternalFormat = TextureInternalFormat.None;

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
                if (GUIEventCommands.IsCurrentItemClicked() & GUIEventCommands.IsWindowHovered())
                    m_CurrentFolder = folder.ParentFolder;
            }
            GUILayoutCommands.StayOnSameLine();
            GUIRenderCommands.CreateText(folder.Name.Replace(PlatformPaths.DomainDirectory,""),"");

            /*
             * Render import
             */
            GUILayoutCommands.StayOnSameLine();
            bool isImportTexture2D = false;
            bool isImportStaticMesh = false;
            if(GUIRenderCommands.CreateButton("Import","import"))
            {
                GUIRenderCommands.SignalPopupCreate("Domain_Import");
            }
           

            /*
             * Render import menu
             */
            if(GUIRenderCommands.CreatePopup("Domain_Import"))
            {
                RenderAssetImportPopup(ref isImportTexture2D,ref isImportStaticMesh);
                GUIRenderCommands.FinalizePopup();
            }

            /*
             * Try first time create import 2d texture2d
             */
            if(isImportTexture2D)
            {
                m_Texture2DOpenFileDialog = GUIRenderCommands.CreateOpenFileDialog(new List<string>() { "*.jpg","*.png" },"Import",m_FolderIcon,m_Texture2DIcon);
            }
            else if(isImportStaticMesh)
            {
                m_StaticMeshOpenFileDialog = GUIRenderCommands.CreateOpenFileDialog(new List<string>() { "*.obj", "*.fbx" }, "Import", m_FolderIcon, m_Texture2DIcon);
            }

            /*
             * Render open file dialog for texture2d
             */
            m_Texture2DOpenFileDialog?.Render();

            /*
             * Render open file dialog for static mesh
             */
            m_StaticMeshOpenFileDialog?.Render();

            /*
             * Validate texture2d selected
             */
            if(m_Texture2DOpenFileDialog != null && m_Texture2DOpenFileDialog.SelectedPath != string.Empty)
            {
                /*
                 * Validate file
                 */
                if(!File.Exists(m_Texture2DOpenFileDialog.SelectedPath))
                {
                    Console.WriteLine("Selected image file is not found!");
                }
                else
                {
                    Session.CreateTexture2DomainContent(m_CurrentFolder, m_Texture2DOpenFileDialog.SelectedFileName, m_Texture2DOpenFileDialog.SelectedPath);
                }

                /*
                 * Close file dialog
                 */
                GUIRenderCommands.CloseOpenFileDialog(m_Texture2DOpenFileDialog);
                m_Texture2DOpenFileDialog = null;
            }

            /*
             * Validate static mesh selected
             */
            if (m_StaticMeshOpenFileDialog != null && m_StaticMeshOpenFileDialog.SelectedPath != string.Empty)
            {
                /*
                 * Validate file
                 */
                if (!File.Exists(m_StaticMeshOpenFileDialog.SelectedPath))
                {
                    Console.WriteLine("Selected static mesh file is not found!");
                }
                else
                {
                    Session.CreateStaticMeshDomainContent(m_CurrentFolder, m_StaticMeshOpenFileDialog.SelectedFileName, m_StaticMeshOpenFileDialog.SelectedPath);
                }

                /*
                 * Close file dialog
                 */
                GUIRenderCommands.CloseOpenFileDialog(m_StaticMeshOpenFileDialog);
                m_StaticMeshOpenFileDialog = null;
            }

            /*
             * Folder Rename event
             */
            if (m_SelectedObject != null)
            {
                if(ImGui.IsKeyPressed((int)Vex.Input.Keys.F2) && m_SelectedObject.GetType() == typeof(DomainFolderView) && GUIEventCommands.IsWindowHovered())
                {
                    GUIRenderCommands.SignalPopupCreate("Domain_Folder_Rename");
                }
                if(ImGui.IsKeyPressed((int)Vex.Input.Keys.F2) && m_SelectedObject.GetType() == typeof(DomainFileView) && GUIEventCommands.IsWindowHovered())
                {
                    GUIRenderCommands.SignalPopupCreate("Domain_File_Rename");
                    
                }
                if (ImGui.IsKeyPressed((int)Vex.Input.Keys.Delete) && m_SelectedObject.GetType() == typeof(DomainFileView) && GUIEventCommands.IsWindowHovered())
                {
                    /*
                     * Get as file view
                     */
                    DomainFileView fileView = m_SelectedObject as DomainFileView;

                    /*
                     * Delete file
                     */
                    m_CurrentFolder.DeleteFile(fileView.AssetID,Session);

                    /*
                     * Set null
                     */
                    m_SelectedObject = null;
                }
                if (ImGui.IsKeyPressed((int)Vex.Input.Keys.Delete) && m_SelectedObject.GetType() == typeof(DomainFolderView) && GUIEventCommands.IsWindowHovered())
                {
                    /*
                     * Get as file view
                     */
                    DomainFolderView folderView = m_SelectedObject as DomainFolderView;

                    /*
                     * Delete file
                     */
                    folderView.Delete(Session);

                    /*
                     * Set null
                     */
                    m_SelectedObject = null;
                }
            }

            /*
             * Create any item hovered state
             */
            bool folderHovered = false;
            bool fileHovered = false;

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
                 * Validate and set folder havored
                 */
                if (folderHovered == false)
                    folderHovered = GUIEventCommands.IsCurrentItemHavored();

                /*
                 * Validate and set hovered folder
                 */
                if (GUIEventCommands.IsCurrentItemHavored())
                    m_HoveredFolder = subFolder;

                /*
                 * Set click event on folder
                 */
                if (GUIEventCommands.IsCurrentItemDoubleClicked() && GUIEventCommands.IsCurrentItemHavored())
                {
                    m_CurrentFolder = subFolder;
                    isClickedEmpty = false;
                }
                else if(GUIEventCommands.IsMouseLeftButtonClicked() && GUIEventCommands.IsCurrentItemHavored() && GUIEventCommands.IsWindowHovered())
                {
                    m_SelectedObject = subFolder;
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
                    /*
                     * Create flipped uvs
                     */
                    Vector2 uv0 = new Vector2(0, 1);
                    Vector2 uv1 = new Vector2(1, 0);

                    /*
                     * Validate load
                     */
                    if (file.TargetAssetObject == null)
                        GUIRenderCommands.CreateImage(m_Texture2DIcon, new Vector2(128, 128),uv0,uv1);
                    else
                        GUIRenderCommands.CreateImage((file.TargetAssetObject as Texture2D), new Vector2(128, 128), uv0, uv1);
                }
                else if (file.AssetType == AssetType.Shader)
                {
                    GUIRenderCommands.CreateImage(m_ShaderIcon, new Vector2(128, 128));
                }
                else if (file.AssetType == AssetType.ShaderProgram)
                {
                    GUIRenderCommands.CreateImage(m_ShaderProgramIcon, new Vector2(128, 128));
                }
                else if(file.AssetType == AssetType.Material)
                {
                    GUIRenderCommands.CreateImage(m_MaterialIcon, new Vector2(128, 128));
                }
                else if (file.AssetType == AssetType.Mesh)
                {
                    GUIRenderCommands.CreateImage(m_StaticMeshFileIcon, new Vector2(128, 128));
                }
                else if (file.AssetType == AssetType.World)
                {
                    GUIRenderCommands.CreateImage(m_WorldFileIcon, new Vector2(128, 128));
                }

                /*
                 *Set havor state
                 */
                if (fileHovered == false)
                    fileHovered = GUIEventCommands.IsCurrentItemHavored();

                /*
                 * Validate and set hovered file
                 */
                if (GUIEventCommands.IsCurrentItemHavored())
                    m_HoveredFile = file;

                /*
                 * Set anchor back
                 */
                GUILayoutCommands.SetCursorPos(fileAnchor);

                /*
                 * Validate and draw selecteable
                 */
                if (m_SelectedObject == file)
                {
                    //ImGui.Selectable("##" + file.AssetAbsolutePath, true, ImGuiSelectableFlags.None, new Vector2(128, 128));
                }

                /*
                 * Create dragable field
                 */
                GUIRenderCommands.CreateObjectField(file.TargetAssetObject, "file_" + file.AssetID.ToString(), new Vector2(128, 128));

                /*
                 * Double click event
                 */
                if (GUIEventCommands.IsCurrentItemHavored() && GUIEventCommands.IsCurrentItemDoubleClicked())
                {
                    Console.WriteLine("DOUBLE clicked");
                    /*
                     * Validate loaded
                     */
                    file.TryLoad(Session);

                    /*
                     * Signal object
                     */
                    GUIObject.SignalNewObject(file.TargetAssetObject);

                    /*
                     * Try open if its a world
                     */
                    if (file.AssetType == AssetType.World)
                    {
                        if(!Session.GamePlayState)
                        {
                            /*
                            * First try save the current world
                            */
                            Session.UpdateDomainAsset(Session.EditorRootWorldID, Session.CurrentWorld);

                            /*
                             * Setup the target world for editor and vex
                             */
                            Session.SetupEditorWorld(file.AssetID);
                        }
                        else
                        {
                            Console.WriteLine("You cannot save world in the play mode");
                        }
                        
                    }

                    isClickedEmpty = false;
                }
                else if (GUIEventCommands.IsMouseLeftButtonClicked() && GUIEventCommands.IsCurrentItemHavored())
                {
                    m_SelectedObject = file;
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
            if (GUIEventCommands.IsMouseRightButtonClicked() && GUIEventCommands.IsWindowHovered() && !folderHovered && !fileHovered)
            {
                GUIRenderCommands.SignalPopupCreate("Domain_Create_Asset");
            }

            /*
             * Validate if folder quick menu required
             */
            if(folderHovered && GUIEventCommands.IsMouseRightButtonClicked())
            {
                GUIRenderCommands.SignalPopupCreate("Folder_Quick_Menu");
            }

            /*
             * Validate if file quick menu required
             */
            if(fileHovered && GUIEventCommands.IsMouseRightButtonClicked())
            {
                GUIRenderCommands.SignalPopupCreate("File_Quick_Menu");
            }


            /*
             * Render folder quick menu
             */
            bool isFolderRenameQuick = false;
            if(GUIRenderCommands.CreatePopup("Folder_Quick_Menu"))
            {
                GUIRenderCommands.CreateImage(m_DeleteIcon, new Vector2(16, 16));
                GUILayoutCommands.StayOnSameLine();
                if (GUIRenderCommands.CreateSelectableItem("Delete","delete_folder"))
                {
                    /*
                     * Get as file view
                     */
                    DomainFolderView folderView = m_HoveredFolder;

                    /*
                     * Delete file
                     */
                    folderView.Delete(Session);

                    /*
                     * Set null
                     */
                    m_HoveredFolder = null;
                    GUIRenderCommands.TerminateCurrentPopup();
                }
                GUIRenderCommands.CreateImage(m_EditIcon, new Vector2(16, 16));
                GUILayoutCommands.StayOnSameLine();
                if (GUIRenderCommands.CreateSelectableItem("Rename", "rename_folder"))
                {
                    GUIRenderCommands.SignalPopupCreate("Domain_Folder_Rename");
                    GUIRenderCommands.TerminateCurrentPopup();
                    isFolderRenameQuick = true;
                }
                GUIRenderCommands.FinalizePopup();
            }


            /*
             * Render file quick menu
             */
            bool isFileRenameQuick = false;
            if (GUIRenderCommands.CreatePopup("File_Quick_Menu"))
            {
                GUIRenderCommands.CreateImage(m_DeleteIcon, new Vector2(16, 16));
                GUILayoutCommands.StayOnSameLine();
                if (GUIRenderCommands.CreateSelectableItem("Delete", "delete_file"))
                {
                    /*
                     * Get as file view
                     */
                    DomainFileView fileView = m_HoveredFile;

                    /*
                     * Delete file
                     */
                    fileView.Delete(Session);

                    /*
                     * Set null
                     */
                    m_HoveredFile = null;
                    GUIRenderCommands.TerminateCurrentPopup();
                }
                GUIRenderCommands.CreateImage(m_EditIcon, new Vector2(16, 16));
                GUILayoutCommands.StayOnSameLine();
                if (GUIRenderCommands.CreateSelectableItem("Rename", "rename_file"))
                {
                    GUIRenderCommands.SignalPopupCreate("Domain_File_Rename");
                    GUIRenderCommands.TerminateCurrentPopup();
                    isFolderRenameQuick = true;
                }
                GUIRenderCommands.FinalizePopup();
            }

            /*
             * Open folder rename popup
             */
            if (isFolderRenameQuick)
                GUIRenderCommands.SignalPopupCreate("Domain_Folder_Rename");

            /*
            * Open file rename popup
            */
            if (isFileRenameQuick)
                GUIRenderCommands.SignalPopupCreate("Domain_File_Rename");

            /*
             * Render create asset popup
             */
            bool isCreateShader = false;
            bool isFolderCreate = false;
            bool isShaderProgramCreate = false;
            bool isMaterialCreate = false;
            bool isWorldCreate = false;
            if (GUIRenderCommands.CreatePopup("Domain_Create_Asset"))
            {
                RenderAssetCreatePopup(ref isCreateShader,ref isFolderCreate,ref isShaderProgramCreate,ref isMaterialCreate,ref  isWorldCreate);
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
             * Open create material popup
             */
            if (isMaterialCreate)
                GUIRenderCommands.SignalPopupCreate("Domain_Create_Material");

            /*
            * Open create material popup
            */
            if (isWorldCreate)
                GUIRenderCommands.SignalPopupCreate("Domain_Create_World");

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
            else if (GUIRenderCommands.CreatePopup("Domain_Create_Material"))
            {
                CreateMaterialCreatePopup();
                GUIRenderCommands.FinalizePopup();
            }
            else if (GUIRenderCommands.CreatePopup("Domain_Create_World"))
            {
                RenderWorldCreatePopup();
                GUIRenderCommands.FinalizePopup();
            }


            /*
             * Render folder rename popup
             */
            if (GUIRenderCommands.CreatePopup("Domain_Folder_Rename"))
            {
                Console.WriteLine("Folder:");
                CreateFolderRenamePopup(isFolderRenameQuick == true ? m_HoveredFolder : m_SelectedObject as DomainFolderView);
                GUIRenderCommands.FinalizePopup();
            }

            /*
             * Render file rename popup
             */
            if (GUIRenderCommands.CreatePopup("Domain_File_Rename"))
            {
                CreateFileRenamePopup(isFileRenameQuick == true ? m_HoveredFile : m_SelectedObject as DomainFileView);
                GUIRenderCommands.FinalizePopup();
            }

            /*
            * Validate click to void
            */
            if (GUIEventCommands.IsMouseLeftButtonClicked() && isClickedEmpty && GUIEventCommands.IsWindowHovered())
            {
                m_SelectedObject = false;
            }
        }
        private void RenderAssetImportPopup(ref bool texture2DImport,ref bool staticMeshImport)
        {
            if (GUIRenderCommands.CreateMenu("Import", ""))
            {
                if (GUIRenderCommands.CreateMenu("Graphics", ""))
                {
                   
                    if (GUIRenderCommands.CreateMenuItem("Texture2D", ""))
                    {
                        GUIRenderCommands.TerminateCurrentPopup();
                        texture2DImport = true;
                    }
                    else if (GUIRenderCommands.CreateMenuItem("Static Mesh", ""))
                    {
                        GUIRenderCommands.TerminateCurrentPopup();
                        staticMeshImport = true;
                    }

                    GUIRenderCommands.FinalizeMenu();
                }
                GUIRenderCommands.FinalizeMenu();
            }
        }
        private void RenderAssetCreatePopup(ref bool isCreateShader,ref bool isFolderCreate,ref bool isShaderProgramCreate,ref bool isMaterialCreate,ref bool isCreateWorld)
        {
            if (GUIRenderCommands.CreateMenu("Create",""))
            {
                if (GUIRenderCommands.CreateMenu("Graphics",""))
                {
                    if (GUIRenderCommands.CreateMenuItem("Shader",""))
                    {
                        isCreateShader = true;
                    }
                    if (GUIRenderCommands.CreateMenuItem("Shader Program",""))
                    {
                        isShaderProgramCreate = true;
                    }
                    if (GUIRenderCommands.CreateMenuItem("Material", ""))
                    {
                        isMaterialCreate = true;
                    }
                   
                    GUIRenderCommands.FinalizeMenu();
                }
                if(GUIRenderCommands.CreateMenu("Domain",""))
                {
                    if(GUIRenderCommands.CreateMenuItem("Folder"," "))
                    {
                        isFolderCreate = true;
                    }
                    GUIRenderCommands.FinalizeMenu();
                }
                if (GUIRenderCommands.CreateMenu("World", ""))
                {
                    if (GUIRenderCommands.CreateMenuItem("World", " "))
                    {
                        isCreateWorld = true;
                    }
                    GUIRenderCommands.FinalizeMenu();
                }
                GUIRenderCommands.FinalizeMenu();
            }
        }
        private void RenderWorldCreatePopup()
        {
            GUIRenderCommands.CreateText("Create World", " ");
            GUIRenderCommands.CreateTextInput(" ", " ", ref m_WorldNameInput);
            if (GUIRenderCommands.CreateButton("Create", "create_world"))
            {
                /*
                 * Create default template world
                 */
                Session.CreateWorldDomainAsset(m_CurrentFolder, m_WorldNameInput);

                /*
                 * Clear input
                 */
                m_WorldNameInput = string.Empty;

                /*
                 * Terminate popup
                 */
                GUIRenderCommands.TerminateCurrentPopup();
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
                m_CreateShaderNameInput = string.Empty;
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
                Session.CreateShaderProgramDomainContent(m_CurrentFolder, m_CreateShaderProgramNameInput, m_CreateShaderProgramCategoryInput, m_CreateShaderProgramCategoryNameInput);
                GUIRenderCommands.TerminateCurrentPopup();
                m_CreateShaderProgramNameInput = string.Empty;
                m_CreateShaderProgramCategoryInput = string.Empty;
                m_CreateShaderProgramCategoryNameInput = string.Empty;
            }
        }

        /// <summary>
        /// A popup which handles the creation of a material asset
        /// </summary>
        private void CreateMaterialCreatePopup()
        {
            GUIRenderCommands.CreateText("Create a Material", "");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateText("Name", " ");
            GUIRenderCommands.CreateTextInput("", "n", ref m_CreateMaterialNameInput);

            if (GUIRenderCommands.CreateButton("Create", " "))
            {
                Session.CreateMaterialDomianContent(m_CurrentFolder, m_CreateMaterialNameInput);
                GUIRenderCommands.TerminateCurrentPopup();
                m_CreateMaterialNameInput = string.Empty;
            }
        }

        /// <summary>
        /// A popup which handles the renaming of the target folder
        /// </summary>
        /// <param name="folder"></param>
        private void CreateFolderRenamePopup(DomainFolderView folder)
        {
            if(folder == null)
            {
                Console.WriteLine("NULL FOLDER");
                return;
            }
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

        /// <summary>
        /// A popup which handles the renaming of the target file
        /// </summary>
        /// <param name="folder"></param>
        private void CreateFileRenamePopup(DomainFileView file)
        {
            GUIRenderCommands.CreateText("File rename", "frnm");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateTextInput("", "frnmi", ref m_FolderRenameInput);
            if (GUIRenderCommands.CreateButton("Apply", "applyFrnmi"))
            {
                file.Rename(m_FolderRenameInput,Session);
                GUIRenderCommands.TerminateCurrentPopup();
            }
        }


        /*
         * Create resources
         */
        private ShaderStage m_ShaderStage = ShaderStage.Vertex;
        private string m_CreateShaderNameInput = string.Empty;

        /*
         * Create folder resources
         */
        private string m_CreateFolderNameInput = string.Empty;

        /*
         * Create shader program resources
         */
        private string m_CreateShaderProgramCategoryInput = string.Empty;
        private string m_CreateShaderProgramCategoryNameInput = string.Empty;
        private string m_CreateShaderProgramNameInput = string.Empty;

        /*
         * Create material resources
         */
        private string m_CreateMaterialNameInput = string.Empty;

        /*
         * Create world resources
         */
        private string m_WorldNameInput = string.Empty;

        /*
         * Rename folder resources
         */
        private string m_FolderRenameInput = string.Empty;

        /*
         * Rename file resources
         */
        private string m_FileRenameInput = string.Empty;

        private uint m_InputBuffer = 24;

        private DomainFolderView m_CurrentFolder;
        private DomainView m_Domain;

        private DomainFolderView m_HoveredFolder;
        private DomainFileView m_HoveredFile;

        private object m_SelectedObject;

        /*
         * Static mesh resources
         */
        private GUIOpenFileDialogHandle m_StaticMeshOpenFileDialog = null;

        /*
         * Texture2d resources
         */
        private GUIOpenFileDialogHandle m_Texture2DOpenFileDialog = null;
        private string[] m_TextureFormatEnumNames;
        private string[] m_TextureInternalFormatEnumNames;
        private TextureFormat m_SelectedTextureFormat;
        private TextureInternalFormat m_SelectedTextureInternalFormat;
        private string m_Texture2DName = string.Empty;
        private int m_Texture2DWidth;
        private int m_Texture2DHeight;

        /*
        * Texture2D resources
        */
        private Texture2D m_FolderIcon;
        private Texture2D m_BackButtonIcon;
        private Texture2D m_ShaderIcon;
        private Texture2D m_ShaderProgramIcon;
        private Texture2D m_MaterialIcon;
        private Texture2D m_Texture2DIcon;
        private Texture2D m_ComputerPathIcon;
        private Texture2D m_StaticMeshFileIcon;
        private Texture2D m_WorldFileIcon;
        private Texture2D m_EditIcon;
        private Texture2D m_DeleteIcon;
    }
}
