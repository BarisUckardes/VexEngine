using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Application;
using Vex.Asset;
using Vex.Framework;
namespace Bite.Core
{
    public sealed class EditorSession
    {
        public EditorSession(ApplicationSession applicationSession)
        {
            m_ApplicationSession = applicationSession;
            m_Resources = new List<EditorResource>();
        }

        public IReadOnlyCollection<World> Worlds
        {
            get
            {
                return m_ApplicationSession.Worlds;
            }
        }

        public DomainView FileDomain
        {
            get
            {
                return m_Domain;
            }
            internal set
            {
                m_Domain = value;
            }
        }
        public VexObject GetEditorResource(string name,AssetType type)
        {
            /*
             * Iterate each resource and validate type
             */
            for(int resourceIndex =0;resourceIndex < m_Resources.Count;resourceIndex++)
            {
                /*
                 * Get resource
                 */
                EditorResource resource = m_Resources[resourceIndex];

                /*
                 * Validate type
                 */
                if(resource.Type == type)
                {
                    /*
                     * Validate name
                     */
                    if (name == resource.Name)
                        return resource.Resource;
                }
            }

            return null;
        }

        internal void SetEditorResources(List<EditorResource> resources)
        {
            m_Resources = resources;
        }
        internal void ShutdownRequest()
        {
            m_ApplicationSession.HasShutdownRequest = true;
        }
        internal void Shutdown()
        {

        }

        private List<EditorResource> m_Resources;
        private ApplicationSession m_ApplicationSession;
        private DomainView m_Domain;
    }
}
