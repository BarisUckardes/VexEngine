using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    internal readonly struct StaticComponentField
    {
        public StaticComponentField(FieldInfo fieldInfo,StaticComponentFieldType fieldType,string expectedFieldName,string fieldDataString)
        {
            ExpectedFieldName = expectedFieldName;
            FieldType = fieldType;
            TargetFieldInfo = fieldInfo;
            FieldDataString = fieldDataString;
        }

        public bool IsValid
        {
            get
            {
                return TargetFieldInfo != null;
            }
        }
        public readonly FieldInfo TargetFieldInfo;
        public readonly StaticComponentFieldType FieldType;
        public readonly string ExpectedFieldName;
        public readonly string FieldDataString;
    }
}
