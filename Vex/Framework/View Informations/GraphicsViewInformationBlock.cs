using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    public sealed class GraphicsViewInformationBlock
    {
        public GraphicsViewInformationBlock()
        {
            m_ResolverInformationBlocks = new List<GraphicsResolverInformationBlock>();
        }

        public List<GraphicsResolverInformationBlock> ResolverInformationBlocks
        {
            get
            {
                return m_ResolverInformationBlocks; 
            }
        }
        public void RegisterResolver(string resolverName,List<GraphicsResolverParameterGroup> resolverParameterGroups)
        {
            m_ResolverInformationBlocks.Add(new GraphicsResolverInformationBlock(resolverName, resolverParameterGroups));
        }

        public void RemoveResolver(string resolverName)
        {
            for(int resolverIndex = 0;resolverIndex < m_ResolverInformationBlocks.Count;resolverIndex++)
            {
                /*
                 * Get resolver
                 */
                GraphicsResolverInformationBlock informationBlock = m_ResolverInformationBlocks[resolverIndex];

                /*
                 * Validate name remove
                 */
                if(informationBlock.ResolverName == resolverName)
                {
                    m_ResolverInformationBlocks.RemoveAt(resolverIndex);
                    return;
                }
            }


        }
        private List<GraphicsResolverInformationBlock> m_ResolverInformationBlocks;
    }
}
