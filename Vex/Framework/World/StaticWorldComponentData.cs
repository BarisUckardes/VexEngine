using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Vex.Types;

namespace Vex.Framework
{
    /// <summary>
    /// Data volume necessary for filling a component in
    /// </summary>
    public struct StaticWorldComponentData
    {
        public StaticWorldComponentData(int localComponentIndex,Type targetComponentType,List<Tuple<string,string,string>> fieldMetaDatas)
        {
            /*
             * Initialize
             */
            m_ComponentFields = new List<StaticComponentField>();

            /*
             * Set target component type
             */
            m_ComponentType = targetComponentType;

            /*
             * Set component index
             */
            m_LocalComponentIndex = localComponentIndex;

            /*
             * Create field-data pairs
             */
            foreach (Tuple<string, string,string> fieldMetaData in fieldMetaDatas)
            {

                /*
                 * Get field type and name
                 */
                string fieldTypeString = fieldMetaData.Item1;
                string fieldName = fieldMetaData.Item2;
                string fieldValue = fieldMetaData.Item3;
                StaticComponentFieldType fieldType =(StaticComponentFieldType)Enum.Parse(typeof(StaticComponentFieldType), fieldTypeString);

                /*
                 * Try get field info
                 */
                FieldInfo fieldInfo = TypeUtils.GetField(targetComponentType,fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                /*
                 * Validate found
                 */
                if(fieldInfo == null) // this component seems changed.Cant find the expected field
                {
                    m_ComponentFields.Add(new StaticComponentField(fieldInfo,StaticComponentFieldType.Invalid,fieldName, fieldValue));
                }
                else
                {
                    /*
                     * 
                     */
                    m_ComponentFields.Add(new StaticComponentField(fieldInfo, fieldType, fieldName, fieldValue));
                }
            }
        }

        /// <summary>
        /// Returns all the component fields
        /// </summary>
        public List<StaticComponentField> Fields
        {
            get
            {
                return m_ComponentFields;
            }
        }

        /// <summary>
        /// Returns the component type of this volume
        /// </summary>
        public Type ComponentType
        {
            get
            {
                return m_ComponentType;
            }
        }

        /// <summary>
        /// Returns the target local compoentn index
        /// </summary>
        public int LocalComponentIndex
        {
            get
            {
                return m_LocalComponentIndex;
            }
        }

        private List<StaticComponentField> m_ComponentFields;
        private Type m_ComponentType;
        private int m_LocalComponentIndex;
    }
}
