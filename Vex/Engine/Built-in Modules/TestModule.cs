using OpenTK.Mathematics;
using Vex.Asset;
using Vex.Framework;
using Vex.Graphics;
using Vex.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Engine
{
    public sealed class TestModule : EngineModule
    {
        public override void OnAttach()
        {
            World world = new World(Session);
            world.AddView<WorldGraphicsView>();
            world.AddView<WorldLogicView>();
            world.Register();

            Entity observerEntity = new Entity("Sprite observer entity", world);
            SpriteObserver spriteObserverComponent = observerEntity.AddComponent<SpriteObserver>();
            spriteObserverComponent.ClearColor = OpenTK.Mathematics.Color4.CornflowerBlue;
            observerEntity.Spatial.Position = new OpenTK.Mathematics.Vector3(0, 0, 0);
            string vertexSource  = @"
#version 450

layout(location = 0) in vec2 v_Position;
layout(location = 1) in vec2 v_Uv;

out vec2 f_Uv;

uniform mat4 v_Mvp;
uniform vec4 v_TestVec;
uniform sampler2D v_TestTexture;
uniform float v_TestFloat;

void main()
{
    gl_Position = v_Mvp*vec4(v_Position,0, 1);
    f_Uv = v_Uv;
}";

            string fragmentSource = @"
#version 450

layout(location = 0) out vec4 f_ColoVexut;
in vec2 f_Uv;


uniform sampler2D f_SpriteTexture;
void main()
{
f_ColoVexut = vec4(texture(f_SpriteTexture,f_Uv).rgb,1.0f);
}";

            Shader vertexShader = new Shader(ShaderStage.Vertex, vertexSource);
            Shader fragmenShader = new Shader(ShaderStage.Fragment, fragmentSource);
            ShaderProgram pVexgram = new ShaderProgram(vertexShader, fragmenShader);
            Material material = new Material("Unlit","Sprite",pVexgram);

            SpriteMesh mesh = new SpriteMesh();
            SpriteVertex[] vertexes = {
            new SpriteVertex(-0.5f, -0.5f,1.0f,1.0f), //Bottom-left vertex
            new SpriteVertex( 0.5f, -0.5f,1.0f,0.0f), //Bottom-right vertex
            new SpriteVertex(0.0f,  0.5f,0.0f,1.0f)}; //Top vertex
            int[] triangles = new int[] { 0, 1, 2};

            mesh.SetVertexData(vertexes);
            mesh.SetTriangleData(triangles);

            Entity spriteEnttiy = new Entity("Sprite entity", world);
            SpriteRenderable spriteRenderable = spriteEnttiy.AddComponent<SpriteRenderable>();
            spriteRenderable.Mesh = mesh;
            spriteRenderable.Material = material;
            spriteEnttiy.Spatial.Position = new Vector3(0, 0, 1);
            spriteEnttiy.Spatial.Scale = new Vector3(5, 5, 5);

            Texture2D sprite = Texture2D.LoadTextureFromPath(@"C:\Users\PC\Desktop\Test Domain\test.jpg");
            spriteRenderable.SpriteTexture = sprite;
        }

        public override void OnDetach()
        {

        }

        public override void OnUpdate()
        {

        }

        public override void OnEvent(PlatformEvent eventData)
        {

        }
    }
}
