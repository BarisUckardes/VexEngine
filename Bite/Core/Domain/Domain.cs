using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Bite.Core
{
    public class Domain
    {
        public Domain(string domainPath)
        {
            /*
             * Set local variables
             */
            m_DomainPath = domainPath;

            /*
             * Collect domain folders
             */
            m_VexotFolder = CollectDomainFolders(domainPath);
        }

        private DomainFolder CollectDomainFolders(string domainFolderPath)
        {
            /*
             * collect folders
             */
            string[] folderPaths = Directory.GetDirectories(domainFolderPath);

            /*
             * Collect files
             */
            string[] files = Directory.GetFiles(domainFolderPath);

            /*
             * Create this domainfolder
             */
            DomainFolder folder = new DomainFolder(folderPaths, files);


            return folder;

        }


        DomainFolder m_VexotFolder;
        private string m_DomainPath;
    }
}
