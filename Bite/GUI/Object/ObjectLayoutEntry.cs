using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{

    /// <summary>
    /// Data volume for object gui layout
    /// </summary>
    public readonly struct ObjectLayoutEntry
    {
        public ObjectLayoutEntry(Type targetObjectType, Type objectLayoutType)
        {
            TargetObjectType = targetObjectType;
            ObjectLayoutType = objectLayoutType;
        }

        /// <summary>
        /// The target object type
        /// </summary>
        public readonly Type TargetObjectType;

        /// <summary>
        /// The gui object layout type
        /// </summary>
        public readonly Type ObjectLayoutType;
    }
}
