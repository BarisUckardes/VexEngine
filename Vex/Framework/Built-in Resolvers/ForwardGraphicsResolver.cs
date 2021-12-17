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
            /*
             * Initialize lists
             */
            m_Renderables = new List<ForwardMeshRenderable>();
            m_Observers = new List<ObserverComponent>();
            m_GBuffers = new List<Framebuffer2D>();
            m_LightExposureBuffers = new List<Framebuffer2D>();

            /*
             * Create gbuffer material
             */
            string gBufferVertexShaderText = @"#version 450
            layout(location = 0) in vec3 v_Position;
            layout(location = 1) in vec3 v_Normal;
            layout(location = 2) in vec2 v_Uv;

            out vec2 f_Uv;
            out vec3 f_Normal;
            out vec3 f_Position;
            uniform mat4 v_Mvp;
            uniform mat4 v_Model;
            void main()
            {
                vec4 objectWorldPosition = v_Model * vec4(v_Position, 1);
                gl_Position = v_Mvp * vec4(v_Position, 1);
                f_Uv = v_Uv;
                f_Normal = (v_Model*vec4(v_Normal,1)).xyz;
                f_Position = objectWorldPosition.xyz;
            }
            ";

             string gBufferFragmentShaderText = @"
             #version 450


              out vec4 ColorOut;
              out vec4 NormalOut;
              out vec3 PositionOut;

              in vec2 f_Uv;
              in vec3 f_Normal;
              in vec3 f_Position;
              uniform sampler2D f_DeferredColorTexture;

              void main()
              {
                  ColorOut = texture(f_DeferredColorTexture,f_Uv);
                  NormalOut = vec4(f_Normal,1);
                  PositionOut = f_Position;

              }";
            Shader gBufferVertexShader = new Shader(ShaderStage.Vertex);
            gBufferVertexShader.Compile(gBufferVertexShaderText);

            Shader gBufferFragmentShader = new Shader(ShaderStage.Fragment);
            gBufferFragmentShader.Compile(gBufferFragmentShaderText);

            ShaderProgram gBufferShaderProgram = new ShaderProgram("Deferred", "GBuffer");
            gBufferShaderProgram.LinkProgram(new List<Shader>() { gBufferVertexShader, gBufferFragmentShader });

            Material gBufferMaterial = new Material(gBufferShaderProgram);
            m_GBufferMaterial = gBufferMaterial;

            /*
             * Create light exposure material
             */

            /*
             * Create gbuffer material
             */
            string lightExposureVertexShaderText = @"#version 450
            layout(location = 0) in vec3 v_Position;
            layout(location = 1) in vec3 v_Normal;
            layout(location = 2) in vec2 v_Uv;

            out vec2 f_Uv;
            
            void main()
            {
                gl_Position = vec4(v_Position, 1);
                f_Uv = v_Uv;
            }
            ";

             string lightExposureFragmentShaderText = @"
             #version 450


              out float ExposureOut;

              in vec2 f_Uv;
              uniform sampler2D f_ColorTexture;
              uniform sampler2D f_PositionTexture;
              uniform sampler2D f_NormalTexture;

              void main()
              {
                    vec3 worldPosition = texture(f_PositionTexture,f_Uv).rgb;
                    vec3 worldNormal = texture(f_NormalTexture,f_Uv).rgb;
                    float lightDot = max(dot(worldNormal,vec3(0,1,0)),0.0f);
                    ExposureOut = lightDot;
              }";

            Shader lightExposureVertexShader = new Shader(ShaderStage.Vertex);
            lightExposureVertexShader.Compile(lightExposureVertexShaderText);

            Shader lightExposureFragmentShader = new Shader(ShaderStage.Fragment);
            lightExposureFragmentShader.Compile(lightExposureFragmentShaderText);

            ShaderProgram lightExposureShaderProgram = new ShaderProgram("Deferred", "Light Exposure");
            lightExposureShaderProgram.LinkProgram(new List<Shader>() { lightExposureVertexShader, lightExposureFragmentShader });

            Material lightExposureMaterial = new Material(lightExposureShaderProgram);
            m_LightExposureMaterial = lightExposureMaterial;

            /*
             * Create screen quad
             */
            List<StaticMeshVertex> vertexes = new List<StaticMeshVertex>();
            vertexes.Add(new StaticMeshVertex(new Vector3(-1, -1, 0), new Vector3(0, 0, 0), new Vector2(0, 0)));
            vertexes.Add(new StaticMeshVertex(new Vector3(1, -1, 0), new Vector3(0, 0, 0), new Vector2(1, 0)));
            vertexes.Add(new StaticMeshVertex(new Vector3(-1, 1, 0), new Vector3(0, 0, 0), new Vector2(0, 1)));
            vertexes.Add(new StaticMeshVertex(new Vector3(1, 1, 0), new Vector3(0, 0, 0), new Vector2(1, 1)));
            List<int> triangles = new List<int>() {0,1,2,1,3,2};
            StaticMesh mesh = new StaticMesh();
            mesh.SetVertexData(vertexes.ToArray());
            mesh.SetTriangleData(triangles.ToArray());
            m_ScreenQuad = mesh;
        }

        public override List<GraphicsObjectRegisterInfo> GetGraphicsComponentRegisterInformations()
        {
            return new List<GraphicsObjectRegisterInfo>()
            {
                new GraphicsObjectRegisterInfo(typeof(ForwardMeshRenderable),OnRenderableRegistered,OnRenderableRemoved)
            };
        }

        public override void OnObserverRegistered(ObserverComponent observer)
        {
            /*
             * Create observer specific gbuffer framebuffer
             */
            Framebuffer2D gBufferFramebuffer = new Framebuffer2D(
                Framebuffer2D.IntermediateFramebuffer.Width,
                Framebuffer2D.IntermediateFramebuffer.Height,
                new List<FramebufferAttachmentCreateParams>()
                {
                    new FramebufferAttachmentCreateParams("Color",TextureFormat.Rgb, TextureInternalFormat.Rgb32f, TextureDataType.UnsignedByte),
                    new FramebufferAttachmentCreateParams("Normal",TextureFormat.Rgb, TextureInternalFormat.Rgb32f, TextureDataType.UnsignedByte),
                    new FramebufferAttachmentCreateParams("Position",TextureFormat.Rgb,TextureInternalFormat.Rgb32f,TextureDataType.UnsignedByte)
                },
                true
                );;

            gBufferFramebuffer.Name = "GBuffer";

            /*
             * Register as inspectable framebuffer
             */
            observer.RegisterFramebuffer2DResource(gBufferFramebuffer);

            /*
             * Register gbuffer
             */
            m_GBuffers.Add(gBufferFramebuffer);

            /*
             * Create observer light exposure framebuffer
             */
            Framebuffer2D lightExposureFramebuffer = new Framebuffer2D(
                Framebuffer2D.IntermediateFramebuffer.Width,
                Framebuffer2D.IntermediateFramebuffer.Height,
                new List<FramebufferAttachmentCreateParams>()
                {
                    new FramebufferAttachmentCreateParams("Light Exposure",TextureFormat.Red, TextureInternalFormat.R32f, TextureDataType.UnsignedByte),
                },
                true
                ); ;

            lightExposureFramebuffer.Name = "Light Exposure";

            /*
             * Register as inspectable framebuffer
             */
            observer.RegisterFramebuffer2DResource(lightExposureFramebuffer);

            /*
             * Register light exposure framebuffer
             */
            m_LightExposureBuffers.Add(lightExposureFramebuffer);

            /*
            * Register observer
            */
            m_Observers.Add(observer);
        }

        public override void OnObserverRemoved(ObserverComponent observer)
        {
            /*
             * Get observer index
             */
            int observerIndex = m_Observers.IndexOf(observer);

            /*
             * Get color framebuffer for this observer
             */
            Framebuffer2D gBuffer = m_GBuffers[observerIndex];

            /*
             * Remove frambuffer from the observer
             */
            observer.RemoveFramebuffer2DResource(gBuffer);

            /*
             * Destroy framebuffer
             */
            gBuffer.Destroy();

            /*
             * Remove framebuffers so they can be garbage collected
             */
            m_GBuffers.RemoveAt(observerIndex);

            /*
             * Remove observer
             */
            m_Observers.Remove(observer);
        }

        private void OnRenderableRegistered(Component renderable)
        {
            m_Renderables.Add(renderable as ForwardMeshRenderable);
        }

        public void OnRenderableRemoved(Component renderable)
        {
            m_Renderables.Remove(renderable as ForwardMeshRenderable);
        }

        public override void Resolve()
        {
            /*
             * Create new command buffer
             */
            CommandBuffer gBufferCommandBuffer = new CommandBuffer();
            CommandBuffer lightExposureCommandBuffer = new CommandBuffer();

            /*
             * Start recording
             */
            gBufferCommandBuffer.StartRecoding();
            lightExposureCommandBuffer.StartRecoding();

            /*
             * Set gbuffer pipeline state
             */
            PipelineState gBufferPipelineState = new PipelineState(new Graphics.PolygonMode(PolygonFillFace.Front, PolygonFillMethod.Fill), new CullingMode(TriangleFrontFace.CCW, CullFace.Back));
            gBufferCommandBuffer.SetPipelineState(gBufferPipelineState);

            /*
             * Iterate each observer for gbuffer
             */
            for (int observerIndex = 0; observerIndex < m_Observers.Count; observerIndex++)
            {
                /*
                 * Get observer and its data
                 */
                ObserverComponent observer = m_Observers[observerIndex];

                /*
                 * Get observer clear color
                 */
                Color4 clearColor = Color4.Black;

                /*
                 * Get observer gBuffer
                 */
                Framebuffer2D gBuffer = m_GBuffers[observerIndex];

                /*
                 * Get observer view matrix
                 */
                Matrix4 viewMatrix = observer.GetViewMatrix();

                /*
                 * Get observer pVexjection matrix
                 */
                Matrix4 projectionMatrix = observer.GetProjectionMatrix();

                /*
                 * Set color framebuffer
                 */
                gBufferCommandBuffer.SetFramebuffer(gBuffer);

                /*
                 * Set framebuffer viewport
                 */
                gBufferCommandBuffer.SetViewport(Vector2.Zero, new Vector2(gBuffer.Width, gBuffer.Height));

                /*
                 * Clear color the framebuffer
                 */
                gBufferCommandBuffer.ClearColor(clearColor);
                gBufferCommandBuffer.ClearDepth(1.0f);

                /*
                 * Set color shader program
                 */
                gBufferCommandBuffer.SetShaderProgram(m_GBufferMaterial.Program);

                /*
                 * Render for gbuffer
                 */
                for (int renderableIndex = 0; renderableIndex < m_Renderables.Count; renderableIndex++)
                {
                    Profiler.StartProfile("Deferred GBuffer Pass");

                    /*
                     * Get renderable
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
                     * Validate vertex and index buffer
                    */
                    if (renderable.Mesh.VertexBuffer == null || renderable.Mesh.IndexBuffer == null)
                        continue;

                    /*
                     * Get vertex buffer
                     */
                    VertexBuffer vertexBuffer = renderable.Mesh == null ? null : renderable.Mesh.VertexBuffer;

                    /*
                     * Get index buffer
                     */
                    IndexBuffer indexBuffer = renderable.Mesh == null ? null : renderable.Mesh.IndexBuffer;

                    /*
                     * Get triangle count
                     */
                    uint triangleCount = renderable.Mesh == null ? 0 : renderable.Mesh.IndexBuffer.IndexCount;

                    /*
                     * Set vertex buffer command
                     */
                    gBufferCommandBuffer.SetVertexbuffer(vertexBuffer);

                    /*
                     * Set index buffer command
                     */
                    gBufferCommandBuffer.SetIndexBuffer(indexBuffer);

                    /*
                     * Create model matrix
                     */
                    Vector3 position = renderable.Spatial.Position.GetAsOpenTK();
                    Vector3 rotation = renderable.Spatial.Rotation.GetAsOpenTK();
                    Vector3 scale = renderable.Spatial.Scale.GetAsOpenTK();

                    /*
                     * Create mvp matrix
                     */
                    Matrix4 modelMatrix =
                        Matrix4.CreateScale(scale) *
                        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z)) *
                        Matrix4.CreateTranslation(position);

                    Matrix4 mvp = modelMatrix * viewMatrix * projectionMatrix;
                    gBufferCommandBuffer.SetUniformMat4x4(m_GBufferMaterial.Program, mvp, "v_Mvp");
                    gBufferCommandBuffer.SetUniformMat4x4(m_GBufferMaterial.Program, modelMatrix, "v_Model");

                    /*
                     * Set color texture
                     */
                    gBufferCommandBuffer.SetTexture2D(m_GBufferMaterial.Program, renderable.ColorTexture, "f_DeferredColorTexture");

                    /*
                     * Draw color buffer
                     */
                    gBufferCommandBuffer.DrawIndexed((int)triangleCount);


                    Profiler.EndProfile();
                }

                
            }

            /*
             * End recording
             */
            gBufferCommandBuffer.EndRecording();

            /*
            * Set light exposure pipeline state
            */
            PipelineState lightExposurePipelineState = new PipelineState(new Graphics.PolygonMode(PolygonFillFace.Front, PolygonFillMethod.Fill), new CullingMode(TriangleFrontFace.CCW, CullFace.Back));
            lightExposurePipelineState.DepthTest = false;
            lightExposureCommandBuffer.SetPipelineState(lightExposurePipelineState);
            for (int observerIndex = 0; observerIndex < m_Observers.Count; observerIndex++)
            {
                /*
                 * Get observer clear color
                 */
                Color4 clearColor = Color4.Black;

                /*
                 * Get observer gBuffer
                 */
                Framebuffer2D lightExposureBuffer = m_LightExposureBuffers[observerIndex];

                /*
                 * Set color framebuffer
                 */
                lightExposureCommandBuffer.SetFramebuffer(lightExposureBuffer);

                /*
                 * Set framebuffer viewport
                 */
                lightExposureCommandBuffer.SetViewport(Vector2.Zero, new Vector2(lightExposureBuffer.Width, lightExposureBuffer.Height));

                /*
                 * Clear color the framebuffer
                 */
                lightExposureCommandBuffer.ClearColor(clearColor);
                lightExposureCommandBuffer.ClearDepth(1.0f);

                /*
                 * Set color shader program
                 */
                lightExposureCommandBuffer.SetShaderProgram(m_LightExposureMaterial.Program);

                /*
                   * Get vertex buffer
                   */
                VertexBuffer vertexBuffer = m_ScreenQuad.VertexBuffer;

                /*
                 * Get index buffer
                 */
                IndexBuffer indexBuffer = m_ScreenQuad.IndexBuffer;

                /*
                 * Get triangle count
                 */
                uint triangleCount = m_ScreenQuad.IndexBuffer.IndexCount;

                /*
                 * Set vertex buffer command
                 */
                lightExposureCommandBuffer.SetVertexbuffer(vertexBuffer);

                /*
                 * Set index buffer command
                 */
                lightExposureCommandBuffer.SetIndexBuffer(indexBuffer);

                /*
                 * Set color texture
                 */
                lightExposureCommandBuffer.SetTexture2D(m_LightExposureMaterial.Program, m_GBuffers[observerIndex].Attachments[0].Texture, "f_ColorTexture");
                lightExposureCommandBuffer.SetTexture2D(m_LightExposureMaterial.Program, m_GBuffers[observerIndex].Attachments[1].Texture, "f_NormalTexture");
                lightExposureCommandBuffer.SetTexture2D(m_LightExposureMaterial.Program, m_GBuffers[observerIndex].Attachments[2].Texture, "f_PositionTexture");
                

                /*
                 * Draw color buffer
                 */
                lightExposureCommandBuffer.DrawIndexed((int)triangleCount);
            }

            /*
             * End recording
             */
            lightExposureCommandBuffer.EndRecording();

            /*
             * Execute command buffer
             */
            gBufferCommandBuffer.Execute();

            /*
             * Execute command buffer
             */
            lightExposureCommandBuffer.Execute();
        }

        private List<ObserverComponent> m_Observers;
        private List<Framebuffer2D> m_GBuffers;
        private List<Framebuffer2D> m_LightExposureBuffers;
        private List<ForwardMeshRenderable> m_Renderables;
        private Material m_GBufferMaterial;
        private Material m_LightExposureMaterial;
        private StaticMesh m_ScreenQuad;
    }
}
