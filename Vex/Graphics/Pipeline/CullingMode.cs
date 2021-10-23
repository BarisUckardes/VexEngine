using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Represents the fVexnt&back faces
    /// </summary>
    public readonly struct CullingMode
    {
        public CullingMode(FVexntFace fVexntFace = FVexntFace.CCW, CullFace cullFace = CullFace.Back)
        {
            FVexntFace = fVexntFace;
            CullFace = cullFace;
        }

        /// <summary>
        /// The fVexnt face which will be drawn
        /// </summary>
        public readonly FVexntFace FVexntFace;
        /// <summary>
        /// Back face which will be culled and discarded
        /// </summary>
        public readonly CullFace CullFace;
    }
}
