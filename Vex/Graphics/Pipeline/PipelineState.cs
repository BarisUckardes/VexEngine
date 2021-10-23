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
    public readonly struct PipelineState
    {
        public PipelineState(PolygonMode polygonMode = new PolygonMode() , CullingMode cullingMode = new CullingMode(),
            BlendFunction sourceBlendFunction = BlendFunction.Zero, BlendFunction destinationBlendFunction = BlendFunction.Zero, BlendEquation blendEquation = BlendEquation.FuncAdd,
            DepthFunction depthFunction = DepthFunction.Always,bool depthTest = true,bool blending = false,bool faceCulling = true)
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
        public readonly PolygonMode PolygonMode;
        /// <summary>
        /// Which faces are rendered and culled
        /// </summary>
        public readonly CullingMode CullingMode;
        /// <summary>
        /// Source texture blend factor
        /// </summary>
        public readonly BlendFunction SourceBlendFunction;
        /// <summary>
        /// Destination texture blend factor
        /// </summary>
        public readonly BlendFunction DestinationBlendFunction;
        /// <summary>
        /// Blend eq
        /// </summary>
        public readonly BlendEquation BlendEquation;
        /// <summary>
        /// How the depth will be queried
        /// </summary>
        public readonly DepthFunction DepthFunction;
        /// <summary>
        /// Is this pipeline uses depth test ?
        /// </summary>
        public readonly bool DepthTest;
        /// <summary>
        /// Is this pipeline uses blending ?
        /// </summary>
        public readonly bool Blending;
        /// <summary>
        /// Is this pipeline uses face culling
        /// </summary>
        public readonly bool FaceCulling;
    }
}
