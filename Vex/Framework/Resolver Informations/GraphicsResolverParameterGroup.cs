using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    public sealed class GraphicsResolverParameterGroup
    {
        public GraphicsResolverParameterGroup(object targetObject,string categoryName,List<string> parameterNames)
        {
            /*
             * Set target object
             */
            m_TargetObject = targetObject;

            /*
             * Set category
             */
            m_CategoryName = categoryName;
            /*
             * Set parameters
             */
            m_Parameters = new List<GraphicsResolverParameter>(parameterNames.Count);
            foreach(string parameterName in parameterNames)
            {
                m_Parameters.Add(new GraphicsResolverParameter(targetObject, parameterName, categoryName));
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
        private object m_TargetObject;
        private string m_CategoryName;
        
    }
}
