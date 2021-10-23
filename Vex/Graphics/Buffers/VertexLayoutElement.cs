using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Vertex layout element primitive for constructing vertex layout
    /// </summary>
    public struct VertexLayoutElement
    {
        public VertexLayoutElement(VertexLayoutDataType type, string name, bool ısNormalized = false)
        {
            Type = type;
            Name = name;
            Size = VertexLayoutDataTypeUtility.GetDataTypeSize(type);
            Offset = 0;
            IsNormalized = ısNormalized;
        }

        /// <summary>
        /// Returns the quantity of the primitive
        /// </summary>
        public uint ComponentCount
        {
            get
            {
                switch (Type)
                {
                    case VertexLayoutDataType.None:
                        return 0;
                        break;
                    case VertexLayoutDataType.Float:
                        return 1;
                        break;
                    case VertexLayoutDataType.Float2:
                        return 2;
                        break;
                    case VertexLayoutDataType.Float3:
                        return 3;
                        break;
                    case VertexLayoutDataType.float4:
                        return 4;
                        break;
                    case VertexLayoutDataType.Mat3:
                        return 3;
                        break;
                    case VertexLayoutDataType.Mat4:
                        return 4;
                        break;
                    case VertexLayoutDataType.Int:
                        return 1;
                        break;
                    case VertexLayoutDataType.Int2:
                        return 2;
                        break;
                    case VertexLayoutDataType.Int3:
                        return 3;
                        break;
                    case VertexLayoutDataType.In4:
                        return 4;
                        break;
                    case VertexLayoutDataType.Bool:
                        return 1;
                        break;
                    default:
                        return 0;
                        break;
                }
            }
        }

        public VertexLayoutDataType Type;
        public readonly string Name;
        public readonly uint Size;
        public uint Offset;
        public readonly bool IsNormalized;

       
    }
}
