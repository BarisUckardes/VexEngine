using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Utility class for native depth function conversions
    /// </summary>
    public static class DepthFunctionUtils
    {
        /// <summary>
        /// Converts the depth function enum to native depth function
        /// </summary>
        /// <param name="depthFunction"></param>
        /// <returns></returns>
        public static OpenTK.Graphics.OpenGL4.DepthFunction GetNative(DepthFunction depthFunction)
        {
            switch (depthFunction)
            {
                case DepthFunction.Never:
                    return OpenTK.Graphics.OpenGL4.DepthFunction.Never;
                    break;
                case DepthFunction.Less:
                    return OpenTK.Graphics.OpenGL4.DepthFunction.Less;
                    break;
                case DepthFunction.Equal:
                    return OpenTK.Graphics.OpenGL4.DepthFunction.Equal;
                    break;
                case DepthFunction.Lequal:
                    return OpenTK.Graphics.OpenGL4.DepthFunction.Lequal;
                    break;
                case DepthFunction.Greater:
                    return OpenTK.Graphics.OpenGL4.DepthFunction.Greater;
                    break;
                case DepthFunction.Notequal:
                    return OpenTK.Graphics.OpenGL4.DepthFunction.Notequal;
                    break;
                case DepthFunction.Gequal:
                    return OpenTK.Graphics.OpenGL4.DepthFunction.Gequal;
                    break;
                case DepthFunction.Always:
                    return OpenTK.Graphics.OpenGL4.DepthFunction.Always;
                    break;
                default:
                    return OpenTK.Graphics.OpenGL4.DepthFunction.Never;
                    break;
            }
        }
    }
}
