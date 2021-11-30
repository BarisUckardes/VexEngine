using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;

namespace Vex.Types
{
    /// <summary>
    /// Misc utils for type operations
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        /// Returns the size of the specified type
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static unsafe int GetTypeSize<TType>() where TType : unmanaged
        {
            return sizeof(TType);
        }
        public static List<FieldInfo> GetAllFields(Type type,BindingFlags flags)
        {
            /*
             * Initialize
             */
            List<FieldInfo> fields = new List<FieldInfo>();
            Type inspectingType = type;
           
            /*
             * Iterate each base type until reaching vexobject
             */
            while (inspectingType != typeof(VexObject))
            {
                /*
                 * Add the fields requested
                 */
                fields.AddRange(inspectingType.GetFields(flags));

                /*
                 * Set the new inspecting type as the base type 
                 */
                inspectingType = inspectingType.BaseType;
            }

            return fields;
        }
        public static FieldInfo GetField(Type targetType,string fieldName,BindingFlags flags)
        {
            /*
             * Initialize insepcting type
             */
            Type inspectingType = targetType;

            /*
             * Iterate each base type until reaching vexobject
             */
            while (inspectingType != typeof(VexObject))
            {
                /*
                 * Add the fields requested
                 */
                FieldInfo[] fields = inspectingType.GetFields(flags);
               
                /*
                 * Iterate all fields and validate match
                 */
                foreach(FieldInfo fieldInfo in fields)
                {
                    if (fieldInfo.Name == fieldName)
                        return fieldInfo;
                }
               
                /*
                 * Set the new inspecting type as the base type 
                 */
                inspectingType = inspectingType.BaseType;
            }

            return null;

        }
        public static void SetDefaultFieldValue(object targetObject,FieldInfo fieldInfo,string data)
        {
            /*
             * Validate field info
             */
            if (fieldInfo == null)
                return;

            /*
             * Get field type
             */
            Type fieldType = fieldInfo.FieldType;

            /*
             * Catch field type
             */
            if (fieldType == typeof(Vector4))
            {
                string[] splitValue = data.Replace("<", "").Replace(">", "").Replace(" ", "").Split(",");
                Vector4 value = new Vector4(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
                fieldInfo.SetValue(targetObject, value);
            }
            else if (fieldType == typeof(Vector3))
            {
                string[] splitValue = data.Replace("<","").Replace(">","").Replace(" ","").Split(",");
                Vector3 value = new Vector3(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]));
                fieldInfo.SetValue(targetObject, value);
            }
            else if (fieldType == typeof(Vector2))
            {
                string[] splitValue = data.Replace("<", "").Replace(">", "").Replace(" ", "").Split(",");
                Vector2 value = new Vector2(float.Parse(splitValue[0]), float.Parse(splitValue[1]));
                fieldInfo.SetValue(targetObject, value);
            }
            else if(fieldType == typeof(float))
            {
                fieldInfo.SetValue(targetObject, float.Parse(data));
            }
            else if (fieldType == typeof(int))
            {
                fieldInfo.SetValue(targetObject, int.Parse(data));
            }
            else
            {
                fieldInfo.SetValue(targetObject, data);
            }
        }
    }
}
