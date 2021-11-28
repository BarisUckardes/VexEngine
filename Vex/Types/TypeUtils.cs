using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
