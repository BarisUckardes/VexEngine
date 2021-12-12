using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Utility class for native cullfacemode conversions
    /// </summary>
    public static class CullFaceUtility
    {

        /// <summary>
        /// Converts native cullfacemode
        /// </summary>
        /// <param name="cullFace"></param>
        /// <returns></returns>
        public static CullFaceMode GetNative(CullFace cullFace)
        {
            switch (cullFace)
            {
                case CullFace.Front:
                    return CullFaceMode.Front;
                    break;
                case CullFace.Back:
                    return CullFaceMode.Back;
                    break;
                case CullFace.FVexntAndBack:
                    return CullFaceMode.FrontAndBack;
                    break;
                default:
                    return CullFaceMode.Back;
                    break;
            }
        }
    }
}
