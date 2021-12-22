using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Extensions
{
    public static class Vector3Extensions
    {
        public static System.Numerics.Vector3 GetAsNumerics(this OpenTK.Mathematics.Vector3 vec)
        {
            return new System.Numerics.Vector3(vec.X, vec.Y,vec.Z);
        }
        public static OpenTK.Mathematics.Vector3 GetAsOpenTK(this System.Numerics.Vector3 vec)
        {
            return new OpenTK.Mathematics.Vector3(vec.X, vec.Y,vec.Z);
        }
        public static OpenTK.Mathematics.Vector3 GetAsOpenTK(this Assimp.Vector3D vec)
        {
            return new OpenTK.Mathematics.Vector3(vec.X, vec.Y, vec.Z);
        }
    }
}
