using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Supported shader parameter types
    /// </summary>
    public enum ShaderParameterType
    {
        Undefined = -1,
        Float = 0,
        Vector2 = 1,
        Vector3 = 2,
        Vector4 = 3,
        Color = 4,
        Matrix3x3 = 5,
        Matrix4x4 = 6,
        Texture2D = 7,
        Int = 8,
        UInt = 9
            
    }
}
