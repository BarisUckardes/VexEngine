using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Asset;
using Vex.Framework;

namespace Bite.Core
{
    public sealed class DomainFileView
    {
        public DomainFileView(in AssetDefinition definition,DomainFileState fileState,string definitionAbsolutePath,string assetAbsolutePath)
        {
            m_DefinitonAbsolutePath = definitionAbsolutePath;
            m_AssetAbsolutePath = assetAbsolutePath;
            m_FileState = fileState;
            m_AssetName = definition.Name;
            m_AssetID = definition.ID;
            m_AssetType = definition.Type;
        }

        public VexObject TargetAssetObject
        {
            get
            {
                return m_LoadedObject;
            }
        }

        public Guid AssetID
        {
            get
            {
                return m_AssetID;
            }
        }
        public string DefinitionAbsolutePath
        {
            get
            {
                return m_DefinitonAbsolutePath;
            }
        }
        public string AssetAbsolutePath
        {
            get
            {
                return m_AssetAbsolutePath;
            }
        }
        public string AssetName
        {
            get
            {
                return m_AssetName;
            }
        }
        public AssetType AssetType
        {
            get
            {
                return m_AssetType;
            }
        }
        public DomainFileState FileState
        {
            get
            {
                return m_FileState;
            }
        }
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

        internal void RenamePaths(string oldRoot,string newRoot)
        {
            m_AssetAbsolutePath = m_AssetAbsolutePath.Replace(oldRoot, newRoot);
            m_DefinitonAbsolutePath = m_DefinitonAbsolutePath.Replace(oldRoot, newRoot);
        }
        private VexObject m_LoadedObject;
        private DomainFileState m_FileState;
        private string m_DefinitonAbsolutePath;
        private string m_AssetAbsolutePath;
        private string m_AssetName;
        private Guid m_AssetID;
        private AssetType m_AssetType;
        private bool m_Loaded;
    }
}
