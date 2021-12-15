using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Asset;
using Vex.Framework;

namespace Bite.Core
{
    /// <summary>
    /// Represents a file view from the physical domain
    /// </summary>
    public sealed class DomainFileView
    {
        public DomainFileView(in AssetDefinition definition,DomainFolderView parentFolder,DomainFileState fileState,string definitionAbsolutePath,string assetAbsolutePath)
        {
            m_ParentFolder = parentFolder;
            m_DefinitonAbsolutePath = definitionAbsolutePath;
            m_AssetAbsolutePath = assetAbsolutePath;
            m_FileState = fileState;
            m_AssetName = definition.Name;
            m_AssetID = definition.ID;
            m_AssetType = definition.Type;
        }

        /// <summary>
        /// Returns the target object which this file view targets
        /// </summary>
        public VexObject TargetAssetObject
        {
            get
            {
                return m_LoadedObject;
            }
        }

        /// <summary>
        /// Returns the asset id of this file view
        /// </summary>
        public Guid AssetID
        {
            get
            {
                return m_AssetID;
            }
        }

        /// <summary>
        /// Returns the absolute path of the defintion this view targets
        /// </summary>
        public string DefinitionAbsolutePath
        {
            get
            {
                return m_DefinitonAbsolutePath;
            }
        }

        /// <summary>
        /// Returns the absolute path of the asset this file view targets
        /// </summary>
        public string AssetAbsolutePath
        {
            get
            {
                return m_AssetAbsolutePath;
            }
        }


        /// <summary>
        /// Returns the asset name of an asset which this file view targets
        /// </summary>
        public string AssetName
        {
            get
            {
                return m_AssetName;
            }
        }

        /// <summary>
        /// Returns the asset type of an asset which this file view targets
        /// </summary>
        public AssetType AssetType
        {
            get
            {
                return m_AssetType;
            }
        }

        /// <summary>
        /// Returns the file state of this file
        /// </summary>
        public DomainFileState FileState
        {
            get
            {
                return m_FileState;
            }
        }

        /// <summary>
        /// Tries to laod this file view's target asset
        /// </summary>
        /// <param name="session"></param>
        public void TryLoad(EditorSession session)
        {
            if(!m_Loaded)
            {
                /*
                 * Get or loade object
                 */
                m_LoadedObject = session.GetOrLoadAsset(m_AssetID);

                /*
                 * Set load state
                 */
                if (m_LoadedObject != null)
                    m_Loaded = true;
                else
                    m_Loaded = false;
            }
        }

        /// <summary>
        /// Renames this file view along with the physical target
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="session"></param>
        public void Rename(string newName,EditorSession session)
        {
            /*
             * Rename self
             */
            string oldDefinitionPath = m_DefinitonAbsolutePath;
            string oldAssetPath = m_AssetAbsolutePath;
            m_DefinitonAbsolutePath = m_ParentFolder.FolderPath + @"\" + newName + @".vdefinition";
            m_AssetAbsolutePath = m_ParentFolder.FolderPath + @"\" + newName + @".vasset";
            m_AssetName = newName;

            /*
             * Rename physical file
             */
            File.Move(oldDefinitionPath, m_DefinitonAbsolutePath);
            File.Move(oldAssetPath, m_AssetAbsolutePath);

            session.RenameAsset(m_AssetID,newName);
        }

        public void Delete(EditorSession session)
        {
            /*
             * Try delete
             */
            m_ParentFolder.DeleteFile(AssetID,session);
        }

        /// <summary>
        /// An ınternal function
        /// </summary>
        /// <param name="oldRoot"></param>
        /// <param name="newRoot"></param>
        /// <param name="session"></param>
        internal void RenamePaths(string oldRoot,string newRoot,EditorSession session)
        {
            /*
             * Rename self paths
             */
            m_AssetAbsolutePath = m_AssetAbsolutePath.Replace(oldRoot, newRoot);
            m_DefinitonAbsolutePath = m_DefinitonAbsolutePath.Replace(oldRoot, newRoot);

            /*
             * Rename assetpool
             */
            session.RenameAssetPaths(m_AssetID,oldRoot,newRoot);
        }

        
        private VexObject m_LoadedObject;
        private DomainFolderView m_ParentFolder;
        private DomainFileState m_FileState;
        private string m_DefinitonAbsolutePath;
        private string m_AssetAbsolutePath;
        private string m_AssetName;
        private Guid m_AssetID;
        private AssetType m_AssetType;
        private bool m_Loaded;
    }
}
