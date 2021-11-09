using Bite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
using Vex.Platform;

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
        }

        public override void OnLayoutFinalize()
        {
            m_Domain = null;
        }

        public override void OnRenderLayout()
        {
            RenderDomainView(m_CurrentFolder);
        }

        private void RenderDomainView(DomainFolder folder)
        {
            /*
             * Render path
             */
            GUIRenderCommands.CreateText(folder.Name.Replace(PlatformPaths.DomainDirectory,""),"");

            /*
             * Draw sub folders
             */
            IReadOnlyCollection<DomainFolder> subFolders = folder.SubFolders;
            for(int folderIndex = 0;folderIndex < subFolders.Count;folderIndex++)
            {
                /*
                 * Get sub folder
                 */
                DomainFolder subFolder = subFolders.ElementAt(folderIndex);

                /*
                 * Draw button
                 */
                if(GUIRenderCommands.CreateButton(subFolder.Name, ""))
                {
                    m_CurrentFolder = subFolder;
                }
            }
        }
        private DomainFolder m_CurrentFolder;
        private Domain m_Domain;
    }
}
