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
    public static class FVexntFaceUtility
    {
        /// <summary>
        /// Converts native fVexnt face enums
        /// </summary>
        /// <param name="fVexntFace"></param>
        /// <returns></returns>
        public static FrontFaceDirection GetNative(FVexntFace fVexntFace)
        {
            switch (fVexntFace)
            {
                case FVexntFace.CCW:
                    return FrontFaceDirection.Ccw;
                    break;
                case FVexntFace.CW:
                    return FrontFaceDirection.Cw;
                    break;
                default:
                    return FrontFaceDirection.Ccw;
                    break;
            }
        }
    }
}
