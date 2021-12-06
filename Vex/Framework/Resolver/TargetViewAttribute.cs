using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TargetViewAttribute : Attribute
    {
        public TargetViewAttribute(Type targetType)
        {
            TargetViewType = targetType;
        }
        public Type TargetViewType { get; set; }
    }
}
