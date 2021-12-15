using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Vex.Framework;

namespace Vex.Graphics
{
    public delegate void OnShaderCompiled();
    /// <summary>
    /// Shader class for all shader operations
    /// </summary>
    public sealed class Shader : AssetObject
    {
        public Shader(ShaderStage type)
        {
            /*
             * Set variables
             */
            m_Source = string.Empty;
            m_LastErrorMessage = string.Empty;
            m_Type = type;
            m_Handle = 0;
            Name = type.ToString() + " Shader";
        }

        /// <summary>
        /// Returns the gpu handle
        /// </summary>
        public int Handle
        {
            get
            {
                return m_Handle;
            }
        }

        /// <summary>
        /// Returns the shader source as string
        /// </summary>
        public string Source
        {
            get
            {
                return m_Source;
            }

        }

        public string LastErrorMessage
        {
            get
            {
                return m_LastErrorMessage;
            }
        }

        /// <summary>
        /// Returns the shader type
        /// </summary>
        public ShaderStage Type
        {
            get
            {
                return m_Type;
            }
        }

        /// <summary>
        /// Returns whether this shader is compiled or not
        /// </summary>
        public bool IsCompiled
        {
            get
            {
                return m_Compiled;
            }
        }

        /// <summary>
        /// Compiles shader with the given string source
        /// </summary>
        /// <param name="source"></param>
        public void Compile(in string source)
        {
            /*
             * Compile
             */
            CompileAs(m_Type, source);

            /*
             * Broadcast
             */
            m_OnShaderCompiledEvent?.Invoke();
        }

        /// <summary>
        /// Registers a delegate to the on shader compiled event
        /// </summary>
        /// <param name="targetDelegate"></param>
        public void RegisterOnShaderCompileDelegate(OnShaderCompiled targetDelegate)
        {
            m_OnShaderCompiledEvent += targetDelegate;
        }

        /// <summary>
        /// Removes a delegates from the on shader compiled event
        /// </summary>
        /// <param name="targetDelegate"></param>
        public void RemoveOnShaderCompileDelegate(OnShaderCompiled targetDelegate)
        {
            m_OnShaderCompiledEvent -= targetDelegate;
        }
        /// <summary>
        /// Creates a new shader
        /// </summary>
        /// <param name="type"></param>
        /// <param name="source"></param>
        private void CompileAs(ShaderStage type,string source)
        {
            bool isSucess = false;
            switch (type)
            {
                case ShaderStage.Vertex:
                    CreateAsVertexShader(source);
                    break;
                case ShaderStage.Fragment:
                    CreateAsFragmentShader(source);
                    break;
                case ShaderStage.Geometry:
                    CreateAsGeometryShader(source);
                    break;
            }

            /*
             * Set source
             */
            m_Source = source;
            m_Type = type;
        }

        /// <summary>
        /// Creates this shader as vertex shader
        /// </summary>
        /// <param name="source"></param>
        private void CreateAsVertexShader(string source)
        {
            /*
             * Create shader
             */
            int vertexShaderHandle = GL.CreateShader(OpenTK.Graphics.OpenGL4.ShaderType.VertexShader);

            /*
             * Set shader source
             */
            GL.ShaderSource(vertexShaderHandle, source);

            /*
             * Compile shader
             */
            GL.CompileShader(vertexShaderHandle);

            /*
             * Check compile status
             */
            bool isCompileSuccessful = CompilationCheck(vertexShaderHandle);

            if(!isCompileSuccessful)
            {
                m_Handle = 0;
                m_Compiled = false;
                return;
            }

            m_Handle = vertexShaderHandle;
            m_Compiled = true;
        }

        /// <summary>
        /// Creates this shader as vertex shader
        /// </summary>
        /// <param name="source"></param>
        private void CreateAsGeometryShader(string source)
        {
            /*
             * Create shader
             */
            int vertexShaderHandle = GL.CreateShader(OpenTK.Graphics.OpenGL4.ShaderType.GeometryShader);

            /*
             * Set shader source
             */
            GL.ShaderSource(vertexShaderHandle, source);

            /*
             * Compile shader
             */
            GL.CompileShader(vertexShaderHandle);

            /*
             * Check compile status
             */
            bool isCompileSuccessful = CompilationCheck(vertexShaderHandle);

            if (!isCompileSuccessful)
            {
                m_Handle = 0;
                m_Compiled = false;
                return;
            }

            m_Handle = vertexShaderHandle;
            m_Compiled = true;
        }

        /// <summary>
        /// Creates this shader as fragment shader
        /// </summary>
        /// <param name="source"></param>
        private void CreateAsFragmentShader(string source)
        {
            /*
            * Create shader
            */
            int vertexShaderHandle = GL.CreateShader(OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader);

            /*
             * Set shader source
             */
            GL.ShaderSource(vertexShaderHandle, source);

            /*
             * Compile shader
             */
            GL.CompileShader(vertexShaderHandle);

            /*
             * Check compile status
             */
            bool isCompileSuccessful = CompilationCheck(vertexShaderHandle);

            if (!isCompileSuccessful)
            {
                m_Handle = 0;
                m_Compiled = false;
                return;
            }

            m_Handle = vertexShaderHandle;
            m_Compiled = true;
        }

        /// <summary>
        /// Checks shader compile status
        /// </summary>
        /// <param name="shaderHandle"></param>
        /// <returns></returns>
        private bool CompilationCheck(int shaderHandle)
        {
            /*
            * Check compilation
            */
            int compileStatus = 0;
            GL.GetShader(shaderHandle, ShaderParameter.CompileStatus, out compileStatus);
            if (compileStatus == 0)
            {
                /*
                 * Get erVexr log
                 */
                string errorLog;
                GL.GetShaderInfoLog(shaderHandle, out errorLog);
                m_LastErrorMessage = errorLog;
                /*
                 * Delete the shader
                 */
                GL.DeleteShader(shaderHandle);

                /*
                 * Log
                 */
                Console.WriteLine("Shader Compile Error: " + errorLog);

                return false;
            }
            m_LastErrorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Validates and deletes the gpu handles
        /// </summary>
        private void ValidateAndDeleteHandles()
        {
            if(m_Handle != 0)
            {
                GL.DeleteShader(m_Handle);
                m_Handle = 0;
            }
        }

        public override void Destroy()
        {
            m_Compiled = false;
            GL.DeleteShader(m_Handle);
        }

        private event OnShaderCompiled m_OnShaderCompiledEvent;
        private ShaderStage m_Type;
        private string m_Source;
        private string m_LastErrorMessage;
        private int m_Handle;
        private bool m_Compiled;
    }
}
