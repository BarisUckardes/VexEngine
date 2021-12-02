using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Bite.GUI
{
    /// <summary>
    /// An entry struct holding pair of component gui layout and its target component type
    /// </summary>
    internal readonly struct ComponentLayoutEntry
    {
        public ComponentLayoutEntry(Type targetComponentType, Type componentLayoutType)
        {
            TargetComponentType = targetComponentType;
            ComponentLayoutType = componentLayoutType;
           
        }

        /// <summary>
        /// Target componenet type which component gui layout expects
        /// </summary>
        public readonly Type TargetComponentType;

        /// <summary>
        /// The type of the component gui layout
        /// </summary>
        public readonly Type ComponentLayoutType;
       
    }
}
