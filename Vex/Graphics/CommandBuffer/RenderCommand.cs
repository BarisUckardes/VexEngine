using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Base class for all the rendercommands
    /// </summary>
    public abstract class RenderCommand
    {
        /// <summary>
        /// Executes the render command
        /// </summary>
        public void Execute()
        {
            ExecuteImpl();
        }


        /// <summary>
        /// User-defined execute method
        /// </summary>
        protected abstract void ExecuteImpl();
    }
}
