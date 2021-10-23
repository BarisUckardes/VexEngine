using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Set sub command buffer render command class
    /// </summary>
    public sealed class SetSubCommandBufferRC : RenderCommand
    {
        public SetSubCommandBufferRC(in CommandBuffer commandBuffer)
        {
            m_CommandBuffer = commandBuffer;
        }

        protected override void ExecuteImpl()
        {
            m_CommandBuffer.Execute();
        }


        private CommandBuffer m_CommandBuffer;
    }
}
