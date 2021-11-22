using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Extensions
{
    public static class Vecto4Extensions
    {
        public static System.Numerics.Vector4 GetAsNumerics(this OpenTK.Mathematics.Vector4 vec)
        {
            return new System.Numerics.Vector4(vec.X, vec.Y, vec.Z,vec.W);
        }
        public static OpenTK.Mathematics.Vector4 GetAsOpenTK(this System.Numerics.Vector4 vec)
        {
            return new OpenTK.Mathematics.Vector4(vec.X, vec.Y, vec.Z, vec.W);
        }
    }
}
