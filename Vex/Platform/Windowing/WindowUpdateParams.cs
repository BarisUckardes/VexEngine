using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Represents the update parameters for when creating a window
    /// </summary>
    public readonly struct WindowUpdateParams
    {
        public WindowUpdateParams(double updateFrequency, double renderFrequency, bool ısMultiThreaded)
        {
            UpdateFrequency = updateFrequency;
            RenderFrequency = renderFrequency;
            IsMultiThreaded = ısMultiThreaded;
        }

        /// <summary>
        /// Input update frequency
        /// </summary>
        public readonly double UpdateFrequency;

        /// <summary>
        /// Render update frequency
        /// </summary>
        public readonly double RenderFrequency;

        /// <summary>
        /// Is context multithreaded
        /// </summary>
        public readonly bool IsMultiThreaded;
    }
}
