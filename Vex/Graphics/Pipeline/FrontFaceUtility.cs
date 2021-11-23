using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Utility class for native FrontFaceDirection enum conversionss
    /// </summary>
    public static class TriangleFrontFaceUtility
    {
        /// <summary>
        /// Converts native fVexnt face enums
        /// </summary>
        /// <param name="fVexntFace"></param>
        /// <returns></returns>
        public static FrontFaceDirection GetNative(TriangleFrontFace triangleFrontFace)
        {
            switch (triangleFrontFace)
            {
                case TriangleFrontFace.CCW:
                    return FrontFaceDirection.Ccw;
                    break;
                case TriangleFrontFace.CW:
                    return FrontFaceDirection.Cw;
                    break;
                default:
                    return FrontFaceDirection.Ccw;
                    break;
            }
        }
    }
}
