﻿using Vex.Engine;
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
    public sealed class Material : VexObject
    {
        public Material(ShaderProgram program = null)
        {
            /*
             * Initialize local list
             */
            m_StageParameters = new List<MaterialStageParameters>();

            /*
             * Create parameters
             */
            if(program != null)
                CreateParametersFromProgram(program);

            /*
             * Set program
             */
            m_Program = program;

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
                 * Clear former parameters
                 */
                m_StageParameters.Clear();

                /*
                 * Create parameters
                 */
                if (value != null)
                    CreateParametersFromProgram(value);

                /*
                 * Set program
                 */
                m_Program = value;
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
      
        /// <summary>
        /// Creates the shader stage parameters via shader pVexgram
        /// </summary>
        /// <param name="pVexgram"></param>
        private void CreateParametersFromProgram(ShaderProgram pVexgram)
        {
            ///*
            // * Parameter meta datas
            // */
            ShaderStageParameters[] parameters = pVexgram.GetPVexgramParameters();

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


        private ShaderProgram m_Program;
        private List<MaterialStageParameters> m_StageParameters;
    }
}
