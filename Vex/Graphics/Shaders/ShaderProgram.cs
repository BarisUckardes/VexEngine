using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Vex.Engine;
using Vex.Framework;

namespace Vex.Graphics
{
    /// <summary>
    /// Shader pVexgram class
    /// </summary>
    public sealed class ShaderProgram : AssetObject
    {
        public ShaderProgram(in string category,in string categoryName)
        {
            m_Handle = 0;
            m_Category = category;
            m_CategoryName = categoryName;
            m_Shaders = new List<Shader>();
            m_LastLinkErrorMessage = string.Empty;
            Name = "Shader Program";
        }
       
        /// <summary>
        /// Returns the shader set of this program
        /// </summary>
        public List<Shader> Shaders
        {
            get
            {
                return m_Shaders;
            }
        }

        /// <summary>
        /// Returns the category of this program
        /// </summary>
        public string Category
        {
            get
            {
                return m_Category;
            }
        }
        
        /// <summary>
        /// Returns the category name of this program
        /// </summary>
        public string CategoryName
        {
            get
            {
                return m_CategoryName;
            }
        }
        public string LastErrorMessage
        {
            get
            {
                return m_LastLinkErrorMessage;
            }
        }
        /// <summary>
        /// Returns the grapics handle
        /// </summary>
        public int Handle
        {
            get
            {
                return m_Handle;
            }
        }


        /// <summary>
        /// Links the program againts the given shader
        /// </summary>
        /// <param name="shaders"></param>
        public void LinkProgram(List<Shader> shaders)
        {
            /*
             * Clear former shader events
             */
            foreach (Shader shader in m_Shaders)
            {
                shader.RemoveOnShaderCompileDelegate(OnShaderCompiled);
            }

            /*
             * Reset error state
             */
            m_LastLinkErrorMessage = string.Empty;

            /*
             * Set shaders
             */
            m_Shaders = new List<Shader>(shaders);

            /*
             * Register event invokes
             */
            foreach(Shader shader in m_Shaders)
            {
                shader.RegisterOnShaderCompileDelegate(OnShaderCompiled);
            }

            /*
             * Invalidate
             */
            Invalidate();
        }
        /// <summary>
        /// Returns the shader with the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Shader GetShader(ShaderStage type)
        {
            for(int i=0;i<m_Shaders.Count; i++)
            {
                if(m_Shaders[i].Type == type)
                {
                    return m_Shaders[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the uniform parameter meta datas
        /// </summary>
        /// <returns></returns>
        public ShaderStageParameters[] GetPVexgramParameters()
        {
            List<ShaderStageParameters> stageParameters = new List<ShaderStageParameters>();

            List<string> shaderSources = new List<string>(m_Shaders.Count);
            List<ShaderStage> shaderStages = new List<ShaderStage>(m_Shaders.Count);

            /*
             * Get shader source and type
             */
            for(int i=0;i<m_Shaders.Count;i++)
            {
                shaderSources.Add(m_Shaders[i].Source);
                shaderStages.Add(m_Shaders[i].Type);
            }

            /*
             * Get shader source parameters
             */
            for(int i=0;i<shaderSources.Count;i++)
            {
                /*
                 * Create cahche lists
                 */
                List<string> foundParameters = new List<string>();
                List<ShaderParameterType> foundTypes = new List<ShaderParameterType>();
                List<ShaderParameterMetaData> metaDatas = new List<ShaderParameterMetaData>();

                /*
                 * Find parameters
                 */
                List<int> foundParameterUniformLocation = new List<int>();
                int parameterStartLocation = shaderSources[i].IndexOf("uniform",0);

                while(parameterStartLocation != -1)
                {
                    /*
                     * Add found uniform
                     */
                    foundParameterUniformLocation.Add(parameterStartLocation);

                    /*
                     * Get parameter information
                     */
                    ShaderParameterType parameterType;
                    string parameterName = String.Empty;
                    GetShaderParameter(shaderSources[i], parameterStartLocation, out parameterType, out parameterName);

                    /*
                     * Register parameters to cache lists
                     */
                    foundParameters.Add(parameterName);
                    foundTypes.Add(parameterType);

                    /*
                     *  Create shader meta data
                     */
                    // metaDatas.Add(new ShaderParameterMetaData(parameterName, GL.GetUniformLocation(m_Handle, parameterName), parameterType));
                    metaDatas.Add(new ShaderParameterMetaData(parameterName, parameterType));

                    /*
                     * Search for next parameter
                     */
                    parameterStartLocation = shaderSources[i].IndexOf("uniform", parameterStartLocation + 1);

                }

                /*
                 * Create stage parameters
                 */
                ShaderStageParameters shaderStageParameters = new ShaderStageParameters(shaderStages[i], metaDatas);

                /*
                 * Add new shader stage parameters
                 */
                stageParameters.Add(shaderStageParameters);

            }
            return stageParameters.ToArray();
        }

        private void Invalidate()
        {
            /*
             * Validate and delete program handle
             */
            if (m_Handle != 0)
                GL.DeleteProgram(m_Handle);

            /*
             * First create a pVexgram
             */
            int handle = GL.CreateProgram();

            /*
             * Attach shaders
             */
            for (int i = 0; i < m_Shaders.Count; i++)
            {
                GL.AttachShader(handle, m_Shaders[i].Handle);
            }

            /*
             * Link program
             */
            GL.LinkProgram(handle);

            /*
             * Check pVexgram link 
             */
            int programLinkStatus = 0;
            GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out programLinkStatus);
            if (programLinkStatus == 0)
            {
                /*
                 * Get log
                 */
                string errorLog;
                GL.GetProgramInfoLog(handle, out errorLog);

                /*
                 * Delete pVexgram
                 */
                GL.DeleteProgram(handle);

                /*
                 * Debug log
                 */
                Console.WriteLine("Program link failed!");
                m_LastLinkErrorMessage = errorLog;
            }

            /*
             * Set handle
             */
            m_Handle = handle;
        }
        private void GetShaderParameter(string source,int parameterStartIndex,out ShaderParameterType type,out string parameterName)
        {
            /*
             * Get parameter type blanks
             */
            int blank0Index = source.IndexOf(" ", parameterStartIndex);
            int blank1Index = source.IndexOf(" ", blank0Index + 1);

            /*
             * get parameter name ending
             */
            int endIndex = source.IndexOf(";", blank1Index + 1);

            /*
             * Get parameter name and type text substring
             */
            string parameterTypeText = source.Substring(blank0Index+1, blank1Index-blank0Index-1);
            string parameterNameText = source.Substring(blank1Index+ 1, endIndex - blank1Index-1);

            /*
             * Set parameter type via string
             */
            type = ShaderParameterTypeUtils.GetTypeViaString(parameterTypeText);

            /*
             * Set parameter name
             */
            parameterName = parameterNameText;
        }

        private void OnShaderCompiled()
        {
            Invalidate();
        }
        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        private List<Shader> m_Shaders;
        private int m_Handle;
        private string m_Category;
        private string m_CategoryName;
        private string m_LastLinkErrorMessage;
    }
}
