using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;

namespace Vex.Graphics
{
    /// <summary>
    /// Material class for encapsualting shaders and their parameters
    /// </summary>
    public sealed class Material : AssetObject
    {
        public Material(ShaderProgram program = null)
        {
            /*
             * Initialize local list
             */
            m_StageParameters = new List<MaterialStageParameters>();

            /*
             * Set program
             */
            m_Program = program;
            Name = "Material";

            /*
             * Invalidate this material
             */
            Invalidate();
        }

        /// <summary>
        /// Returns and sets material shader program
        /// </summary>
        public ShaderProgram Program
        {
            get
            {
                return m_Program;
            }
            set
            {
                /*
                 * Remove delegate
                 */
                m_Program?.RemoveProgramLinkedDelegate(OnShaderProgramLinked);

                /*
                 * Set program
                 */
                m_Program = value;

                /*
                 * Invalidate
                 */
                Invalidate();
            }
        }

        /// <summary>
        /// Returns the shader stage parameters 
        /// </summary>
        public MaterialStageParameters[] StageParameters
        {
            get
            {
                return m_StageParameters.ToArray();
            }
        }


        /// <summary>
        /// Gets a specific material stage via shader stage
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        public MaterialStageParameters GetStageParameters(ShaderStage stage)
        {
            for (int i = 0; i < m_StageParameters.Count;i++)
                if(m_StageParameters[i].Stage == stage)
                return m_StageParameters[i];

            return null;
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }


        private void Invalidate()
        {
            /*
             * Clear former delegate
             */
            m_Program?.RegisterProgramLinkedDelegate(OnShaderProgramLinked);

            /*
             * Clear former parameters
             */
            m_StageParameters.Clear();

            /*
             * Create parameters
             */
            if (m_Program != null)
                CreateParametersFromProgram(m_Program);
        }
        /// <summary>
        /// Creates the shader stage parameters via shader pVexgram
        /// </summary>
        /// <param name="pVexgram"></param>
        private void CreateParametersFromProgram(ShaderProgram program)
        {
            ///*
            // * Parameter meta datas
            // */
            ShaderStageParameters[] parameters = program.GetProgramParameters();

            m_StageParameters = GetStageParameters(parameters);
        }

        /// <summary>
        /// Returns the material shader stages via stage parameters
        /// </summary>
        /// <param name="stageParameters"></param>
        /// <returns></returns>
        private List<MaterialStageParameters> GetStageParameters(ShaderStageParameters[] stageParameters)
        {
            List<MaterialStageParameters> stageParams = new List<MaterialStageParameters>();

            for(int i=0;i<stageParameters.Length;i++)
            {
                stageParams.Add(new MaterialStageParameters(stageParameters[i]));
            }

            return stageParams;
        }

        private void OnShaderProgramLinked()
        {
            Invalidate();
        }
        private ShaderProgram m_Program;
        private List<MaterialStageParameters> m_StageParameters;
    }
}
