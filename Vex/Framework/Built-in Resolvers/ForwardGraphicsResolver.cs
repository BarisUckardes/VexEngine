using OpenTK.Mathematics;
using Vex.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Vex.Extensions;
using Vex.Profiling;
namespace Vex.Framework
{

    /// <summary>
    /// Customized forward graphics resolver
    /// </summary>
    [TargetView(typeof(WorldGraphicsView))]
    public sealed class ForwardGraphicsResolver : GraphicsResolver
    {
       
        public ForwardGraphicsResolver()
        {
            m_Renderables = new List<ForwardMeshRenderable>();
            m_Observers = new List<ForwardMeshObserver>();
           
        }
        public override Type ExpectedRenderableType
        {
            get
            {
                return typeof(ForwardMeshRenderable);
            }
        }

        public override Type ExpectedObserverType
        {
            get
            {
                return typeof(ForwardMeshObserver);
            }
        }

        public override void OnObserverRegistered(ObserverComponent observer)
        {
            m_Observers.Add(observer as ForwardMeshObserver);
        }

        public override void OnObserverRemoved(ObserverComponent observer)
        {
            m_Observers.Remove(observer as ForwardMeshObserver);
        }

        public override void OnRenderableRegistered(RenderableComponent renderable)
        {
            m_Renderables.Add(renderable as ForwardMeshRenderable);
        }

        public override void OnRenderableRemoved(RenderableComponent renderable)
        {
            m_Renderables.Remove(renderable as ForwardMeshRenderable);
        }

        public override void Resolve()
        {
            Profiling.Profiler.StartProfile();

            /*
             * Create new command buffer
             */
            CommandBuffer commandBuffer = new CommandBuffer();

            /*
             * Start recording
             */
            commandBuffer.StartRecoding();

            /*
             * Set state
             */
            PipelineState state = new PipelineState(new Graphics.PolygonMode(PolygonFillFace.FrontAndBack,PolygonFillMethod.Fill),new CullingMode(TriangleFrontFace.CCW,CullFace.Back));
            commandBuffer.SetPipelineState(state);

            /*
             * Iterate each observer
             */
            for (int observerIndex = 0;observerIndex < m_Observers.Count; observerIndex++)
            {
                Profiler.StartProfile("Observer Submit");
                /*
                 * Get observer and its data
                 */
                ForwardMeshObserver observer = m_Observers[observerIndex];

                /*
                 * Get observer clear color
                 */
                Color4 clearColor = observer.ClearColor;

                /*
                 * Get observer framebuffer
                 */
                Framebuffer framebuffer = observer.Framebuffer == null ? Framebuffer2D.IntermediateFramebuffer : observer.Framebuffer;

                /*
                 * Get observer view matrix
                 */
                Matrix4 viewMatrix = observer.GetViewMatrix();

                /*
                 * Get observer pVexjection matrix
                 */
                Matrix4 projectionMatrix = observer.GetProjectionMatrix();

                /*
                 * Set observer framebuffer
                 */
                commandBuffer.SetFramebuffer(framebuffer);

                /*s
                 * Set framebuffer viewport
                 */
                Framebuffer2D framebufferAs2D = (Framebuffer2D)framebuffer;
                commandBuffer.SetViewport(Vector2.Zero, new Vector2(framebufferAs2D.Width, framebufferAs2D.Height));

                /*
                 * Clear color the framebuffer
                 */
                commandBuffer.ClearColor(clearColor);

                /*
                 * Render each sprite renderable
                 */
                for (int renderableIndex = 0; renderableIndex< m_Renderables.Count; renderableIndex++)
                {
                    Profiler.StartProfile("Submit draw call");

                    /*
                     * Set sprite renderable
                     */
                    ForwardMeshRenderable renderable = m_Renderables[renderableIndex];

                    /*
                     * Validate renderable
                     */
                    if (renderable == null)
                        continue;

                    /*
                     * Validate mesh
                     */
                    if (renderable.Mesh == null)
                        continue;

                    /*
                     * Validate material
                     */
                    if (renderable.Material == null)
                        continue;

                    VertexBuffer vertexBuffer = renderable.Mesh == null ? null : renderable.Mesh.VertexBuffer;
                    IndexBuffer indexBuffer = renderable.Mesh == null ? null : renderable.Mesh.IndexBuffer;
                    uint triangleCount = renderable.Mesh == null ? 0 : renderable.Mesh.IndexBuffer.IndexCount;

                    /*
                     * Set vertex buffer command
                     */
                    commandBuffer.SetVertexbuffer(vertexBuffer);

                    /*
                     * Set index buffer command
                     */
                    commandBuffer.SetIndexBuffer(indexBuffer);

                    /*
                     * Set shader pVexgram
                     */
                    commandBuffer.SetShaderProgram(renderable.Material != null ? renderable.Material.Program : null);

                    /*
                     * Create model matrix
                     */
                    Vector3 position = renderable.Spatial.Position.GetAsOpenTK();
                    position.X *= -1;
                    Vector3 rotation = renderable.Spatial.Rotation.GetAsOpenTK();
                    Vector3 scale = renderable.Spatial.Scale.GetAsOpenTK();
                    
                    /*
                     * Create mvp matrix
                     */
                    Matrix4 modelMatrix =
                        Matrix4.CreateScale(scale) *
                        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z))*
                        Matrix4.CreateTranslation(position);

                    Matrix4 mvp = modelMatrix * viewMatrix* projectionMatrix;
                    commandBuffer.SetUniformMat4x4(renderable.Material.Program, mvp, "v_Mvp");

                    /*
                     * Set material parameters
                     */
                    MaterialStageParameters[] stageParameters = renderable.Material.StageParameters;
                    for(int stageIndex = 0;stageIndex < stageParameters.Length;stageIndex++)
                    {
                        /*
                         * Set float parameters
                         */
                        MaterialParameterField<float>[] floatParamaters =  stageParameters[stageIndex].FloatParameters;
                        for(int parameterIndex = 0;parameterIndex< floatParamaters.Length;parameterIndex++)
                        {
                            commandBuffer.SetUniformFloat(renderable.Material.Program, floatParamaters[parameterIndex].Data, floatParamaters[parameterIndex].Name);
                        }

                        /*
                         * Set texture2d parameters
                         */
                        MaterialParameterField<Texture2D>[] texture2DParameters = stageParameters[stageIndex].Texture2DParameters;
                        for(int parameterIndex = 0;parameterIndex < texture2DParameters.Length;parameterIndex++)
                        {
                            commandBuffer.SetTexture2D(texture2DParameters[parameterIndex].Data, texture2DParameters[parameterIndex].Name, renderable.Material.Program);
                        }
                    }

                    /*
                     * Draw
                     */
                    commandBuffer.DrawIndexed((int)triangleCount);
                    Profiler.EndProfile();
                }

                Profiler.EndProfile();
            }

            /*
             * End recording
             */
            commandBuffer.EndRecording();

            /*
             * Execute command buffer
             */
            Profiler.StartProfile("Render");
            commandBuffer.Execute();
            Profiler.EndProfile();

            Profiling.Profiler.EndProfile();
        }

        private List<ForwardMeshObserver> m_Observers;
        private List<ForwardMeshRenderable> m_Renderables;
    }
}
