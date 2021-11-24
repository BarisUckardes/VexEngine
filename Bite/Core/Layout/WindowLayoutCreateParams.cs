using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    internal readonly struct WindowLayoutCreateParams
    {
        public WindowLayoutCreateParams(Type layoutType, Guid iD)
        {
            LayoutType = layoutType;
            ID = iD;
        }
        public readonly Type LayoutType;
        public readonly Guid ID;

       
    }
}
