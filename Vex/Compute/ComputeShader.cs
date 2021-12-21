using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Compute
{
    /// <summary>
    /// A shader specialized in compute shading
    /// </summary>
    public sealed class ComputeShader : AssetObject
    {
        public ComputeShader()
        {

        }

        /// <summary>
        /// Returns the last compilation message (if any)
        /// </summary>
        public string LastCompileErrorMessage
        {
            get
            {
                return m_LastCompileErrorMessage;
            }
        }

        public void Compile(string source)
        {
            /*
             * Set source
             */
            m_Source = source;

            /*
             * Compile
             */
            Invalidate();
            
        }
        public void DispatchAndWait(int threadX,int threadY,int threadZ)
        {
            /*
             * Use compute shader program
             */
            GL.UseProgram(m_ProgramHandle);

            /*
             * Create fence
             */
            IntPtr fencePtr = GL.FenceSync(SyncCondition.SyncGpuCommandsComplete, WaitSyncFlags.None);

            /*
             * Dispatch compute shader
             */
            GL.DispatchCompute(threadX, threadY, threadZ);

            /*
             * Wait for fence
             */
            GL.WaitSync(fencePtr, WaitSyncFlags.None, 0);

            /*
             * Delete fence
             */
            GL.DeleteSync(fencePtr);
        }

        public ComputeWaitHandle Dispatch(int threadX,int threadY,int threadZ)
        {
            /*
            * Use compute shader program
            */
            GL.UseProgram(m_ProgramHandle);

            /*
             * Create fence
             */
            IntPtr fencePtr = GL.FenceSync(SyncCondition.SyncGpuCommandsComplete, WaitSyncFlags.None);

            /*
             * Dispatch compute shader
             */
            GL.DispatchCompute(threadX, threadY, threadZ);

            return new ComputeWaitHandle(fencePtr);
        }
        public void SetTexture3D()
        {

        }
        public void SetTexture2D()
        {

        }
        public void SetTexture1D()
        {

        }

        public override void Destroy()
        {

        }

        private void Invalidate()
        {
            /*
             * Create shader id
             */
            int shaderID = GL.CreateShader(ShaderType.ComputeShader);

            /*
             * Upload shader source
             */
            GL.ShaderSource(shaderID,m_Source);

            /*
             * Compile shader
             */
            GL.CompileShader(shaderID);

            /*
             * Create shader program id
             */
            int shaderProgram = GL.CreateProgram();

            /*
             * Attach shader
             */
            GL.AttachShader(shaderProgram, shaderID);

            /*
             * Link program
             */
            GL.LinkProgram(shaderProgram);

            /*
             * Set handles
             */
            m_ProgramHandle = shaderProgram;
            m_ShaderHandle = shaderID;
        }

        private string m_Source;
        private int m_ProgramHandle;
        private int m_ShaderHandle;
        private string m_LastCompileErrorMessage;
    }
}
