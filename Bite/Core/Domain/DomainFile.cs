using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Asset;

namespace Bite.Core
{
    public sealed class DomainFile
    {
        public DomainFile(in AssetDefinition definition,DomainFileState fileState,string definitionAbsolutePath,string assetAbsolutePath)
        {
            m_Definition = definition;
            m_DefinitonAbsolutePath = definitionAbsolutePath;
            m_AssetAbsolutePath = assetAbsolutePath;
            m_FileState = fileState;
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
        private AssetDefinition m_Definition;
        private DomainFileState m_FileState;
        private string m_DefinitonAbsolutePath;
        private string m_AssetAbsolutePath;
    }
}
