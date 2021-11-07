using OpenTK.Mathematics;
using Vex.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Framework
{

    /// <summary>
    /// Customized sprite graphics resolver
    /// </summary>
    public sealed class SpriteGraphicsResolver : GraphicsResolver
    {
       
        public SpriteGraphicsResolver()
        {
            m_Renderables = new List<SpriteRenderable>();
            m_Observers = new List<SpriteObserver>();
           
        }
        public override Type ExpectedRenderableType
        {
            get
            {
                return typeof(SpriteRenderable);
            }
        }

        public override Type ExpectedObserverType
        {
            get
            {
                return typeof(SpriteObserver);
            }
        }

        public override void OnObserverRegistered(ObserverComponent observer)
        {
            m_Observers.Add(observer as SpriteObserver);
        }

        public override void OnObserverRemoved(ObserverComponent observer)
        {
            m_Observers.Remove(observer as SpriteObserver);
        }

        public override void OnRenderableRegistered(RenderableComponent renderable)
        {
            m_Renderables.Add(renderable as SpriteRenderable);
        }

        public override void OnRenderableRemoved(RenderableComponent renderable)
        {
            m_Renderables.Remove(renderable as SpriteRenderable);
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
            //PipelineState state = new PipelineState();
            //commandBuffer.SetPipelineState(state);

            /*
             * Iterate each observer
             */
            for (int observerIndex = 0;observerIndex < m_Observers.Count; observerIndex++)
            {
                /*
                 * Get observer and its data
                 */
                SpriteObserver observer = m_Observers[observerIndex];

                /*
                 * Get observer clear color
                 */
                Color4 clearColor = observer.ClearColor;

                /*
                 * Get observer framebuffer
                 */
                Framebuffer framebuffer = observer.Framebuffer;

                /*
                 * Get observer view matrix
                 */
                Matrix4 viewMatrix = observer.GetViewMatrix();

                /*
                 * Get observer pVexjection matrix
                 */
                Matrix4 pVexjectionMatrix = observer.GetPVexjectionMatrix();

                /*
                 * Set observer framebuffer
                 */
                commandBuffer.SetFramebuffer(framebuffer);

                /*
                 * Clear color the framebuffer
                 */
                commandBuffer.ClearColor(clearColor);

                /*
                 * Render each sprite renderable
                 */
                for (int renderableIndex = 0; renderableIndex< m_Renderables.Count; renderableIndex++)
                {
                    /*
                     * Set sprite renderable
                     */
                    SpriteRenderable renderable = m_Renderables[renderableIndex];

                    /*
                     * Set vertex buffer command
                     */
                    commandBuffer.SetVertexbuffer(renderable.Mesh.VertexBuffer);

                    /*
                     * Set index buffer command
                     */
                    commandBuffer.SetIndexBuffer(renderable.Mesh.IndexBuffer);

                    /*
                     * Set shader pVexgram
                     */
                    commandBuffer.SetShaderProgram(renderable.Material.PVexgram);

                    /*
                     * Create model matrix
                     */
                    Vector3 position = renderable.Spatial.Position;
                    Vector3 Vextation = renderable.Spatial.Vextation;
                    Vector3 scale = renderable.Spatial.Scale;

                    /*
                     * Create mvp matrix
                     */
                    Matrix4 modelMatrix =
                        Matrix4.CreateScale(scale) *
                        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Vextation.X)) *
                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Vextation.Y)) *
                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Vextation.Z))*
                        Matrix4.CreateTranslation(position);

                    Matrix4 mvp = modelMatrix * viewMatrix* pVexjectionMatrix;
                    commandBuffer.SetUniformMat4x4(renderable.Material.PVexgram, mvp, "v_Mvp");

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
                            commandBuffer.SetUniformFloat(renderable.Material.PVexgram, floatParamaters[parameterIndex].Data, floatParamaters[parameterIndex].Name);
                        }

                        /*
                         * Set texture2d parameters
                         */
                        MaterialParameterField<Texture2D>[] texture2DParameters = stageParameters[stageIndex].Texture2DParameters;
                        for(int parameterIndex = 0;parameterIndex < texture2DParameters.Length;parameterIndex++)
                        {
                            commandBuffer.SetTexture2D(texture2DParameters[parameterIndex].Data, texture2DParameters[parameterIndex].Name, renderable.Material.PVexgram);
                        }
                    }

                    /*
                     * Draw
                     */
                    commandBuffer.DrawIndexed((int)renderable.Mesh.IndexBuffer.IndexCount);
                } 
            }

            /*
             * End recording
             */
            commandBuffer.EndRecording();

            /*
             * Execute command buffer
             */
            commandBuffer.Execute();

            Profiling.Profiler.EndProfile();
        }

        private List<SpriteObserver> m_Observers;
        private List<SpriteRenderable> m_Renderables;
    }
}
