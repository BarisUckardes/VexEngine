using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Supported texture formats
    /// </summary>
    public enum TextureFormat
    {
        RGB = 0,
        RGBA = 1,
        DepthComponent = 2,
        UnsignedShort = 3,
        UnsignedInt = 4,
        ColorIndex = 5,
        Alpha = 6
    }
}
