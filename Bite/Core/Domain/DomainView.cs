using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Bite.Core
{
    public class DomainView
    {
        public DomainView(string domainPath)
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
        public DomainFolderView RootFolder
        {
            get
            {
                return m_RootFolder;
            }
        }
        private DomainFolderView CollectDomainFolders(string domainFolderPath)
        {
            /*
             * collect folders
             */
            string[] folderPaths = Directory.GetDirectories(domainFolderPath + @"\");

            /*
             * Collect files
             */
            string[] files = Directory.GetFiles(domainFolderPath + @"\","*.rdefinition");

            /*
             * Create this domainfolder
             */
            DomainFolderView folder = new DomainFolderView(null,domainFolderPath + @"\",files, folderPaths);

            return folder;
        }

        internal void Shutdown()
        {

        }

        DomainFolderView m_RootFolder;
        private string m_DomainPath;
    }
}
