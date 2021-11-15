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
            m_FolderTexture = Session.GetEditorResource("FolderIcon",AssetType.Texture2D) as Texture2D;
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
             * Render path
             */
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
                 * Draw button
                 */
                if(GUIRenderCommands.CreateButton(subFolder.Name,subFolder.ID.ToString()))
                {
                    m_CurrentFolder = subFolder;
                }
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
                if(file.Definition.Type == AssetType.Texture2D)
                {
                    GUIRenderCommands.CreateImage(m_FolderTexture);
                }
            }
        }

        private DomainFolderView m_CurrentFolder;
        private DomainView m_Domain;
        private Texture2D m_FolderTexture;
    }
}
