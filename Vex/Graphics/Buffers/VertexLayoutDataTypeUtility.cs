using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Utility class for vertex layout
    /// </summary>
    public static class VertexLayoutDataTypeUtility
    {
        /// <summary>
        /// Returns the size of the data type in bytes
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
       public  static uint GetDataTypeSize(VertexLayoutDataType type)
        {
            switch (type)
            {
                case VertexLayoutDataType.None:
                    return 0;
                case VertexLayoutDataType.Float:
                    return 4;
                    break;
                case VertexLayoutDataType.Float2:
                    return 8;
                    break;
                case VertexLayoutDataType.Float3:
                    return 12;
                    break;
                case VertexLayoutDataType.float4:
                    return 16;
                    break;
                case VertexLayoutDataType.Mat3:
                    return 4 * 3 * 3;
                    break;
                case VertexLayoutDataType.Mat4:
                    return 4 * 4 * 4;
                    break;
                case VertexLayoutDataType.Int:
                    return 4;
                    break;
                case VertexLayoutDataType.Int2:
                    return 8;
                    break;
                case VertexLayoutDataType.Int3:
                    return 12;
                    break;
                case VertexLayoutDataType.In4:
                    return 16;
                    break;
                case VertexLayoutDataType.Bool:
                    return 1;
                    break;
                default:
                    return 0;
                    break;
            }
        }

        /// <summary>
        /// Returns the native vertex attribute pointer enum
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static VertexAttribPointerType GetVertexAttribPointerType(VertexLayoutDataType type)
        {
            switch (type)
            {
                case VertexLayoutDataType.None:
                    return VertexAttribPointerType.Byte;
                    break;
                case VertexLayoutDataType.Float:
                case VertexLayoutDataType.Float2:
                case VertexLayoutDataType.Float3:
                case VertexLayoutDataType.float4:
                case VertexLayoutDataType.Mat3:
                case VertexLayoutDataType.Mat4:
                    return VertexAttribPointerType.Float;
                    break;
                case VertexLayoutDataType.Int:
                case VertexLayoutDataType.Int2:
                case VertexLayoutDataType.Int3:
                case VertexLayoutDataType.In4:
                    return VertexAttribPointerType.Int;
                    break;
                case VertexLayoutDataType.Bool:
                    return VertexAttribPointerType.Byte;
                    break;
                default:
                    return VertexAttribPointerType.Byte;
                    break;
            }
        }
    }
}
