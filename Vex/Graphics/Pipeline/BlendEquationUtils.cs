using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Utility class for native blend equation enum conversions
    /// </summary>
    public static class BlendEquationUtils
    {

        /// <summary>
        /// Returns the native BlendEquationMode
        /// </summary>
        /// <param name="blendEq"></param>
        /// <returns></returns>
        public static BlendEquationMode GetNative(BlendEquation blendEq)
        {
            switch (blendEq)
            {
                case BlendEquation.FuncAdd:
                    return BlendEquationMode.FuncAdd;
                    break;
                case BlendEquation.Min:
                    return BlendEquationMode.Min;
                    break;
                case BlendEquation.Max:
                    return BlendEquationMode.Max;
                    break;
                case BlendEquation.FuncSubtract:
                    return BlendEquationMode.FuncSubtract;
                    break;
                case BlendEquation.FuncReverseSubtract:
                    return BlendEquationMode.FuncReverseSubtract;
                    break;
                default:
                    return BlendEquationMode.FuncAdd;
                    break;
            }
        }
    }
}
