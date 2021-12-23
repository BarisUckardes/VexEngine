using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Asset
{
    /// <summary>
    /// Supported asset types
    /// </summary>
    public enum AssetType
    {
        Undefined = 0,
        Texture2D = 1,
        Shader = 2,
        ShaderProgram = 3,
        Material = 4,
        Framebuffer1D = 5,
        Framebuffer2D = 6,
        Framebuffer3D = 7,
        World = 8,
        EntityPrefab = 9,
        Definition = 10,
        Mesh = 11,
        CubTexture = 12
    }
}
