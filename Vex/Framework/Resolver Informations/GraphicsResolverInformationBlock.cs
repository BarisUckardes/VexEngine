using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    public sealed class GraphicsResolverInformationBlock
    {
        public GraphicsResolverInformationBlock(string resolverName,List<GraphicsResolverParameterGroup> parameterGroups)
        {
            /*
             * Set parameter groups
             */
            m_Groups = parameterGroups;

            /*
             * Sets resolver name
             */
            m_ResolverName = resolverName;
        }

        /// <summary>
        /// Returns all the parameter groups this resolver information block has
        /// </summary>
        public List<GraphicsResolverParameterGroup> ParameterGroups
        {
            get
            {
                return m_Groups;
            }
        }

        /// <summary>
        /// Returns the resolver name
        /// </summary>
        public string ResolverName
        {
            get
            {
                return m_ResolverName;
            }
        }

        public void Free()
        {
            foreach (GraphicsResolverParameterGroup group in m_Groups)
                group.Free();
        }

        private List<GraphicsResolverParameterGroup> m_Groups;
        private string m_ResolverName;
    }
}
