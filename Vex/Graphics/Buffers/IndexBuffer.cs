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
    /// Represents a triangle buffer
    /// </summary>
    public class IndexBuffer : IDestroyableObject
    {
        public bool IsDisposed { get; set; }

        /// <summary>
        /// Native buffer ID
        /// </summary>
        public uint IndexBufferID
        {
            get
            {
                return m_IndexBufferID;
            }
        }

        /// <summary>
        /// Returns the index count of the total triangles
        /// </summary>
        public uint IndexCount
        {
            get
            {
                return m_IndexCount;
            }
        }

        public bool IsDestroyed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Sets the native buffer data on gpu
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="dataSize"></param>
        public void SetData<TData>(TData[] data, uint dataSize) where TData:struct
        {
            /*
             * Delete former handles
             */
            ValidateAndDeleteHandles();

            /*
             * Create new handle and bind it
             */
            GL.GenBuffers(1, out m_IndexBufferID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_IndexBufferID);

            /*
             * Set buffer data
             */
            GL.BufferData(BufferTarget.ElementArrayBuffer, (int)dataSize, data, BufferUsageHint.StaticDraw);

            /*
             * Unbind handles
             */
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            /*
             * Set index count
             */
            m_IndexCount = (uint)data.Length;

 
        }
     

        /// <summary>
        /// Validates then deletes the native handles
        /// </summary>
        private void ValidateAndDeleteHandles()
        {
            if(m_IndexBufferID != 0)
            {
                GL.DeleteBuffers(1, ref m_IndexBufferID);
            }

            m_IndexBufferID = 0;
        }

        public void Destroy()
        {
            GL.DeleteBuffer(m_IndexBufferID);
        }

        private uint m_IndexBufferID;
        private uint m_IndexCount;
    }
}
