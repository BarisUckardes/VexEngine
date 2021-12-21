using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Vex.Framework
{
    /// <summary>
    /// A reference structure which acts as an interface to a graphics resolver parameter
    /// </summary>
    public sealed class GraphicsResolverParameter
    {
        public GraphicsResolverParameter(object targetObject,string parameterName,string parameterCategory)
        {
            /*
             * Set target object
             */
            m_TargetObject = targetObject;

            /*
             * Get field information
             */
            m_TargetFieldInformation = targetObject.GetType().GetField(parameterName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            /*
             * Set category and name
             */
            m_ParameterCategory = parameterCategory;
            m_ParameterName = parameterName;
        }

        /// <summary>
        /// Returns the parameter category
        /// </summary>
        public string ParameterCategory
        {
            get
            {
                return m_ParameterCategory;
            }
        }

        /// <summary>
        /// Returns the parameter name
        /// </summary>
        public string ParameterName
        {
            get
            {
                return m_ParameterName;
            }
        }

        /// <summary>
        /// Sets the graphics parameter value
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        public void SetParameter<TData>(TData data)
        {
            /*
             * Set target field data
             */
            m_TargetFieldInformation.SetValue(m_TargetObject, data);
        }

        /// <summary>
        /// Returns the graphics parameter value
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public TData GetParameter<TData>() 
        {
            return (TData)m_TargetFieldInformation.GetValue(m_TargetObject);
        }

        /// <summary>
        /// Frees the references which this parameter holds
        /// </summary>
        public void Free()
        {
            m_TargetObject = null;
            m_TargetFieldInformation = null;
            m_ParameterName = String.Empty;
            m_ParameterCategory = String.Empty;
        }
        private FieldInfo m_TargetFieldInformation;
        private object m_TargetObject;
        private string m_ParameterCategory;
        private string m_ParameterName;
    }
}
