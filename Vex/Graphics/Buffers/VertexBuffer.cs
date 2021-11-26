using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Vex.Framework;

namespace Vex.Graphics
{
    /// <summary>
    /// Represents a vertex buffer
    /// </summary>
    public class VertexBuffer : IDestroyableObject
    {

       

        /// <summary>
        /// Returns the native handle of this vertex buffer
        /// </summary>
        public VertexBufferHandle Handle
        {
            get
            {
                return m_Handle;
            }
        }

        /// <summary>
        /// Returns the total vertex count of this buffer
        /// </summary>
        public uint VertexCount
        {
            get
            {
                return m_VertexCount;
            }
        }

        public bool IsDestroyed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Sets the native data on gpu
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="dataSize"></param>
        /// <param name="layout"></param>
        public void SetData<TData>(TData[] data,uint dataSize,VertexLayout layout) where TData:struct
        {
            /*
             * Validate and delete former handles
             */
            ValidateAndDeleteHandles();

            uint vertexArrayID = 0;
            uint vertexBufferID = 0;
            /*
             * Create and bind new vertex array
             */
            GL.GenVertexArrays(1, out vertexArrayID);
            GL.BindVertexArray(vertexArrayID);

            /*
             * Create new vertex buffer
             */
            GL.GenBuffers(1, out vertexBufferID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferID);

            /*
             * Set buffer data
             */
            GL.BufferData(BufferTarget.ArrayBuffer, (int)dataSize, data, BufferUsageHint.StaticDraw);

            /*
             * Create vertex array layout
             */
            uint vertexLayoutIndex = 0;
            VertexLayoutElement[] elements = layout.Elements;
            for(int i=0;i<elements.Length;i++)
            {
                switch (elements[i].Type)
                {
                    case VertexLayoutDataType.None:
                        break;
                    case VertexLayoutDataType.Float:
                    case VertexLayoutDataType.Float2:
                    case VertexLayoutDataType.Float3:
                    case VertexLayoutDataType.float4:
                        GL.EnableVertexAttribArray(vertexLayoutIndex);
                        GL.VertexAttribPointer((int)vertexLayoutIndex,
                            (int)elements[i].ComponentCount,
                            VertexLayoutDataTypeUtility.GetVertexAttribPointerType(elements[i].Type),
                            elements[i].IsNormalized,
                            (int)layout.Stride,
                            (int)elements[i].Offset);
                        Console.WriteLine($"Component Count: {(int)elements[i].ComponentCount}");
                        Console.WriteLine($" Pointer Type: {VertexLayoutDataTypeUtility.GetVertexAttribPointerType(elements[i].Type).ToString()}");
                        Console.WriteLine($" Stride: {layout.Stride.ToString()}, Offset: {elements[i].Offset}");
                        vertexLayoutIndex++;
                        break;
                    case VertexLayoutDataType.Mat3:
                    case VertexLayoutDataType.Mat4:
                        uint count = elements[i].ComponentCount;
                        for(int p=0;p<count;p++)
                        {
                            GL.EnableVertexAttribArray(vertexLayoutIndex);
                            GL.VertexAttribPointer((int)vertexLayoutIndex,
                                (int)elements[i].ComponentCount,
                                VertexLayoutDataTypeUtility.GetVertexAttribPointerType(elements[i].Type),
                                elements[i].IsNormalized,
                                (int)layout.Stride,
                                (int)(elements[i].Offset + sizeof(float) * count * i));
                        }
                        vertexLayoutIndex++;
                        break;
                    case VertexLayoutDataType.Int:
                        break;
                    case VertexLayoutDataType.Int2:
                        break;
                    case VertexLayoutDataType.Int3:
                        break;
                    case VertexLayoutDataType.In4:
                        break;
                    case VertexLayoutDataType.Bool:
                        break;
                    default:
                        break;
                }
            }


            /*
             * Unbind handles
             */
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            /*
             * Create new handle
             */
            m_Handle = new VertexBufferHandle(vertexBufferID, vertexArrayID);

            /*
             *  Set vertex count
             */
            m_VertexCount = (uint)data.Length;
        }

     

        /// <summary>
        /// Validates then deletes the native handles
        /// </summary>
        private void ValidateAndDeleteHandles()
        {
            /*
             * Validate delete vertex buffer
             */
            if(m_Handle.VertexBufferID != 0)
            {
                uint id = m_Handle.VertexBufferID;
                GL.DeleteBuffers(1, ref id);
            }

            /*
             * Validate and delete vertex array
             */
            if(m_Handle.VertexArrayID != 0)
            {
                uint id = m_Handle.VertexArrayID;
                GL.DeleteVertexArrays(1, ref id);
            }

            /*
             * Create empty handle
             */
            m_Handle = new VertexBufferHandle();
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        private VertexBufferHandle m_Handle;
        private uint m_VertexCount;
    }
}
