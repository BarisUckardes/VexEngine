using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Data volume for component field
    /// </summary>
    public readonly struct StaticComponentField
    {
        public StaticComponentField(FieldInfo fieldInfo,StaticComponentFieldType fieldType,string expectedFieldName,string fieldDataString)
        {
            ExpectedFieldName = expectedFieldName;
            FieldType = fieldType;
            TargetFieldInfo = fieldInfo;
            FieldDataString = fieldDataString;
        }

        /// <summary>
        /// Is this field is a valid field
        /// </summary>
        public bool IsValid
        {
            get
            {
                return TargetFieldInfo != null;
            }
        }

        /// <summary>
        /// Target type field info
        /// </summary>
        public readonly FieldInfo TargetFieldInfo;

        /// <summary>
        /// Type of the field
        /// </summary>
        public readonly StaticComponentFieldType FieldType;

        /// <summary>
        /// Expected field name
        /// </summary>
        public readonly string ExpectedFieldName;

        /// <summary>
        /// Field data in string
        /// </summary>
        public readonly string FieldDataString;
    }
}
