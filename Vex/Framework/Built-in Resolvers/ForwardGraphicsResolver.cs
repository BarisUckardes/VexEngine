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
            m_AmbientOcclusionBuffers = new List<Framebuffer2D>();
            m_PointLights = new List<DeferredPointLight>();
            m_SSAOKernels = new List<Vector3>();
            m_BlurBuffers = new List<Framebuffer2D>();
            m_FinalColorBuffers = new List<Framebuffer2D>();
            m_InstancedMeshes = new List<DeferredInstancedMesh>();

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
            uniform mat4 v_MvpMatrix;
            uniform mat4 v_ModelMatrix;
            uniform mat4 v_ViewMatrix;
            uniform mat4 v_ProjectionMatrix;
            uniform mat4 v_NormalMatrix;
            void main()
            {
                gl_Position = v_MvpMatrix * vec4(v_Position, 1);
                mat3 nMatrix = transpose(inverse(mat3(v_ViewMatrix*v_ModelMatrix)));
                f_Uv = v_Uv;
                f_Normal = nMatrix*v_Normal;
                f_Position = (v_ViewMatrix*v_ModelMatrix * vec4(v_Position, 1)).xyz;
            }
            ";

             string gBufferFragmentShaderText = @"
             #version 450


              out vec4 ColorOut;
              out vec3 NormalOut;
              out vec3 PositionOut;

              in vec2 f_Uv;
              in vec3 f_Normal;
              in vec3 f_Position;

              uniform sampler2D f_DeferredColorTexture;
              uniform vec4 f_ViewPosition;
              void main()
              {
                  ColorOut = texture(f_DeferredColorTexture,f_Uv);
                  NormalOut = normalize(f_Normal);
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
            string ambientOcclusionVertexShaderText = @"#version 450
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

              string ambientOcclusionFragmentShaderText =
              @"
              #version 450

              out float ColorOut;

              in vec2 f_Uv;

              int kernelSize = 64;
              float radius = 0.15f;
              float bias = 0.01f;
              float power = 3.5f;
              const vec2 noiseScale = vec2(1920.0/4.0,1080.0/4.0);

              uniform sampler2D f_PositionTexture;
              uniform sampler2D f_NormalTexture;
              uniform sampler2D f_DepthBuffer;
                
             
              uniform vec3[64] f_SSAOKernels;
              uniform sampler2D f_NoiseTexture;
              uniform mat4 f_ProjectionMatrix;
              void main()
              {
                    vec3 viewPosition = texture(f_PositionTexture,f_Uv).rgb;
                    vec3 viewNormal = texture(f_NormalTexture,f_Uv).rgb;
                    vec3 randomVec = texture(f_NoiseTexture,f_Uv*noiseScale).xyz;
                    vec3 tangent = normalize(randomVec-viewNormal*dot(randomVec,viewNormal));
                    
                    vec3 bitangent = cross(viewNormal,tangent);
                    mat3 TBN = mat3(tangent,bitangent,viewNormal);
                    
                    float occlusion = 0.0f;
                    for(int kernelIndex = 0;kernelIndex < kernelSize;kernelIndex++)
                    {
                        vec3 samplePosition = TBN * f_SSAOKernels[kernelIndex];
                        samplePosition = samplePosition*radius + viewPosition;

                        vec4 offset = vec4(samplePosition,1.0);
                        offset = f_ProjectionMatrix*offset;
                        offset.xyz /= offset.w;
                        offset.xyz  = offset.xyz*0.5f + 0.5f;

                        vec3 occluderPosition = texture(f_PositionTexture,offset.xy).rgb;
                        float rangeCheck = smoothstep(0.0,1.0,radius / abs(viewPosition.z - occluderPosition.z));    
                        occlusion += (occluderPosition.z >= samplePosition.z + bias ? 1.0 : 0.0)*rangeCheck;
                    }
                    occlusion = 1.0f-pow((occlusion / kernelSize)*power,2);
                    ColorOut = occlusion; 
              }";

            Shader ambientOcclusionVertexShader = new Shader(ShaderStage.Vertex);
            ambientOcclusionVertexShader.Compile(ambientOcclusionVertexShaderText);

            Shader ambientOcclusionFragmentShader = new Shader(ShaderStage.Fragment);
            ambientOcclusionFragmentShader.Compile(ambientOcclusionFragmentShaderText);

            ShaderProgram ambientOcclusionShaderProgram = new ShaderProgram("Deferred", "Ambient Occlusion");
            ambientOcclusionShaderProgram.LinkProgram(new List<Shader>() { ambientOcclusionVertexShader, ambientOcclusionFragmentShader });

            Material ambientOcclusionMaterial = new Material(ambientOcclusionShaderProgram);
            m_AmbientOcclusionMaterial = ambientOcclusionMaterial;

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

            /*
             * Create SSAO kernels
             */
            Random rangeGenerator = new Random();
            for(int kernelIndex = 0;kernelIndex<64;kernelIndex++)
            {

                Vector3 kernel = new Vector3(
                    ((float)rangeGenerator.NextDouble())*2 -1.0f,
                    ((float)rangeGenerator.NextDouble()) * 2 - 1.0f,
                    ((float)rangeGenerator.NextDouble())
                    );
                kernel.Normalize();
                kernel *= (float)rangeGenerator.NextDouble();

                float scale = (float)kernelIndex / 64.0f;
                kernel *= Lerp(0.1f, 1.0f, scale * scale);
                m_SSAOKernels.Add(kernel);
            }

            /*
             * Create SSAO noise texture
             */
            Texture2D ssaoNoiseTexture = new Texture2D(4, 4, TextureFormat.Rgb, TextureInternalFormat.Rgb32f, TextureDataType.Float, TextureWrapMode.Repeat, TextureWrapMode.Repeat, TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            List<Vector3> noiseTextureKernels = new List<Vector3>();
            for(int kernelIndex = 0;kernelIndex < 16;kernelIndex++)
            {
                noiseTextureKernels.Add(new Vector3(
                     (float)rangeGenerator.NextDouble() * 2 - 1,
                    (float)rangeGenerator.NextDouble() * 2 - 1,
                    0
                    ));
            }
            ssaoNoiseTexture.SetData(noiseTextureKernels.ToArray(), false);
            m_SSAONoiseTexture = ssaoNoiseTexture;

            /*
             * Create blur pass
             */
            string blurVertexShaderText = @"#version 450
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

            string blurFragmentShaderText =
            @"
              #version 450

              out float ColorOut;

              in vec2 f_Uv;
              uniform sampler2D f_AmbientOcclusionTexture;
             
              void main()
              {
                    vec2 texelSize = 1.0f / vec2(textureSize(f_AmbientOcclusionTexture,0));
                    float result = 0.0f;
                    for(int x = -2;x < 2;++x)
                    {
                        for(int y = -2;y<2;++y)
                        {
                            vec2 offset = vec2(float(x),float(y))*texelSize;
                            result+= texture(f_AmbientOcclusionTexture,f_Uv + offset).r;
                        }
                    }
                    result = result / (4.0f*4.0f);
                    ColorOut = result;
              }";

            Shader blurVertexShader = new Shader(ShaderStage.Vertex);
            blurVertexShader.Compile(blurVertexShaderText);

            Shader blurFragmentShader = new Shader(ShaderStage.Fragment);
            blurFragmentShader.Compile(blurFragmentShaderText);

            ShaderProgram blurShaderProgram = new ShaderProgram("Deferred","Blur");
            blurShaderProgram.LinkProgram(new List<Shader>() { blurVertexShader, blurFragmentShader });

            Material blurMaterial = new Material(blurShaderProgram);
            m_BlurMaterial = blurMaterial;

            /*
             * Final color pass
             */
            string finalColorVertexShaderText = @"#version 450
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

            string finalColorFragmentShaderText =
            @"
              #version 450

              out vec3 ColorOut;

              in vec2 f_Uv;
              const float nearPlane = 0.001f;
              const float farPlane = 10.0f;
              uniform sampler2D f_AmbientOcclusionTexture;
              uniform sampler2D f_ColorTexture;
              uniform sampler2D f_NormalTexture;
              uniform sampler2D f_ViewSpacePositionTexture;
              uniform samplerCube f_CubeTexture;
              uniform mat4 f_ProjectionMatrix;
              uniform vec4 f_TextureSize;
            
              float amount = 0.8f;
               
              void main()
              {
                    float ambientFactor = texture(f_AmbientOcclusionTexture,f_Uv).r;
                    vec3 normalViewSpace = texture(f_NormalTexture,f_Uv).rgb;
                    float diffuseFactor = max(dot(normalViewSpace,vec3(0,1,0)),0);
                    vec3 cubeColor = texture(f_CubeTexture,reflect(texture(f_ViewSpacePositionTexture,f_Uv).rgb,normalViewSpace)).rgb;
                    ColorOut = texture(f_ColorTexture,f_Uv).rgb*cubeColor*ambientFactor*diffuseFactor;
                    //ColorOut = cubeColor;
              }";

            Shader finalColorVertexShader = new Shader(ShaderStage.Vertex);
            finalColorVertexShader.Compile(finalColorVertexShaderText);

            Shader finalColorFragmentShader = new Shader(ShaderStage.Fragment);
            finalColorFragmentShader.Compile(finalColorFragmentShaderText);

            ShaderProgram finalColorShaderProgram = new ShaderProgram("Deferred", "Final Color");
            finalColorShaderProgram.LinkProgram(new List<Shader>() {finalColorVertexShader,finalColorFragmentShader});

            Material finalColorMaterial = new Material(finalColorShaderProgram);
            m_FinalColorMaterial = finalColorMaterial;

            /*
             * Load cubemap
             */
            List<string> paths = new List<string>()
            {
                @"C:\Users\PC\Documents\Cubemap\right.jpg",
                @"C:\Users\PC\Documents\Cubemap\left.jpg",
                @"C:\Users\PC\Documents\Cubemap\top.jpg",
                @"C:\Users\PC\Documents\Cubemap\bottom.jpg",
                @"C:\Users\PC\Documents\Cubemap\front.jpg",
                @"C:\Users\PC\Documents\Cubemap\back.jpg"
            };

            CubeTexture cubeTexture = new CubeTexture();
            cubeTexture.LoadFromFile(paths);
            m_CubeTexture = cubeTexture;

        }
        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat + (secondFloat - firstFloat) * by;
        }
        public override List<GraphicsObjectRegisterInfo> GetGraphicsComponentRegisterInformations()
        {
            return new List<GraphicsObjectRegisterInfo>()
            {
                new GraphicsObjectRegisterInfo(typeof(ForwardMeshRenderable),OnRenderableRegistered,OnRenderableRemoved),
                new GraphicsObjectRegisterInfo(typeof(DeferredPointLight),OnPointLightRegister,OnPointLightRemove),
                new GraphicsObjectRegisterInfo(typeof(DeferredInstancedMesh),OnInstancedMeshRegister,OnInstancedMeshRemove)
            };
        }

        public void OnPointLightRegister(Component pointLight)
        {
            m_PointLights.Add(pointLight as DeferredPointLight);
        }
        public void OnPointLightRemove(Component pointLight)
        {
            m_PointLights.Remove(pointLight as DeferredPointLight);
        }
        public void OnInstancedMeshRegister(Component mesh)
        {
            m_InstancedMeshes.Add(mesh as DeferredInstancedMesh);
        }
        public void OnInstancedMeshRemove(Component mesh)
        {
            m_InstancedMeshes.Remove(mesh as DeferredInstancedMesh);
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
                    new FramebufferAttachmentCreateParams("Position",TextureFormat.Rgb,TextureInternalFormat.Rgb32f,TextureDataType.UnsignedByte),
                },
                true
                );

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
             * Create observer ambient occlusion framebuffer
             */
            Framebuffer2D ambientOcclusionFramebuffer = new Framebuffer2D(
                Framebuffer2D.IntermediateFramebuffer.Width,
                Framebuffer2D.IntermediateFramebuffer.Height,
                new List<FramebufferAttachmentCreateParams>()
                {
                    new FramebufferAttachmentCreateParams("Occlusion",TextureFormat.Red, TextureInternalFormat.R32f, TextureDataType.Float),
                },
                true
                ); ;

            ambientOcclusionFramebuffer.Name = "Ambient";

           
            /*
             * Register as inspectable framebuffer
             */
            observer.RegisterFramebuffer2DResource(ambientOcclusionFramebuffer);

            /*
             * Register light exposure framebuffer
             */
            m_AmbientOcclusionBuffers.Add(ambientOcclusionFramebuffer);

            /*
             * Create observer ambient occlusion framebuffer
             */
            Framebuffer2D blurFramebuffer = new Framebuffer2D(
               Framebuffer2D.IntermediateFramebuffer.Width,
               Framebuffer2D.IntermediateFramebuffer.Height,
               new List<FramebufferAttachmentCreateParams>()
               {
                    new FramebufferAttachmentCreateParams("Ambiend Blured",TextureFormat.Red, TextureInternalFormat.R32f, TextureDataType.Float),
               },
               true
               ); ;

            blurFramebuffer.Name = "Blur";

            /*
             * Register as inspectable framebuffer
             */
            observer.RegisterFramebuffer2DResource(blurFramebuffer);

            /*
             * Register to list
             */
            m_BlurBuffers.Add(blurFramebuffer);

            /*
             * Create observer ambient occlusion framebuffer
             */
            Framebuffer2D finalColorFramebuffer = new Framebuffer2D(
               Framebuffer2D.IntermediateFramebuffer.Width,
               Framebuffer2D.IntermediateFramebuffer.Height,
               new List<FramebufferAttachmentCreateParams>()
               {
                    new FramebufferAttachmentCreateParams("Color",TextureFormat.Rgb, TextureInternalFormat.Rgb32f, TextureDataType.UnsignedByte),
               },
               true
               ); ;

            finalColorFramebuffer.Name = "Final";

            /*
             * Register as inspectable framebuffer
             */
            observer.RegisterFramebuffer2DResource(finalColorFramebuffer);

            /*
             * Register to list
             */
            m_FinalColorBuffers.Add(finalColorFramebuffer);

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
            CommandBuffer gBufferPassCommandBuffer = new CommandBuffer();
            CommandBuffer ambientOcclusionCommandBuffer = new CommandBuffer();
            CommandBuffer ambientBluredCommandBuffer = new CommandBuffer();
            CommandBuffer finalColorCommandBuffer = new CommandBuffer();

            /*
             * Start recording
             */
            gBufferPassCommandBuffer.StartRecoding();
            ambientOcclusionCommandBuffer.StartRecoding();
            ambientBluredCommandBuffer.StartRecoding();
            finalColorCommandBuffer.StartRecoding();

            /*
             * Set gbuffer pipeline state
             */
            PipelineState gBufferPipelineState = new PipelineState(new Graphics.PolygonMode(PolygonFillFace.Front, PolygonFillMethod.Fill), new CullingMode(TriangleFrontFace.CCW, CullFace.Back));
            gBufferPassCommandBuffer.SetPipelineState(gBufferPipelineState);

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
                gBufferPassCommandBuffer.SetFramebuffer(gBuffer);
                GL.DrawBuffers(3, new DrawBuffersEnum[] { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1 , DrawBuffersEnum.ColorAttachment2 });
                /*
                 * Set framebuffer viewport
                 */
                gBufferPassCommandBuffer.SetViewport(Vector2.Zero, new Vector2(gBuffer.Width, gBuffer.Height));

                /*
                 * Clear color the framebuffer
                 */
                gBufferPassCommandBuffer.ClearDepth(1.0f);
                gBufferPassCommandBuffer.ClearColor(clearColor);
                
                /*
                 * Set color shader program
                 */
                gBufferPassCommandBuffer.SetShaderProgram(m_GBufferMaterial.Program);

                /*
                 * Render for gbuffer
                 */
                for (int renderableIndex = 0; renderableIndex < m_Renderables.Count; renderableIndex++)
                {
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
                    gBufferPassCommandBuffer.SetVertexbuffer(vertexBuffer);

                    /*
                     * Set index buffer command
                     */
                    gBufferPassCommandBuffer.SetIndexBuffer(indexBuffer);

                    /*
                     * Create model matrix
                     */
                    Vector3 position = renderable.Spatial.Position.GetAsOpenTK();
                    Vector3 rotation = renderable.Spatial.Rotation.GetAsOpenTK();
                    Vector3 scale = renderable.Spatial.Scale.GetAsOpenTK();

                    /*
                     * Create model matrix matrix
                     */
                    Matrix4 modelMatrix =
                        Matrix4.CreateScale(scale) *
                        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z)) *
                        Matrix4.CreateTranslation(position);

                    /*
                     * Create model-view-projection matrix
                     */
                    Matrix4 mvp = modelMatrix * viewMatrix * projectionMatrix;

                    /*
                     * Create normal matrix
                     */
                    Matrix4 normalMatrix = modelMatrix;
                    normalMatrix.Invert();
                    normalMatrix.Transpose();

                    /*
                     * Set uniforms that are required from the gbuffer's vertex shader stage
                     */
                    gBufferPassCommandBuffer.SetUniformMat4x4(m_GBufferMaterial.Program, mvp, "v_MvpMatrix");
                    gBufferPassCommandBuffer.SetUniformMat4x4(m_GBufferMaterial.Program, modelMatrix, "v_ModelMatrix");
                    gBufferPassCommandBuffer.SetUniformMat4x4(m_GBufferMaterial.Program, normalMatrix, "v_NormalMatrix");
                    gBufferPassCommandBuffer.SetUniformMat4x4(m_GBufferMaterial.Program, viewMatrix, "v_ViewMatrix");
                    gBufferPassCommandBuffer.SetUniformMat4x4(m_GBufferMaterial.Program, projectionMatrix, "v_ProjectionMatrix");

                    /*
                     * Set color texture
                     */
                    gBufferPassCommandBuffer.SetTexture2D(m_GBufferMaterial.Program, renderable.ColorTexture, "f_DeferredColorTexture");

                    /*
                     * Draw color buffer
                     */
                    gBufferPassCommandBuffer.DrawIndexed((int)triangleCount);


                }
            }

            /*
             * End recording
             */
            gBufferPassCommandBuffer.EndRecording();

            /*
            * Set ambient occlusion pipeline state
            */
            PipelineState ambientOcclusionPipelineState = new PipelineState(new Graphics.PolygonMode(PolygonFillFace.Front, PolygonFillMethod.Fill), new CullingMode(TriangleFrontFace.CCW, CullFace.Back));
            ambientOcclusionPipelineState.DepthTest = false;
            ambientOcclusionCommandBuffer.SetPipelineState(ambientOcclusionPipelineState);
            for (int observerIndex = 0; observerIndex < m_Observers.Count; observerIndex++)
            {
                /*
                 * Get observer
                 */
                ObserverComponent observer = m_Observers[observerIndex];

                /*
                 * Get observer clear color
                 */
                Color4 clearColor = Color4.Black;

                /*
                 * Get observer gBuffer
                 */
                Framebuffer2D ambientOcclusionBuffer = m_AmbientOcclusionBuffers[observerIndex];

                /*
                 * Set color framebuffer
                 */
                ambientOcclusionCommandBuffer.SetFramebuffer(ambientOcclusionBuffer);

                /*
                 * Set framebuffer viewport
                 */
                ambientOcclusionCommandBuffer.SetViewport(Vector2.Zero, new Vector2(ambientOcclusionBuffer.Width, ambientOcclusionBuffer.Height));

                /*
                 * Clear color the framebuffer
                 */
                ambientOcclusionCommandBuffer.ClearColor(clearColor);
                ambientOcclusionCommandBuffer.ClearDepth(1.0f);

                /*
                 * Set color shader program
                 */
                ambientOcclusionCommandBuffer.SetShaderProgram(m_AmbientOcclusionMaterial.Program);

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
                ambientOcclusionCommandBuffer.SetVertexbuffer(vertexBuffer);

                /*
                 * Set index buffer command
                 */
                ambientOcclusionCommandBuffer.SetIndexBuffer(indexBuffer);

                /*
                 * Set color texture
                 */
                
                ambientOcclusionCommandBuffer.SetTexture2D(m_AmbientOcclusionMaterial.Program, m_GBuffers[observerIndex].Attachments[1].Texture, "f_NormalTexture");
                ambientOcclusionCommandBuffer.SetTexture2D(m_AmbientOcclusionMaterial.Program, m_GBuffers[observerIndex].Attachments[2].Texture, "f_PositionTexture");
                ambientOcclusionCommandBuffer.SetTexture2D(m_AmbientOcclusionMaterial.Program, m_GBuffers[observerIndex].DepthTexture, "f_DepthBuffer");
                ambientOcclusionCommandBuffer.SetUniformVector3Array(m_AmbientOcclusionMaterial.Program, m_SSAOKernels.ToArray(), "f_SSAOKernels");
                ambientOcclusionCommandBuffer.SetTexture2D(m_AmbientOcclusionMaterial.Program, m_SSAONoiseTexture, "f_NoiseTexture");
                ambientOcclusionCommandBuffer.SetUniformMat4x4(m_AmbientOcclusionMaterial.Program,observer.GetProjectionMatrix(), "f_ProjectionMatrix");


                /*
                 * Draw color buffer
                 */
                ambientOcclusionCommandBuffer.DrawIndexed((int)triangleCount);
            }

            /*
             * End recording
             */
            ambientOcclusionCommandBuffer.EndRecording();

            /*
            * Set ambient occlusion blur pipeline state
            */
            PipelineState blurPipelineState = new PipelineState(new Graphics.PolygonMode(PolygonFillFace.Front, PolygonFillMethod.Fill), new CullingMode(TriangleFrontFace.CCW, CullFace.Back));
            blurPipelineState.DepthTest = false;
            ambientBluredCommandBuffer.SetPipelineState(ambientOcclusionPipelineState);
            for (int observerIndex = 0; observerIndex < m_Observers.Count; observerIndex++)
            {
                /*
                 * Get observer
                 */
                ObserverComponent observer = m_Observers[observerIndex];

                /*
                 * Get observer clear color
                 */
                Color4 clearColor = Color4.Black;

                /*
                 * Get observer gBuffer
                 */
                Framebuffer2D blurBuffer = m_BlurBuffers[observerIndex];

                /*
                 * Set color framebuffer
                 */
                ambientBluredCommandBuffer.SetFramebuffer(blurBuffer);

                /*
                 * Set framebuffer viewport
                 */
                ambientBluredCommandBuffer.SetViewport(Vector2.Zero, new Vector2(blurBuffer.Width, blurBuffer.Height));

                /*
                 * Clear color the framebuffer
                 */
                ambientBluredCommandBuffer.ClearColor(clearColor);
                ambientBluredCommandBuffer.ClearDepth(1.0f);

                /*
                 * Set color shader program
                 */
                ambientBluredCommandBuffer.SetShaderProgram(m_BlurMaterial.Program);

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
                ambientBluredCommandBuffer.SetVertexbuffer(vertexBuffer);

                /*
                 * Set index buffer command
                 */
                ambientBluredCommandBuffer.SetIndexBuffer(indexBuffer);

                /*
                 * Set color texture
                 */
                ambientBluredCommandBuffer.SetTexture2D(m_BlurMaterial.Program, m_AmbientOcclusionBuffers[observerIndex].Attachments[0].Texture, "f_AmbientOcclusionTexture");


                /*
                 * Draw color buffer
                 */
                ambientBluredCommandBuffer.DrawIndexed((int)triangleCount);
            }

            /*
             * End recording
             */
            ambientBluredCommandBuffer.EndRecording();

            /*
           * Set ambient occlusion blur pipeline state
           */
            PipelineState finalColorPipelineState = new PipelineState(new Graphics.PolygonMode(PolygonFillFace.Front, PolygonFillMethod.Fill), new CullingMode(TriangleFrontFace.CCW, CullFace.Back));
            finalColorPipelineState.DepthTest = false;
            finalColorCommandBuffer.SetPipelineState(finalColorPipelineState);
            for (int observerIndex = 0; observerIndex < m_Observers.Count; observerIndex++)
            {
                /*
                 * Get observer
                 */
                ObserverComponent observer = m_Observers[observerIndex];

                /*
                 * Get observer clear color
                 */
                Color4 clearColor = Color4.Black;

                /*
                 * Get observer gBuffer
                 */
                Framebuffer2D finalColorBuffer = m_FinalColorBuffers[observerIndex];

                /*
                 * Set color framebuffer
                 */
                finalColorCommandBuffer.SetFramebuffer(finalColorBuffer);

                /*
                 * Set framebuffer viewport
                 */
                finalColorCommandBuffer.SetViewport(Vector2.Zero, new Vector2(finalColorBuffer.Width, finalColorBuffer.Height));

                /*
                 * Clear color the framebuffer
                 */
                finalColorCommandBuffer.ClearColor(clearColor);
                finalColorCommandBuffer.ClearDepth(1.0f);

                /*
                 * Set color shader program
                 */
                finalColorCommandBuffer.SetShaderProgram(m_FinalColorMaterial.Program);

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
                finalColorCommandBuffer.SetVertexbuffer(vertexBuffer);

                /*
                 * Set index buffer command
                 */
                finalColorCommandBuffer.SetIndexBuffer(indexBuffer);

                /*
                 * Set color texture
                 */
                finalColorCommandBuffer.SetTexture2D(m_FinalColorMaterial.Program, m_BlurBuffers[observerIndex].Attachments[0].Texture, "f_AmbientOcclusionTexture");
                finalColorCommandBuffer.SetTexture2D(m_FinalColorMaterial.Program, m_GBuffers[observerIndex].Attachments[0].Texture, "f_ColorTexture");
                finalColorCommandBuffer.SetTexture2D(m_FinalColorMaterial.Program, m_GBuffers[observerIndex].Attachments[1].Texture, "f_NormalTexture");
                finalColorCommandBuffer.SetTexture2D(m_FinalColorMaterial.Program,m_GBuffers[observerIndex].Attachments[2].Texture, "f_ViewSpacePositionTexture");
                finalColorCommandBuffer.SetUniformMat4x4(m_FinalColorMaterial.Program, observer.GetProjectionMatrix(), "f_ProjectionMatrix");
                finalColorCommandBuffer.SetUniformVector4(m_FinalColorMaterial.Program, new Vector4(finalColorBuffer.Width, finalColorBuffer.Height, 0, 0),"f_TextureSize");
                finalColorCommandBuffer.SetCubeTexture(m_FinalColorMaterial.Program, m_CubeTexture, "f_CubeTexture");

                /*
                 * Draw color buffer
                 */
                finalColorCommandBuffer.DrawIndexed((int)triangleCount);
            }

            /*
             * Finalize recording
             */
            finalColorCommandBuffer.EndRecording();

            /*
             * Execute command buffer
             */
            gBufferPassCommandBuffer.Execute();

            /*
             * Execute command buffer
             */
            ambientOcclusionCommandBuffer.Execute();

            /*
             * Execute command buffer
             */
            ambientBluredCommandBuffer.Execute();

            /*
             * Execute command buffer
             */
            finalColorCommandBuffer.Execute();
        }

        private List<ObserverComponent> m_Observers;
        private List<Framebuffer2D> m_GBuffers;
        private List<Framebuffer2D> m_AmbientOcclusionBuffers;
        private List<Framebuffer2D> m_BlurBuffers;
        private List<Framebuffer2D> m_FinalColorBuffers;
        private List<ForwardMeshRenderable> m_Renderables;
        private List<DeferredPointLight> m_PointLights;
        private List<DeferredInstancedMesh> m_InstancedMeshes;
        private List<Vector3> m_SSAOKernels;
        private Material m_GBufferMaterial;
        private Material m_AmbientOcclusionMaterial;
        private Material m_BlurMaterial;
        private Material m_FinalColorMaterial;
        private StaticMesh m_ScreenQuad;
        private Texture2D m_SSAONoiseTexture;
        private CubeTexture m_CubeTexture;
    }
}
