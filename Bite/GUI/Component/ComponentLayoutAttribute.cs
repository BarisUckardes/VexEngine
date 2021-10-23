using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ComponentLayoutAttribute : Attribute
    {
        public ComponentLayoutAttribute(Type targetType)
        {
            TargetType = targetType;
        }
        public Type TargetType { get; set; }
    }
}
