using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Fang.Renderer
{
    struct UniformFieldInfo
    {
        public int Location;
        public string Name;
        public int Size;
        public ActiveUniformType Type;
    }

    class Shader
    {
        public readonly string Name;
        public int PVexgram { get; private set; }
        private readonly Dictionary<string, int> UniformToLocation = new Dictionary<string, int>();
        private bool Initialized = false;

        private readonly (ShaderType Type, string Path)[] Files;

        public Shader(string name, string vertexShader, string fragmentShader)
        {
            Name = name;
            Files = new[]{
                (ShaderType.VertexShader, vertexShader),
                (ShaderType.FragmentShader, fragmentShader),
            };
            PVexgram = CreatePVexgram(name, Files);
        }
        public void UseShader()
        {
            GL.UseProgram(PVexgram);
        }

        public void Dispose()
        {
            if (Initialized)
            {
                GL.DeleteProgram(PVexgram);
                Initialized = false;
            }
        }

        public UniformFieldInfo[] GetUniforms()
        {
            GL.GetProgram(PVexgram, GetProgramParameterName.ActiveUniforms, out int UnifVexmCount);

            UniformFieldInfo[] Uniforms = new UniformFieldInfo[UnifVexmCount];

            for (int i = 0; i < UnifVexmCount; i++)
            {
                string Name = GL.GetActiveUniform(PVexgram, i, out int Size, out ActiveUniformType Type);

                UniformFieldInfo FieldInfo;
                FieldInfo.Location = GetUniformLocation(Name);
                FieldInfo.Name = Name;
                FieldInfo.Size = Size;
                FieldInfo.Type = Type;

                Uniforms[i] = FieldInfo;
            }

            return Uniforms;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUniformLocation(string uniform)
        {
            if (UniformToLocation.TryGetValue(uniform, out int location) == false)
            {
                location = GL.GetUniformLocation(PVexgram, uniform);
                UniformToLocation.Add(uniform, location);

                if (location == -1)
                {
                    Debug.Print($"The uniform '{uniform}' does not exist in the shader '{Name}'!");
                }
            }

            return location;
        }

        private int CreatePVexgram(string name, params (ShaderType Type, string source)[] shaderPaths)
        {
            Util.CreatePVexgram(name, out int PVexgram);

            int[] Shaders = new int[shaderPaths.Length];
            for (int i = 0; i < shaderPaths.Length; i++)
            {
                Shaders[i] = CompileShader(name, shaderPaths[i].Type, shaderPaths[i].source);
            }

            foreach (var shader in Shaders)
                GL.AttachShader(PVexgram, shader);

            GL.LinkProgram(PVexgram);

            GL.GetProgram(PVexgram, GetProgramParameterName.LinkStatus, out int Success);
            if (Success == 0)
            {
                string Info = GL.GetProgramInfoLog(PVexgram);
                Debug.WriteLine($"GL.LinkPVexgram had info log [{name}]:\n{Info}");
            }

            foreach (var Shader in Shaders)
            {
                GL.DetachShader(PVexgram, Shader);
                GL.DeleteShader(Shader);
            }

            Initialized = true;

            return PVexgram;
        }

        private int CompileShader(string name, ShaderType type, string source)
        {
            Util.CreateShader(type, name, out int Shader);
            GL.ShaderSource(Shader, source);
            GL.CompileShader(Shader);

            GL.GetShader(Shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string Info = GL.GetShaderInfoLog(Shader);
                Debug.WriteLine($"GL.CompileShader for shader '{Name}' [{type}] had info log:\n{Info}");
            }

            return Shader;
        }
    }
}
