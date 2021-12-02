using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    /// <summary>
    /// And attribute which binds a type to component gui layout
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ComponentLayoutAttribute : Attribute
    {
        public ComponentLayoutAttribute(Type targetType)
        {
            TargetType = targetType;
        }

        /// <summary>
        /// The type which gui component layout targets
        /// </summary>
        public Type TargetType { get; set; }
    }
}
