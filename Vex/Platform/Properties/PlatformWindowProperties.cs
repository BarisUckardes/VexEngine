using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    public static class PlatformWindowProperties
    {

        public static Vector2 Size
        {
            get
            {
                return s_Size;
            }
            internal set
            {
                s_Size = value;
            }
        }
        private static Vector2 s_Size;
    }
}
