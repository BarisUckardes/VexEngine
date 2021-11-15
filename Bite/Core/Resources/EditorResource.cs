using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Asset;
using Vex.Framework;
namespace Bite.Core
{
    public sealed class EditorResource
    {
        public EditorResource(VexObject resource,string name,AssetType type)
        {
            /*
             * Set resource name
             */
            resource.Name = name;

            /*
             * Initialize
             */
            m_Type = type;
            m_Resource = resource;
            m_Name = name;
        }

        public VexObject Resource
        {
            get
            {
                return m_Resource;
            }
        }
        public string Name
        {
            get
            {
                return m_Name;
            }
        }
        public AssetType Type
        {
            get
            {
                return m_Type;
            }
        }
       
        private VexObject m_Resource;
        private string m_Name;
        private AssetType m_Type;
    }
}
