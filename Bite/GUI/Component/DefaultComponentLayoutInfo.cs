using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Bite.GUI
{
    /// <summary>
    /// Data volume for defualt component layout
    /// </summary>
    internal readonly struct DefaultComponentLayoutInfo
    {

        public DefaultComponentLayoutInfo(FieldInfo[] fieldInfo,PropertyInfo[] propertyInfo,Type targetComponentType)
        {
            FieldInfo = fieldInfo;
            PropertyInfo = propertyInfo;
            TargetComponentType = targetComponentType;
        }

        /// <summary>
        /// Target component type which has no custom componen layout
        /// </summary>
        public readonly Type TargetComponentType;

        /// <summary>
        /// All the fields which are exposable
        /// </summary>
        public readonly FieldInfo[] FieldInfo;

        /// <summary>
        /// All the properties which are exosable
        /// </summary>
        public readonly PropertyInfo[] PropertyInfo;

       
    }
}
