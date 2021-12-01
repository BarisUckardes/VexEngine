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

        /// <summary>
        /// Returns all the fields in this type including all the base classes in the sub hierarchy
        /// </summary>
        /// <param name="type"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a field info in this type of the base types in the sub hierarchy
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="fieldName"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static FieldInfo GetField(Type targetType,string fieldName,BindingFlags flags)
        {
            /*
             * Initialize insepcting type
             */
            Type inspectingType = targetType;

            /*
             * Validate inspecting type
             */
            if (inspectingType == null)
                return null;

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

        /// <summary>
        /// Sets the default value for a field info
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="fieldInfo"></param>
        /// <param name="data"></param>
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

            try
            {
                /*
            * Catch field type
            */
                if (fieldType == typeof(Vector4)) // set as vector4
                {
                    /*
                     * Split string into 4 pieces.(X,Y,Z,W)
                     */
                    string[] splitValue = data.Replace("<", "").Replace(">", "").Replace(" ", "").Split(",");

                    /*
                     * Parse and initialize a vector4
                     */
                    Vector4 value = new Vector4(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));

                    /*
                     * Set field value as vector4
                     */
                    fieldInfo.SetValue(targetObject, value);
                }
                else if (fieldType == typeof(Vector3)) // set as vector3
                {
                    /*
                     * Split string into 3 pieces.(X,Y,Z)
                     */
                    string[] splitValue = data.Replace("<", "").Replace(">", "").Replace(" ", "").Split(",");

                    /*
                     * Parse and intialize a vector3
                     */
                    Vector3 value = new Vector3(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]));

                    /*
                     * Set field value as vector3
                     */
                    fieldInfo.SetValue(targetObject, value);
                }
                else if (fieldType == typeof(Vector2)) // set as vector2
                {
                    /*
                     * Split string into 2 pieces.(X,Y)
                     */
                    string[] splitValue = data.Replace("<", "").Replace(">", "").Replace(" ", "").Split(",");

                    /*
                     * Parse and intialize a vector3
                     */
                    Vector2 value = new Vector2(float.Parse(splitValue[0]), float.Parse(splitValue[1]));

                    /*
                     * Set field value as vector2
                     */
                    fieldInfo.SetValue(targetObject, value);
                }
                else if (fieldType == typeof(float)) // set as float
                {
                    /*
                     * Set field value as float
                     */
                    fieldInfo.SetValue(targetObject, float.Parse(data));
                }
                else if (fieldType == typeof(int)) // set as int
                {
                    /*
                     * Set field value as int
                     */
                    fieldInfo.SetValue(targetObject, int.Parse(data));
                }
                else // set as string
                {
                    /*
                     * Set field value as string
                     */
                    fieldInfo.SetValue(targetObject, data);
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine("Field read error: " + exception.Message);
            }
        }
    }
}
