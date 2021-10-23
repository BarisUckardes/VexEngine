using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Bite.GUI
{
    public readonly struct DefaultComponentLayoutInfo
    {

        public DefaultComponentLayoutInfo(FieldInfo[] fieldInfo,PropertyInfo[] propertyInfo,Type targetComponentType)
        {
            FieldInfo = fieldInfo;
            PropertyInfo = propertyInfo;
            TargetComponentType = targetComponentType;
        }

        public readonly Type TargetComponentType;
        public readonly FieldInfo[] FieldInfo;
        public readonly PropertyInfo[] PropertyInfo;

       
    }
}
