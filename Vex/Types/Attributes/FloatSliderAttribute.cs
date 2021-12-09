using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Types
{
    /// <summary>
    /// Supports a slider metadata for a float
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FloatSlider : Attribute
    {
        public FloatSlider(float min, float max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// The min amount can slider have
        /// </summary>
        public float Min { get; set; }

        /// <summary>
        /// The max amount can slider have
        /// </summary>
        public float Max { get; set; }
    }
}
