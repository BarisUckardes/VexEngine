using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Utility class for native blend enum conversions
    /// </summary>
    public static class BlendFunctionUtils
    {
        /// <summary>
        /// Returns the native blending factor enums
        /// </summary>
        /// <param name="blendFunction"></param>
        /// <returns></returns>
        public static BlendingFactor GetNative(BlendFunction blendFunction)
        {
            switch (blendFunction)
            {
                case BlendFunction.Zero:
                    return BlendingFactor.Zero;
                    break;
                case BlendFunction.One:
                    return BlendingFactor.One;
                    break;
                case BlendFunction.SourceColor:
                    return BlendingFactor.SrcColor;
                    break;
                case BlendFunction.OneMinusSourceColor:
                    return BlendingFactor.OneMinusSrcColor;
                    break;
                case BlendFunction.DestColor:
                    return BlendingFactor.DstColor;
                    break;
                case BlendFunction.OneMinusDestColor:
                    return BlendingFactor.OneMinusDstColor;
                    break;
                case BlendFunction.SourceAlpha:
                    return BlendingFactor.SrcAlpha;
                    break;
                case BlendFunction.OneMinusSourceAlpha:
                    return BlendingFactor.OneMinusSrcAlpha;
                    break;
                case BlendFunction.DestAlpha:
                    return BlendingFactor.DstAlpha;
                    break;
                case BlendFunction.OneMinusDestAlpha:
                    return BlendingFactor.OneMinusDstAlpha;
                    break;
                case BlendFunction.ConstantColor:
                    return BlendingFactor.ConstantColor;
                    break;
                case BlendFunction.OneMinusConstantColor:
                    return BlendingFactor.OneMinusConstantColor;
                    break;
                case BlendFunction.ConstantAlpha:
                    return BlendingFactor.ConstantAlpha;
                    break;
                case BlendFunction.OneMinusConstantAlpha:
                    return BlendingFactor.OneMinusConstantAlpha;
                    break;
                default:
                    return BlendingFactor.Zero;
                    break;
            }
        }
    }
}
