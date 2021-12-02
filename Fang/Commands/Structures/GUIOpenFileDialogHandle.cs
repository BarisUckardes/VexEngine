using ImGuiNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;

namespace Fang.Commands
{
    public class GUIOpenFileDialogHandle
    {
        public GUIOpenFileDialogHandle(string searchFolderPath,string buttonName,List<string> targetExtensions,bool showFolders,Texture2D folderTexture,Texture2D fileTexture)
        {
            m_ShowFolders = showFolders;
            m_TargetExtensions = targetExtensions;
            m_CurrentPath = searchFolderPath;
            m_ButtonName = buttonName;
            m_FolderTexture = folderTexture;
            m_FileTexture = fileTexture;
            GUIRenderCommands.SignalPopupCreate("Open_File_Dialog");
        }
        public string SelectedPath
        {
            get
            {
                return m_SelectedPath;
            }
        }

        public string SelectedFileName
        {
            get
            {
                return m_SelectedFileName;
            }
        }
        public void Render()
        {
            /*
            * Start popup
            */
            if (GUIRenderCommands.CreatePopup("Open_File_Dialog"))
            {
                /*
                 * Validate my computer
                 */
                if (GUIRenderCommands.CreateButton("Go to computer", "my_computer"))
                {
                    
                }

                /*
                 * Render body
                 */
                GUIRenderCommands.CreateText(m_CurrentPath, "currentPathDir");
                GUIRenderCommands.CreateSeperatorLine();
                GUIRenderCommands.CreateEmptySpace();

                /*
                 * Get folders
                 */
                string[] folders = Directory.GetDirectories(m_CurrentPath);

                /*
                 * Render folders
                 */
                for (int folderIndex = 0; folderIndex < folders.Length; folderIndex++)
                {
                    /*
                     * Get folder index
                     */
                    string folderPath = folders[folderIndex];

                    /*
                     * Get folder name
                     */
                    string folderName = Path.GetFileNameWithoutExtension(folderPath);

                    /*
                     * Render folder buttons
                     */
                    GUIRenderCommands.CreateImage(m_FolderTexture, "folder-tex", new System.Numerics.Vector2(16, 16));
                    GUILayoutCommands.StayOnSameLine();
                    if(GUIRenderCommands.CreateSelectableItem(folderName, "currentPathDir"))
                    {
                        m_CurrentPath = folderPath;
                    }
                   
                }

                /*
                 * Collect files
                 */
                List<string> files = new List<string>();
                for(int extensionIndex = 0;extensionIndex < m_TargetExtensions.Count;extensionIndex++)
                {
                    files.AddRange(Directory.GetFiles(m_CurrentPath + @"\", m_TargetExtensions[extensionIndex]));
                }

                /*
                 * Render files
                 */
                for(int fileIndex = 0;fileIndex < files.Count;fileIndex++)
                {
                    /*
                    * Get file path
                    */
                    string filePath = files[fileIndex];

                    /*
                     * Get file name
                     */
                    string fileName = Path.GetFileNameWithoutExtension(filePath);

                    /*
                     * Render file
                     */
                    GUIRenderCommands.CreateImage(m_FileTexture, "file-tex", new System.Numerics.Vector2(16, 16));
                    GUILayoutCommands.StayOnSameLine();
                    if (ImGui.Selectable(fileName,m_SelectedTempPath == filePath, ImGuiSelectableFlags.DontClosePopups))
                    {
                        m_SelectedTempPath = filePath;
                        m_SelectedFileName = fileName;
                    }
                }

                /*
                 * Render button
                 */
                if(GUIRenderCommands.CreateButton(m_ButtonName,m_ButtonName + "_btn"))
                {
                    m_SelectedPath = m_SelectedTempPath;
                }
                GUIRenderCommands.FinalizePopup();
            }
        }

        internal void CloseHandle()
        {
            GUIRenderCommands.TerminateCurrentPopup();
        }
        private Texture2D m_FolderTexture;
        private Texture2D m_FileTexture;
        private List<string> m_TargetExtensions;
        private string m_SelectedPath = string.Empty;
        private string m_SelectedTempPath = string.Empty;
        private string m_SelectedFileName = string.Empty;
        private string m_CurrentPath;
        private string m_ButtonName;
        private bool m_ShowFolders;
    }
    
}
