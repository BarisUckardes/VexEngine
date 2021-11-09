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
            m_RootFolder = CollectDomainFolders(domainPath);
        }
        public DomainFolder RootFolder
        {
            get
            {
                return m_RootFolder;
            }
        }
        private DomainFolder CollectDomainFolders(string domainFolderPath)
        {
            /*
             * collect folders
             */
            string[] folderPaths = Directory.GetDirectories(domainFolderPath + @"\");

            /*
             * Collect files
             */
            string[] files = Directory.GetFiles(domainFolderPath + @"\");

            /*
             * Create this domainfolder
             */
            DomainFolder folder = new DomainFolder(null,domainFolderPath + @"\",files, folderPaths);

            return folder;
        }

        internal void Shutdown()
        {

        }

        DomainFolder m_RootFolder;
        private string m_DomainPath;
    }
}
