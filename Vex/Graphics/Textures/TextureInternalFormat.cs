using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Supported texture internal formats
    /// </summary>
    public enum TextureInternalFormat
    {
        DepthComponent = 0,
        Alpha = 1,
        RGB = 2,
        RGBA = 3,
        RGB8 = 4,
        RGB16 = 5,
        RGB32 = 6,
    }
}
