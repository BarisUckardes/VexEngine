using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Contains a graphics resolver's parameter groups
    /// </summary>
    public sealed class GraphicsResolverParameterGroup
    {
        public GraphicsResolverParameterGroup(object targetObject,string categoryName,List<Tuple<string,string>> parameterNames)
        {
            /*
             * Set category
             */
            m_CategoryName = categoryName;
            /*
             * Set parameters
             */
            m_Parameters = new List<GraphicsResolverParameter>(parameterNames.Count);
            foreach(Tuple<string,string> parameterName in parameterNames)
            {
                m_Parameters.Add(new GraphicsResolverParameter(targetObject, parameterName.Item1, parameterName.Item2, categoryName));
            }

        }

        /// <summary>
        /// Returns the group parameters
        /// </summary>
        public List<GraphicsResolverParameter> Parameters
        {
            get
            {
                return m_Parameters;
            }
        }
        /// <summary>
        /// Returns the category name
        /// </summary>
        public string CategoryName
        {
            get
            {
                return m_CategoryName;
            }
        }

        /// <summary>
        /// Frees this parameter group
        /// </summary>
        public void Free()
        {
            foreach(GraphicsResolverParameter parameter in m_Parameters)
            {
                parameter.Free();
            }
            m_Parameters.Clear();
        }

        private List<GraphicsResolverParameter> m_Parameters;
        private string m_CategoryName;
        
    }
}
