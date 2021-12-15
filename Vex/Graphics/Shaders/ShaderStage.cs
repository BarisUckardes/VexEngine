using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Supported shader stages
    /// </summary>
    public enum ShaderStage
    {
        None = 0,
        //
        // Summary:
        //     Original was GL_FRAGMENT_SHADER = 0x8B30
        Fragment = 35632,
        //
        // Summary:
        //     Original was GL_FRAGMENT_SHADER_ARB = 0x8B30
        FragmentArb = 35632,
        //
        // Summary:
        //     Original was GL_VERTEX_SHADER = 0x8B31
        Vertex = 35633,
        //
        // Summary:
        //     Original was GL_VERTEX_SHADER_ARB = 0x8B31
        VertexArb = 35633,
        //
        // Summary:
        //     Original was GL_GEOMETRY_SHADER = 0x8DD9
        Geometry = 36313,
        //
        // Summary:
        //     Original was GL_TESS_EVALUATION_SHADER = 0x8E87
        TessEvaluation = 36487,
        //
        // Summary:
        //     Original was GL_TESS_CONTROL_SHADER = 0x8E88
        TessControl = 36488,
        //
        // Summary:
        //     Original was GL_COMPUTE_SHADER = 0x91B9
        Compute = 37305
    }
}
