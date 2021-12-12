using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Represents the whole pipeline state
    /// </summary>
    public struct PipelineState
    {
        public PipelineState(PolygonMode polygonMode , CullingMode cullingMode,
            BlendFunction sourceBlendFunction = BlendFunction.Zero, BlendFunction destinationBlendFunction = BlendFunction.Zero, BlendEquation blendEquation = BlendEquation.FuncAdd,
            DepthFunction depthFunction = DepthFunction.Lequal,bool depthTest = true,bool blending = false,bool faceCulling = true)
        {
            PolygonMode = polygonMode;
            CullingMode = cullingMode;
            SourceBlendFunction = sourceBlendFunction;
            DestinationBlendFunction = destinationBlendFunction;
            BlendEquation = blendEquation;
            DepthFunction = depthFunction;
            DepthTest = depthTest;
            Blending = blending;
            FaceCulling = faceCulling;
        }

        /// <summary>
        /// How the polygons will be drawed
        /// </summary>
        public PolygonMode PolygonMode;
        /// <summary>
        /// Which faces are rendered and culled
        /// </summary>
        public CullingMode CullingMode;
        /// <summary>
        /// Source texture blend factor
        /// </summary>
        public BlendFunction SourceBlendFunction;
        /// <summary>
        /// Destination texture blend factor
        /// </summary>
        public BlendFunction DestinationBlendFunction;
        /// <summary>
        /// Blend eq
        /// </summary>
        public BlendEquation BlendEquation;
        /// <summary>
        /// How the depth will be queried
        /// </summary>
        public DepthFunction DepthFunction;
        /// <summary>
        /// Is this pipeline uses depth test ?
        /// </summary>
        public bool DepthTest;
        /// <summary>
        /// Is this pipeline uses blending ?
        /// </summary>
        public bool Blending;
        /// <summary>
        /// Is this pipeline uses face culling
        /// </summary>
        public bool FaceCulling;
    }
}
