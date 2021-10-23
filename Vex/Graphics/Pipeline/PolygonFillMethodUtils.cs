using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Utility class for converting polygon fill method to native method
    /// </summary>
    public static class PolygonFillMethodUtils
    {
        /// <summary>
        /// Returns the native method enum
        /// </summary>
        /// <param name="fillMethod"></param>
        /// <returns></returns>
        public static OpenTK.Graphics.OpenGL4.PolygonMode GetNative(PolygonFillMethod fillMethod)
        {
            switch (fillMethod)
            {
                case PolygonFillMethod.Point:
                    return OpenTK.Graphics.OpenGL4.PolygonMode.Point;
                    break;
                case PolygonFillMethod.Line:
                    return OpenTK.Graphics.OpenGL4.PolygonMode.Line;
                    break;
                case PolygonFillMethod.Fill:
                    return OpenTK.Graphics.OpenGL4.PolygonMode.Fill;
                    break;
                default:
                    return OpenTK.Graphics.OpenGL4.PolygonMode.Fill;
                    break;
            }
        }
    }
}
