using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vex.Asset;
using Microsoft.VisualBasic;

namespace Bite.Core
{
    /// <summary>
    /// Represents a folder view from the physical domain
    /// </summary>
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
                string expectedAssetPath = folderPath + @"\" + fileName + ".vasset";

                /*
                * Validate if its an asset definition file
                */
                if (extension != ".vdefinition")
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
                m_Files.Add(new DomainFileView(definition,this,state,files[fileIndex], expectedAssetPath));
            }
        }
        public DomainFolderView(DomainFolderView parentFolder,string seflPath,string selfName)
        {
            m_ParentFolder = parentFolder;
            m_Path = seflPath;
            m_Name = selfName;
            m_Files = new List<DomainFileView>();
            m_SubFolders = new List<DomainFolderView>();
        }

        /// <summary>
        /// Returns the sub folders of this folder view 
        /// </summary>
        public IReadOnlyCollection<DomainFolderView> SubFolders
        {
            get
            {
                return m_SubFolders.AsReadOnly();
            }
        }

        /// <summary>
        /// Returns the files of this folder view
        /// </summary>
        public IReadOnlyCollection<DomainFileView> Files
        {
            get
            {
                return m_Files.AsReadOnly();
            }
        }

        /// <summary>
        /// Returns the parent folder.(If have any)
        /// </summary>
        public DomainFolderView ParentFolder
        {
            get
            {
                return m_ParentFolder;
            }
        }

        /// <summary>
        /// Returns the unique id of this folder
        /// </summary>
        public Guid ID
        {
            get
            {
                return m_ID;
            }
        }

        /// <summary>
        /// Returns the name of the folder
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// Returns the full path of the folder
        /// </summary>
        public string FolderPath
        {
            get
            {
                return m_Path;
            }
        }

        /// <summary>
        /// Renames the folder
        /// </summary>
        /// <param name="name"></param>
        /// <param name="session"></param>
        public void Rename(string name,EditorSession session)
        {
            /*
             * Get new path
             */
            string newPath = Directory.GetParent(m_Path).FullName + @"\" + name;

            /*
             * Rename
             */
            Directory.Move(m_Path, newPath);

            /*
             * Set name
             */
            m_Name = name;

            /*
             * Signal rename
             */
            SignalRename(m_Path,newPath,session);
        }

        /// <summary>
        /// Creates new sub folder
        /// </summary>
        /// <param name="folderName"></param>
        public void CreateNewSubFolder(string folderName)
        {
            m_SubFolders.Add(new DomainFolderView(this, m_Path + @"\" + folderName, folderName));
        }

        /// <summary>
        /// Creates anew file
        /// </summary>
        /// <param name="parentFolder"></param>
        /// <param name="fileName"></param>
        /// <param name="definitionAbsolutePath"></param>
        /// <param name="assetAbsolutePath"></param>
        /// <param name="definition"></param>
        public void CreateNewFile(DomainFolderView parentFolder,string fileName,string definitionAbsolutePath,string assetAbsolutePath,in AssetDefinition definition)
        {
            m_Files.Add(new DomainFileView(definition,parentFolder, DomainFileState.Valid, definitionAbsolutePath, assetAbsolutePath));
        }

        public void DeleteFile(in Guid id,EditorSession session)
        {
            /*
             * Try find file
             */
            for(int fileIndex = 0;fileIndex <m_Files.Count;fileIndex++)
            {
                /*
                 * Get file view
                 */
                DomainFileView fileView = m_Files[fileIndex];

                /*
                 * Validate match
                 */
                if(fileView.AssetID == id) // found a match
                {
                    /*
                     * Remove from
                     */
                    m_Files.RemoveAt(fileIndex);
                    session.DeleteAsset(id);
                    return;
                }
                
            }
        }

        /// <summary>
        /// Renames the folder files recursively.(Sub folder and files are included)
        /// </summary>
        /// <param name="oldRoot"></param>
        /// <param name="newRoot"></param>
        /// <param name="session"></param>
        private void SignalRename(string oldRoot,string newRoot,EditorSession session)
        {
            /*
             * Replace old root with the new root
             */
            string path = m_Path.Replace(oldRoot, newRoot);

            /*
             * Set self path
             */
            m_Path = path;

            /*
             * Broadcast renaming to sub folders
             */
            for(int subFolderIndex = 0;subFolderIndex < m_SubFolders.Count;subFolderIndex++)
            {
                m_SubFolders[subFolderIndex].SignalRename(oldRoot, newRoot,session);
            }

            /*
             * Broadcast renaming to files
             */
            for(int fileIndex =0;fileIndex < m_Files.Count;fileIndex++)
            {
                m_Files[fileIndex].RenamePaths(oldRoot, newRoot,session);
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
