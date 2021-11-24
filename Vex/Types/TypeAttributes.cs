using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Types
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DontExposeThis : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ExposeThis : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FloatSlider : Attribute
    {
        public FloatSlider(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min { get; set; }
        public float Max { get; set; }
    }
}
