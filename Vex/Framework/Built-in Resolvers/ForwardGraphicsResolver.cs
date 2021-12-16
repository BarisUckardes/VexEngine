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
            m_ColorFramebuffers = new List<Framebuffer2D>();
            m_NormalFramebuffers = new List<Framebuffer2D>();

            /*
             * Create color write material
             */
            string colorVertexShaderText = @"#version 450
            layout(location = 0) in vec3 v_Position;
            layout(location = 1) in vec3 v_Normal;
            layout(location = 2) in vec2 v_Uv;




            out vec2 f_Uv;


            uniform mat4 v_Mvp;

            void main()

            {
                gl_Position = v_Mvp * vec4(v_Position, 1);
                f_Uv = v_Uv;
            }
            ";

            string colorFragmentShaderText = @"
 #version 450


  out vec4 ColorOut;


  in vec2 f_Uv;

  uniform sampler2D f_DeferredColorTexture;

  void main()

  {
      ColorOut = texture(f_DeferredColorTexture,f_Uv);
  }";
            Shader colorVertexShader = new Shader(ShaderStage.Vertex);
            colorVertexShader.Compile(colorVertexShaderText);

            Shader colorFragmentShader = new Shader(ShaderStage.Fragment);
            colorFragmentShader.Compile(colorFragmentShaderText);

            ShaderProgram colorShaderProgram = new ShaderProgram("Deferred", "Color");
            colorShaderProgram.LinkProgram(new List<Shader>() { colorVertexShader, colorFragmentShader });

            Material colorMaterial = new Material(colorShaderProgram);

            /*
             * Create normal write material
             */
            string normalvertexShaderText = @"
 #version 450

  layout(location = 0) in vec3 v_Position;
  layout(location = 1) in vec3 v_Normal;
  layout(location = 2) in vec2 v_Uv;

  out vec3 f_Normal;
  uniform mat4 v_Mvp;

  void main()
  {
      gl_Position = v_Mvp*vec4(v_Position,1);
      f_Normal = v_Normal;
  }";
            string normalFragmentShaderText = @"
 #version 450


  out vec4 NormalOut;

  in vec3 f_Normal;
  void main()
  {
      NormalOut = vec4(f_Normal,1);
  }
";
            Shader normalVertexShader = new Shader(ShaderStage.Vertex);
            normalVertexShader.Compile(normalvertexShaderText);

            Shader normalFragmentShader = new Shader(ShaderStage.Fragment);
            normalFragmentShader.Compile(normalFragmentShaderText);

            ShaderProgram normalShaderProgram = new ShaderProgram("Deferred", "Normal");
            normalShaderProgram.LinkProgram(new List<Shader>() { normalVertexShader, normalFragmentShader });

            Material normalMaterial = new Material(normalShaderProgram);

            /*
             * Register created materials
             */
            m_ColorMaterial = colorMaterial;
            m_NormalMaterial = normalMaterial;
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
             * Create observer specific color framebuffer
             */
            Framebuffer2D colorFramebuffer = new Framebuffer2D(Framebuffer2D.IntermediateFramebuffer.Width, Framebuffer2D.IntermediateFramebuffer.Height, TextureFormat.Rgba, TextureInternalFormat.Rgba32f, TextureDataType.UnsignedByte);
            colorFramebuffer.Name = "Unlit Color";

            /*
            * Create observer specific normal framebuffer
            */
            Framebuffer2D normalFramebuffer = new Framebuffer2D(Framebuffer2D.IntermediateFramebuffer.Width, Framebuffer2D.IntermediateFramebuffer.Height, TextureFormat.Rgba, TextureInternalFormat.Rgba32f, TextureDataType.UnsignedByte);
            normalFramebuffer.Name = "Object Normals";

            /*
             * Register as inspectable framebuffer
             */
            observer.RegisterFramebuffer2DResource(colorFramebuffer);
            observer.RegisterFramebuffer2DResource(normalFramebuffer);

            /*
             * Register observer
             */
            m_Observers.Add(observer);

            /*
             * Register color framebuffer
             */
            m_ColorFramebuffers.Add(colorFramebuffer);
            m_NormalFramebuffers.Add(normalFramebuffer);
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
            Framebuffer2D colorFramebuffer = m_ColorFramebuffers[observerIndex];

            /*
            * Get normal framebuffer for this observer
            */
            Framebuffer2D normalFramebuffer = m_NormalFramebuffers[observerIndex];

            /*
             * Remove frambuffer from the observer
             */
            observer.RemoveFramebuffer2DResource(colorFramebuffer);
            observer.RemoveFramebuffer2DResource(normalFramebuffer);

            /*
             * Destroy framebuffer
             */
            colorFramebuffer.Destroy();
            normalFramebuffer.Destroy();

            /*
             * Remove framebuffers so they can be garbage collected
             */
            m_ColorFramebuffers.RemoveAt(observerIndex);
            m_NormalFramebuffers.RemoveAt(observerIndex);

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

            {
                /*
                * Create new command buffer
               */
                CommandBuffer colorCommandBuffer = new CommandBuffer();
                CommandBuffer normalCommandBuffer = new CommandBuffer();

                /*
                 * Start recording
                 */
                colorCommandBuffer.StartRecoding();
                normalCommandBuffer.StartRecoding();

                /*
                 * Set state
                 */
                PipelineState state = new PipelineState(new Graphics.PolygonMode(PolygonFillFace.Front, PolygonFillMethod.Fill), new CullingMode(TriangleFrontFace.CCW, CullFace.Back));
                colorCommandBuffer.SetPipelineState(state);
                normalCommandBuffer.SetPipelineState(state);

                /*
                 * Iterate each observer
                 */
                for (int observerIndex = 0; observerIndex < m_Observers.Count; observerIndex++)
                {
                    Profiler.StartProfile("Observer Submit");

                    /*
                     * Get observer and its data
                     */
                    ObserverComponent observer = m_Observers[observerIndex];

                    /*
                     * Get observer clear color
                     */
                    Color4 clearColor = observer.ClearColor;

                    /*
                     * Get observer color framebuffer
                     */
                    Framebuffer2D colorFramebuffer = m_ColorFramebuffers[observerIndex];
                    Framebuffer2D normalFramebuffer = m_NormalFramebuffers[observerIndex];

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
                    colorCommandBuffer.SetFramebuffer(colorFramebuffer);

                    /*
                     * Set framebuffer viewport
                     */
                    colorCommandBuffer.SetViewport(Vector2.Zero, new Vector2(colorFramebuffer.Width, colorFramebuffer.Height));

                    /*
                     * Clear color the framebuffer
                     */
                    colorCommandBuffer.ClearColor(clearColor);
                    colorCommandBuffer.ClearDepth(1.0f);

                    /*
                     * Set color shader program
                     */
                    colorCommandBuffer.SetShaderProgram(m_ColorMaterial.Program);

                    /*
                     * Render each mesh color
                     */
                    for (int renderableIndex = 0; renderableIndex < m_Renderables.Count; renderableIndex++)
                    {
                        Profiler.StartProfile("Deferred Color Render Pass");

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
                        colorCommandBuffer.SetVertexbuffer(vertexBuffer);

                        /*
                         * Set index buffer command
                         */
                        colorCommandBuffer.SetIndexBuffer(indexBuffer);

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
                        colorCommandBuffer.SetUniformMat4x4(m_ColorMaterial.Program, mvp, "v_Mvp");

                        /*
                         * Set color texture
                         */
                        colorCommandBuffer.SetTexture2D(m_ColorMaterial.Program,renderable.ColorTexture,"f_DeferredColorTexture");

                        /*
                         * Draw color buffer
                         */
                        colorCommandBuffer.DrawIndexed((int)triangleCount);


                        Profiler.EndProfile();
                    }

                    /*
                     * Set normal framebuffer
                     */
                    normalCommandBuffer.SetFramebuffer(normalFramebuffer);

                    /*
                     * Set framebuffer viewport
                     */
                    normalCommandBuffer.SetViewport(Vector2.Zero, new Vector2(normalFramebuffer.Width, normalFramebuffer.Height));

                    /*
                     * Clear color the framebuffer
                     */
                    normalCommandBuffer.ClearColor(clearColor);
                    normalCommandBuffer.ClearDepth(1.0f);

                    /*
                    * Set color shader program
                    */
                    normalCommandBuffer.SetShaderProgram(m_NormalMaterial.Program);

                    /*
                     * Render each mesh normal
                     */
                    for (int renderableIndex = 0; renderableIndex < m_Renderables.Count; renderableIndex++)
                    {
                        Profiler.StartProfile("Deferred Normal Render Pass");

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
                        normalCommandBuffer.SetVertexbuffer(vertexBuffer);

                        /*
                         * Set index buffer command
                         */
                        normalCommandBuffer.SetIndexBuffer(indexBuffer);

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
                        normalCommandBuffer.SetUniformMat4x4(m_NormalMaterial.Program, mvp, "v_Mvp");

                        /*
                         * Draw color buffer
                         */
                        normalCommandBuffer.DrawIndexed((int)triangleCount);

                        Profiler.EndProfile();
                    }

                    Profiler.EndProfile();
                }

                /*
                 * End recording
                 */
                colorCommandBuffer.EndRecording();
                normalCommandBuffer.EndRecording();

                /*
                 * Execute command buffer
                 */
                colorCommandBuffer.Execute();
                normalCommandBuffer.Execute();
            }
        }

        private List<ObserverComponent> m_Observers;
        private List<Framebuffer2D> m_ColorFramebuffers;
        private List<Framebuffer2D> m_NormalFramebuffers;
        private List<ForwardMeshRenderable> m_Renderables;
        private Material m_ColorMaterial;
        private Material m_NormalMaterial;
    }
}
