﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Engine;
using Vex.Types;
namespace Vex.Graphics
{
    /// <summary>
    /// Base mesh class for all meshes
    /// </summary>
    public abstract class Mesh : EngineObject
    {
        public abstract VertexLayout Layout { get; }

        /// <summary>
        /// Returns the vertex buffer of this mesh
        /// </summary>
        public VertexBuffer VertexBuffer
        {
            get
            {
                return m_VertexBuffer;
            }
        }

        /// <summary>
        /// Returns the index buffer of this mesh
        /// </summary>
        public IndexBuffer IndexBuffer
        {
            get
            {
                return m_IndexBuffer;
            }
        }

        /// <summary>
        /// Sets the vertex buffer data
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        protected void SetVertexBufferData<TData>(TData[] data) where TData:unmanaged
        {
            /*
             * Validate and delete former vertex buffer
             */
            ValidateAndDeleteVertexBuffer();

            /*
             * Create vertex buffer
             */
            m_VertexBuffer = new VertexBuffer();

            /*
             * Set data
             */
            m_VertexBuffer.SetData(data, (uint)(data.Length*TypeUtils.GetTypeSize<TData>()), Layout);
        }

        /// <summary>
        /// Sets the index buffer data
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        protected void SetIndexBufferData<TData>(TData[] data) where TData:unmanaged
        {
            /*
             * Validate and delete former index buffer
             */
            ValidateAndDeleteIndexBuffer();

            /*
             * Create index buffer
             */
            m_IndexBuffer = new IndexBuffer();

            /*
             * Set data
             */
            m_IndexBuffer.SetData(data, (uint)(data.Length* TypeUtils.GetTypeSize<TData>()));
        }

        /// <summary>
        /// Check and delete vertex buffer handles
        /// </summary>
        private void ValidateAndDeleteVertexBuffer()
        {
            if(m_VertexBuffer != null && !m_VertexBuffer.IsDisposed)
            {
                m_VertexBuffer.Dispose();
                m_VertexBuffer = null;
            }
        }

        /// <summary>
        /// Check and delete index buffer handles
        /// </summary>
        private void ValidateAndDeleteIndexBuffer()
        {
            if(m_IndexBuffer != null && !m_IndexBuffer.IsDisposed)
            {
                m_IndexBuffer.Dispose();
                m_IndexBuffer = null;
            }
        }

        sealed internal override void DestVexyInternal()
        {
            m_VertexBuffer?.Dispose();
            m_IndexBuffer?.Dispose();
        }

        private VertexBuffer m_VertexBuffer;
        private IndexBuffer m_IndexBuffer;
    }
}
