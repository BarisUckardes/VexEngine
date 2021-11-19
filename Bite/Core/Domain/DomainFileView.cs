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
            m_Definition = definition;
            m_DefinitonAbsolutePath = definitionAbsolutePath;
            m_AssetAbsolutePath = assetAbsolutePath;
            m_FileState = fileState;
        }

        public VexObject TargetAssetObject
        {
            get
            {
                return m_LoadedObject;
            }
        }
        public AssetDefinition Definition
        {
            get
            {
                return m_Definition;
            }
        }
        public DomainFileState FileState
        {
            get
            {
                return m_FileState;
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
        public void TryLoad(EditorSession session)
        {
            if(!m_Loaded)
            {
                /*
                 * Get or loade object
                 */
                m_LoadedObject = session.GetOrLoadAsset(m_Definition.ID);

                /*
                 * Set load state
                 */
                if (m_LoadedObject != null)
                    m_Loaded = true;
                else
                    m_Loaded = false;
            }
        }

        private VexObject m_LoadedObject;
        private AssetDefinition m_Definition;
        private DomainFileState m_FileState;
        private string m_DefinitonAbsolutePath;
        private string m_AssetAbsolutePath;
        private bool m_Loaded;
    }
}
