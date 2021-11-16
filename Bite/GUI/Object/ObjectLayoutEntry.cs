using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    public readonly struct ObjectLayoutEntry
    {
        public ObjectLayoutEntry(Type targetObjectType, Type objectLayoutType)
        {
            TargetObjectType = targetObjectType;
            ObjectLayoutType = objectLayoutType;
        }

        public readonly Type TargetObjectType;
        public readonly Type ObjectLayoutType;
    }
}
