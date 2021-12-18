using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// A render command which sets a new pipeline state
    /// </summary>
    public sealed class SetPipelineStateRC : RenderCommand
    {
        public SetPipelineStateRC(in PipelineState state)
        {
            m_State = state;
        }
        protected override void ExecuteImpl()
        {
            
            /*
             * Set polygon mode
             */
            GL.PointSize(15.0f);
            GL.LineWidth(1.0f);
            GL.PolygonMode(
                PolygonFillfaceUtils.GetNative(m_State.PolygonMode.FillFace),
                PolygonFillMethodUtils.GetNative(m_State.PolygonMode.FillMethod)
                );

            /*
             * Set blend mode
             */
            if(!m_State.Blending)
            {
                GL.Disable(EnableCap.Blend);
            }
            else
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendFunctionUtils.GetNative(m_State.SourceBlendFunction),BlendFunctionUtils.GetNative(m_State.DestinationBlendFunction));

                /*
                * Set blend equation
                 */
                GL.BlendEquation(BlendEquationUtils.GetNative(m_State.BlendEquation));
            }

            /*
             * Set depth function
             */
            if(!m_State.DepthTest)
            {
                
                GL.Disable(EnableCap.DepthTest);
                GL.DepthMask(false);
            }
            else
            {
                
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunctionUtils.GetNative(m_State.DepthFunction));
                GL.DepthMask(true);
            }
            

            /*
             * Set culling mode
             */
            if(!m_State.FaceCulling)
            {
                GL.Disable(EnableCap.CullFace);

            }
            else
            {
                GL.Enable(EnableCap.CullFace);

                GL.FrontFace(TriangleFrontFaceUtility.GetNative(m_State.CullingMode.TriangleFrontFace));
                GL.CullFace(CullFaceUtility.GetNative(m_State.CullingMode.CullFace));
            }
        }

        private readonly PipelineState m_State;
    }
}
