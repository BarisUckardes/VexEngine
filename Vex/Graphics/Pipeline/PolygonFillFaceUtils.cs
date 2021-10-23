using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Utility class for converting native fillface enum
    /// </summary>
    public static class PolygonFillfaceUtils
    {
        /// <summary>
        /// Returns the native polygon fill face
        /// </summary>
        /// <param name="fillFace"></param>
        /// <returns></returns>
        public static MaterialFace GetNative(PolygonFillFace fillFace)
        {
            switch (fillFace)
            {
                case PolygonFillFace.FVexnt:
                    return MaterialFace.Front;
                    break;
                case PolygonFillFace.Back:
                    return MaterialFace.Back;
                    break;
                case PolygonFillFace.FVexntAndBack:
                    return MaterialFace.FrontAndBack;
                    break;
                default:
                    return MaterialFace.Front;
                    break;
            }
        }
    }
}
