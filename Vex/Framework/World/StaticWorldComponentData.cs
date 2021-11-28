using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Vex.Types;

namespace Vex.Framework
{
    internal readonly struct StaticWorldComponentData
    {
        public StaticWorldComponentData(Type targetComponentType,List<Tuple<string,string>> fieldMetaDatas)
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
             * Create field-data pairs
             */
            foreach(Tuple<string, string> fieldMetaData in fieldMetaDatas)
            {
                /*
                 * Get tuple value
                 */
                string[] fieldTypeNameValue = fieldMetaData.Item1.Split(" ");

                /*
                 * Get field type and name
                 */
                string fieldTypeString = fieldTypeNameValue[0];
                string fieldName = fieldTypeNameValue[1];
                string fieldValue = fieldTypeNameValue[2];
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

        private readonly List<StaticComponentField> m_ComponentFields;
        private readonly Type m_ComponentType;
    }
}
