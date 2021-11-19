using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Vex.Framework;

namespace Vex.Graphics
{
    /// <summary>
    /// Shader class for all shader operations
    /// </summary>
    public sealed class Shader : VexObject
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

        public void Compile(in string source)
        {
            CompileAs(m_Type, source);
        }

        /// <summary>
        /// Creates a new shader
        /// </summary>
        /// <param name="type"></param>
        /// <param name="source"></param>
        private void CompileAs(ShaderStage type,string source)
        {
            switch (type)
            {
                case ShaderStage.Vertex:
                    CreateAsVertexShader(source);
                    break;
                case ShaderStage.Fragment:
                    CreateAsFragmentShader(source);
                    break;
                case ShaderStage.Geometry:
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
            Console.WriteLine("Compiled as vertex shader");
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
        /// Creates this shader as fragment shader
        /// </summary>
        /// <param name="source"></param>
        private void CreateAsFragmentShader(string source)
        {
            Console.WriteLine("Compiled as fragment shader");
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

        private ShaderStage m_Type;
        private string m_Source;
        private string m_LastErrorMessage;
        private int m_Handle;
        private bool m_Compiled;
    }
}
