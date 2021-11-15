using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Bite.GUI
{
    internal readonly struct ComponentLayoutEntry
    {
        public ComponentLayoutEntry(Type targetComponentType, Type componentLayoutType)
        {
            TargetComponentType = targetComponentType;
            ComponentLayoutType = componentLayoutType;

           
        }

        public readonly Type TargetComponentType;
        public readonly Type ComponentLayoutType;
       
    }
}
