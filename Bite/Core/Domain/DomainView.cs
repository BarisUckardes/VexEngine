using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Bite.Core
{
    /// <summary>
    /// A view which represents the physical domain
    /// </summary>
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

        /// <summary>
        /// Returns the main/root folder of the domain
        /// </summary>
        public DomainFolderView RootFolder
        {
            get
            {
                return m_RootFolder;
            }
        }

        /// <summary>
        /// Returns the domain folders
        /// </summary>
        /// <param name="domainFolderPath"></param>
        /// <returns></returns>
        private DomainFolderView CollectDomainFolders(string domainFolderPath)
        {
            /*
             * collect folders
             */
            string[] folderPaths = Directory.GetDirectories(domainFolderPath + @"\");
          
            /*
             * Collect files
             */
            string[] files = Directory.GetFiles(domainFolderPath + @"\","*.vdefinition");
            for(int i = 0;i<files.Length;i++)
            {
                Console.WriteLine("File: " + files[i]);
            }

            /*
             * Create this domainfolder
             */
            DomainFolderView folder = new DomainFolderView(null,domainFolderPath + @"\",files, folderPaths);

            return folder;
        }

        /// <summary>
        /// Disposes the domain
        /// </summary>
        internal void Shutdown()
        {

        }

        DomainFolderView m_RootFolder;
        private string m_DomainPath;
    }
}
