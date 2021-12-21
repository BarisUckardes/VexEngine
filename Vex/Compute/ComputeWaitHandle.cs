using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Compute
{
    /// <summary>
    /// And wait handle object for compute shaders
    /// </summary>
    public sealed class ComputeWaitHandle
    {
        internal ComputeWaitHandle(in IntPtr fencePtr)
        {
            m_FencePtr = fencePtr;
        }

        public void WaitForFinish()
        {
            /*
            * Wait for fence
            */
            GL.WaitSync(m_FencePtr, WaitSyncFlags.None, 0);

            /*
             * Delete fence
             */
            GL.DeleteSync(m_FencePtr);
        }
        /// <summary>
        /// Returns the fence ptr
        /// </summary>
        internal IntPtr FencePtr
        {
            get
            {
                return m_FencePtr;
            }
        }
        private IntPtr m_FencePtr;
    }
}
