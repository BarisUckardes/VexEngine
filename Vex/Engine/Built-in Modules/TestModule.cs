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
using Vex.Extensions;

namespace Vex.Engine
{
    public sealed class TestModule : EngineModule
    {
        public override void OnAttach()
        {
            World world = new World(Session,new WorldSettings(typeof(DefaultLogicResolver),new List<Type>() {typeof(ForwardGraphicsResolver)}));
            world.Name = "My Test World";
            world.AddView<WorldGraphicsView>();
            world.AddView<WorldLogicView>();
            world.Register();

            Entity observerEntity = new Entity("Forward observer entity", world);
            ForwardMeshObserver forwardMeshObserver = observerEntity.AddComponent<ForwardMeshObserver>();
            forwardMeshObserver.ClearColor = OpenTK.Mathematics.Color4.Crimson;
            observerEntity.Spatial.Position = new OpenTK.Mathematics.Vector3(0, 0, 0).GetAsNumerics();
            string vertexSource  = @"
#version 450

layout(location = 0) in vec3 v_Position;
layout(location = 0) in vec3 v_Normal;
layout(location = 1) in vec2 v_Uv;

out vec2 f_Uv;

uniform mat4 v_Mvp;


void main()
{
    gl_Position = v_Mvp*vec4(v_Position, 1);
    f_Uv = v_Uv;
}";

            string fragmentSource = @"
#version 450

layout(location = 0) out vec4 f_ColorOut;
in vec2 f_Uv;


uniform sampler2D f_SpriteTexture;
void main()
{
f_ColorOut = texture(f_SpriteTexture,f_Uv);
}";

            Shader vertexShader = new Shader(ShaderStage.Vertex);
            Shader fragmenShader = new Shader(ShaderStage.Fragment);
            vertexShader.Compile(vertexSource);
            fragmenShader.Compile(fragmentSource);
            ShaderProgram shaderProgram = new ShaderProgram("Forward","Unlit");
            shaderProgram.LinkProgram(new List<Shader>() { vertexShader,fragmenShader});

            Material material = new Material(shaderProgram);
           
            Entity renderableEntity = new Entity("Forward entity", world);
            ForwardMeshRenderable renderableComponent = renderableEntity.AddComponent<ForwardMeshRenderable>();
            renderableComponent.Material = material;
            renderableEntity.Spatial.Position = new Vector3(0, 0, 1).GetAsNumerics();
            renderableEntity.Spatial.Scale = new Vector3(5, 5, 5).GetAsNumerics();
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
