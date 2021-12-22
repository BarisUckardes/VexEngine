using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Extensions
{
    public static class Vector2Extensions
    {
        public static System.Numerics.Vector2 GetAsNumerics(this OpenTK.Mathematics.Vector2 vec)
        {
            return new System.Numerics.Vector2(vec.X, vec.Y);
        }
        public static OpenTK.Mathematics.Vector2 GetAsOpenTK(this System.Numerics.Vector2 vec)
        {
            return new OpenTK.Mathematics.Vector2(vec.X, vec.Y);
        }
        public static OpenTK.Mathematics.Vector2 GetAsOpenTK(this Assimp.Vector2D vec)
        {
            return new OpenTK.Mathematics.Vector2(vec.X, vec.Y);
        }
    }
}
