using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    public sealed class RenderPass
    {
        public RenderPass()
        {
            m_ResolverMaterialPairs = new List<RenderPassResolverMaterialPair>();
            m_PassName = "Default pass name";
        }

        /// <summary>
        /// Returns the list of a resolver material pairs
        /// </summary>
        public List<RenderPassResolverMaterialPair> ResolverMaterialPairs
        {
            get
            {
                return new List<RenderPassResolverMaterialPair>(m_ResolverMaterialPairs);
            }
        }

        /// <summary>
        /// The target framebuffer which this pass will render into
        /// </summary>
        public Framebuffer TargetFramebuffer
        {
            get
            {
                return m_TargetFramebuffer;
            }
            set
            {
                m_TargetFramebuffer = value;
            }
        }

        /// <summary>
        /// The name of this pass
        /// </summary>
        public string PassName
        {
            get
            {
                return m_PassName;
            }
            set
            {
                m_PassName = value;
            }
        }

      

        /// <summary>
        /// Register new pass pair
        /// </summary>
        /// <param name="pair"></param>
        public void RegisterPair(in RenderPassResolverMaterialPair pair)
        {
            m_ResolverMaterialPairs.Add(pair);
        }

        private List<RenderPassResolverMaterialPair> m_ResolverMaterialPairs;
        private Framebuffer m_TargetFramebuffer;
        private string m_PassName;
    }
}
