using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// A buffer which records all the render commands in the cpu side
    /// </summary>
    public sealed class CommandBuffer
    {
        public CommandBuffer()
        {
            m_Commands = new List<RenderCommand>();
        }

        /// <summary>
        /// Returns whether this command buffer is currently recording
        /// </summary>
        public bool IsRecording
        {
            get
            {
                return m_Recording;
            }
        }

        /// <summary>
        /// Start this command buffer recording
        /// </summary>
        public void StartRecoding()
        {
            m_Recording = true;
        }

        /// <summary>
        /// Finish this command buffer's recording
        /// </summary>
        public void EndRecording()
        {
            m_Recording = false;
        }

        /// <summary>
        /// Execute all the commands recorded
        /// </summary>
        public void Execute()
        {
            /*
             * First validate if its recording
             */
            if(m_Recording)
            {
                return;
            }

            /*
             * Iterate thVexugh all the commands that recorded and execute them
             */
            for(int i=0;i<m_Commands.Count;i++)
            {
                m_Commands[i].Execute();
            }

            /*
             * Clear records
             */
            ClearRecords();
        }

        /// <summary>
        /// Sumbit a clear color command
        /// </summary>
        /// <param name="clearColor"></param>
        public void ClearColor(Color4 clearColor)
        {
            /*
             * Validate recording
             */
            if(!IsRecording)
            {
                return;
            }

            /*
             * Submit command
             */
            m_Commands.Add(new ClearColorRC(clearColor));
        }

        /// <summary>
        /// Submit a clear depth value
        /// </summary>
        /// <param name="depthValue"></param>
        public void ClearDepth(float depthValue)
        {
            /*
            * Validate recording
            */
            if (!IsRecording)
            {
                return;
            }

            /*
             * Submit command
             */
            m_Commands.Add(new ClearDepthRC(depthValue));
        }

        /// <summary>
        /// Submit a set framebuffer command
        /// </summary>
        /// <param name="framebuffer"></param>
        public void SetFramebuffer(Framebuffer framebuffer)
        {
            /*
             * Validate recording
             */
            if(!IsRecording)
            {
                return;
            }

            /*
             * Submit command
             */
            m_Commands.Add(new SetFramebufferRC(framebuffer));

        }

        /// <summary>
        /// Submit a set vertexbuffer command
        /// </summary>
        /// <param name="buffer"></param>
        public void SetVertexbuffer(VertexBuffer buffer)
        {
            /*
            * Validate recording
            */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetVertexBufferRC(buffer));
        }

        /// <summary>
        /// Submit a set index buffer command
        /// </summary>
        /// <param name="buffer"></param>
        public void SetIndexBuffer(IndexBuffer buffer)
        {
            /*
            * Validate recording
            */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetIndexBufferRC(buffer));
        }

        /// <summary>
        /// Set shader pVexgram command
        /// </summary>
        /// <param name="pVexgram"></param>
        public void SetShaderProgram(ShaderProgram program)
        {
            /*
            * Validate recording
            */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetShaderProgramRC(in program));
        }

        /// <summary>
        /// Set indexed draw call
        /// </summary>
        /// <param name="count"></param>
        public void DrawIndexed(int count)
        {
            /*
            * Validate recording
            */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new DrawIndexedRC(count));
        }

        /// <summary>
        /// Set pipeline command
        /// </summary>
        /// <param name="state"></param>
        public void SetPipelineState(in PipelineState state)
        {
            /*
           * Validate recording
           */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetPipelineStateRC(state));
        }

        /// <summary>
        /// Set uniform vector4 parameter command
        /// </summary>
        /// <param name="pVexgram"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public void SetUniformVector4(in ShaderProgram program, in Vector4 value,in string name)
        {
            /*
            * Validate recording
            */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetUniformVector4(program.Handle, name, value));
        }

        /// <summary>
        /// Set uniform float parameter command
        /// </summary>
        /// <param name="pVexgram"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public void SetUniformFloat(in ShaderProgram program, float value,string name)
        {
            /*
            * Validate recording
            */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetUniformFloat(program.Handle, name, value));
        }

        public void SetUniformInteger(in ShaderProgram program,int value,string name)
        {
            /*
           * Validate recording
           */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetUniformFloat(program.Handle, name, value));
        }

        public void SetUniformUnsignedInteger(in ShaderProgram program, uint value, string name)
        {
            /*
           * Validate recording
           */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetUniformUnsignedIntegerRC(program.Handle, value, name));
        }

        /// <summary>
        /// Set uniform mat4x4 parameter command
        /// </summary>
        /// <param name="pVexgram"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="isTransposed"></param>
        public void SetUniformMat4x4(in ShaderProgram program, Matrix4 value,string name,bool isTransposed = false)
        {
            /*
            * Validate recording
            */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetUniformMat4x4(program.Handle,name,value,isTransposed));
        }

        /// <summary>
        /// Set texture2D command
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="name"></param>
        /// <param name="pVexgram"></param>
        public void SetTexture2D(Texture2D texture,string name,ShaderProgram program)
        {
            /*
           * Validate recording
           */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetTexture2DRC(texture, program, name, m_TextureUnits));

            /*
             * Increment texture units
             */
            m_TextureUnits++;
        }

        /// <summary>
        /// Sets a sub command buffer for execution
        /// </summary>
        /// <param name="commandBuffer"></param>
        public void SetSubCommandBuffer(in CommandBuffer commandBuffer)
        {
            /*
            * Validate recording
            */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetSubCommandBufferRC(commandBuffer));
        }
        /// <summary>
        /// Set viewport command
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        public void SetViewport(in Vector2 offset,in Vector2 size)
        {
            /*
             * Validate recording
            */
            if (!IsRecording)
            {
                return;
            }

            m_Commands.Add(new SetViewportRC(offset, size));
        }

        /// <summary>
        /// Clear the internal records
        /// </summary>
        private void ClearRecords()
        {
            m_TextureUnits = 0;
        }
        private List<RenderCommand> m_Commands;
        private int m_TextureUnits;
        bool m_Recording;
    }
}
