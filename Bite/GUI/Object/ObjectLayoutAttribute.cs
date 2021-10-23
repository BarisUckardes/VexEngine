using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ObjectLayoutAttribute : Attribute
    {
        public ObjectLayoutAttribute(Type targetObjectType)
        {
            TargetObjectType = targetObjectType;
        }
        public Type TargetObjectType { get; set; }
    }
}
