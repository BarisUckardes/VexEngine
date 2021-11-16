using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vex.Asset;
namespace Bite.Core
{
    public class DomainFolderView
    {
        public DomainFolderView(DomainFolderView parentFolder,string selfPath,string[] files,string[] folders)
        {
            /*
             * Initialize local variables
             */
            m_SubFolders = new List<DomainFolderView>();
            m_Files = new List<DomainFileView>();
            m_Name = Path.GetFileName(Path.GetDirectoryName(selfPath+@"\"));
            m_Path = selfPath;
            m_ParentFolder = parentFolder;
            m_ID = Guid.NewGuid();

           
            /*
             * Collect folders
             */
            for (int folderIndex =0;folderIndex < folders.Length;folderIndex++)
            {
                /*
                 * Collect folders
                 */
                string[] subFolders = Directory.GetDirectories(folders[folderIndex]);

                /*
                 * Collect files
                 */
                string[] subFiles = Directory.GetFiles(folders[folderIndex]);

                /*
                 * Create domain folder and register it
                 */
                m_SubFolders.Add(new DomainFolderView(this,folders[folderIndex],subFiles, subFolders));
            }

            /*
             * Collect files
             */
            for (int fileIndex=0; fileIndex < files.Length; fileIndex++)
            { 
                /*
                 * Create meta data
                 */
                string fileName = Path.GetFileNameWithoutExtension(files[fileIndex]);
                string extension = Path.GetExtension(files[fileIndex]);
                string folderPath = Path.GetDirectoryName(files[fileIndex]);
                string expectedAssetPath = folderPath + @"\" + fileName + ".rasset";

                /*
                * Validate if its an asset definition file
                */
                if (extension != ".rdefinition")
                {
                    continue;
                }

                /*
                 * Load definition file content
                 */
                string definitionFileContent = File.ReadAllText(files[fileIndex]);

                /*
                 * Validate source .rasset file
                 */
                DomainFileState state;
                if (File.Exists(expectedAssetPath))
                    state = DomainFileState.Valid;
                else
                    state = DomainFileState.Missing;

                /*
                 * Read domain file definition
                 */
                AssetDefinition definition = new AssetInterface(null).GenerateAsset(AssetType.Definition,definitionFileContent) as AssetDefinition;

                /*
                 * Register domain file
                 */
                m_Files.Add(new DomainFileView(definition,state,files[fileIndex], expectedAssetPath));
            }
        }
        public IReadOnlyCollection<DomainFolderView> SubFolders
        {
            get
            {
                return m_SubFolders.AsReadOnly();
            }
        }
        public IReadOnlyCollection<DomainFileView> Files
        {
            get
            {
                return m_Files.AsReadOnly();
            }
        }
        public DomainFolderView ParentFolder
        {
            get
            {
                return m_ParentFolder;
            }
        }
        public Guid ID
        {
            get
            {
                return m_ID;
            }
        }
        public string Name
        {
            get
            {
                return m_Name;
            }
        }
        public string FolderPath
        {
            get
            {
                return m_Path;
            }
        }

      
        private List<DomainFolderView> m_SubFolders;
        private List<DomainFileView> m_Files;
        private DomainFolderView m_ParentFolder;
        private Guid m_ID;
        private string m_Name;
        private string m_Path;
    }
}
