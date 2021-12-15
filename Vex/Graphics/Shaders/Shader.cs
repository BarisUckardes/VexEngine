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
             * Set source
             */
            m_Source = source;
            m_Compiled = false;
            m_LastErrorMessage = "";

            /*
             * Compile
             */
            Invalidate();

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
        private void Invalidate()
        {
            /*
             * Delete former shader
             */
            GL.DeleteShader(m_Handle);

            /*
             * Create shader
             */
            int vertexShaderHandle = GL.CreateShader((OpenTK.Graphics.OpenGL4.ShaderType)m_Type);

            /*
             * Set shader source
             */
            GL.ShaderSource(vertexShaderHandle, m_Source);

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
