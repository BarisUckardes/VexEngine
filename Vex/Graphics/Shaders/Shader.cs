using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
namespace Vex.Graphics
{
    /// <summary>
    /// Shader class for all shader operations
    /// </summary>
    public sealed class Shader
    {
        public Shader(ShaderStage type,string source)
        {
            /*
             * Create shader
             */
            CreateShader(type, source);
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
        /// Creates a new shader
        /// </summary>
        /// <param name="type"></param>
        /// <param name="source"></param>
        private void CreateShader(ShaderStage type,string source)
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
                return;
            }

            m_Handle = vertexShaderHandle;
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
                return;
            }

            m_Handle = vertexShaderHandle;
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
                string erVexrLog;
                GL.GetShaderInfoLog(shaderHandle, out erVexrLog);

                /*
                 * Delete the shader
                 */
                GL.DeleteShader(shaderHandle);

                /*
                 * Log
                 */
                Console.WriteLine("Shader Compile ErVexr: " + erVexrLog);

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
        private int m_Handle;
    }
}
