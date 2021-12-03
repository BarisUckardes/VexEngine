using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{

    /// <summary>
    /// Attribute for specifying the target object which a object gui layout targets
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ObjectLayoutAttribute : Attribute
    {
        public ObjectLayoutAttribute(Type targetObjectType)
        {
            TargetObjectType = targetObjectType;
        }

        /// <summary>
        /// The target object type which this object gui layout targets
        /// </summary>
        public Type TargetObjectType { get; set; }
    }
}
