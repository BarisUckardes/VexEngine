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
    public sealed class ShaderProgram : VexObject
    {
        public ShaderProgram(in string category,in string categoryName,params Shader[] shaders)
        {
            /*
             * First create a pVexgram
             */
            int handle = GL.CreateProgram();

            /*
             * Attach shaders
             */
            for(int i=0;i<shaders.Length;i++)
            {
                GL.AttachShader(handle, shaders[i].Handle);
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
            if(programLinkStatus == 0)
            {
                /*
                 * Get log
                 */
                string erVexrLog;
                GL.GetProgramInfoLog(handle,out erVexrLog);

                /*
                 * Delete pVexgram
                 */
                GL.DeleteProgram(handle);

                /*
                 * Debug log
                 */
                Console.WriteLine("PVexgram link failed!");

                return;
            }

            m_Handle = handle;
            m_Shaders = shaders;
            m_Category = category;
            m_CategoryName = categoryName;
        }
        public Shader[] Shaders
        {
            get
            {
                return m_Shaders.ToArray();
            }
        }
        public string Category
        {
            get
            {
                return m_Category;
            }
        }
        
        public string CategoryName
        {
            get
            {
                return m_CategoryName;
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
        /// Returns the shader with the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Shader GetShader(ShaderStage type)
        {
            for(int i=0;i<m_Shaders.Length;i++)
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

            List<string> shaderSources = new List<string>(m_Shaders.Length);
            List<ShaderStage> shaderStages = new List<ShaderStage>(m_Shaders.Length);

            /*
             * Get shader source and type
             */
            for(int i=0;i<m_Shaders.Length;i++)
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
                    metaDatas.Add(new ShaderParameterMetaData(parameterName, GL.GetUniformLocation(m_Handle, parameterName), parameterType));

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

        private Shader[] m_Shaders;
        private int m_Handle;
        private string m_Category;
        private string m_CategoryName;
    }
}
