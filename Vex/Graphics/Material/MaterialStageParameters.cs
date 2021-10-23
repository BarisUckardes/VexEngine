using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Encapsulates all the parameters of a shader stage
    /// </summary>
    public class MaterialStageParameters
    {

        public MaterialStageParameters(in ShaderStageParameters stageParameters)
        {
            /*
             * Set stage
             */
            m_Stage = stageParameters.Stage;

            /*
             * Initialize local lists
             */
            m_FloatParameters = new List<MaterialParameterField<float>>();
            m_Vector4Parameters = new List<MaterialParameterField<Vector4>>();
            m_Texture2DParameters = new List<MaterialParameterField<Texture2D>>();
            m_Matrix4x4Parameters = new List<MaterialParameterField<Matrix4>>();

            /*
             * Get parameters
             */
            for (int i = 0; i < stageParameters.Parameters.Length; i++)
            {
                switch (stageParameters.Parameters[i].Type)
                {
                    case ShaderParameterType.Float:
                        m_FloatParameters.Add(new MaterialParameterField<float>(stageParameters.Parameters[i].Name, 0.0f, stageParameters.Parameters[i].Handle));
                        break;
                    case ShaderParameterType.Vector2:
                        break;
                    case ShaderParameterType.Vector3:
                        break;
                    case ShaderParameterType.Vector4:
                        m_Vector4Parameters.Add(new MaterialParameterField<Vector4>(stageParameters.Parameters[i].Name, Vector4.Zero, stageParameters.Parameters[i].Handle));
                        break;
                    case ShaderParameterType.Color:
                        break;
                    case ShaderParameterType.Matrix3x3:
                        break;
                    case ShaderParameterType.Matrix4x4:
                        m_Matrix4x4Parameters.Add(new MaterialParameterField<Matrix4>(stageParameters.Parameters[i].Name, Matrix4.Identity, stageParameters.Parameters[i].Handle));
                        break;
                    case ShaderParameterType.Texture2D:
                        m_Texture2DParameters.Add(new MaterialParameterField<Texture2D>(stageParameters.Parameters[i].Name, null, stageParameters.Parameters[i].Handle));
                        break;
                }
            }
        }

        /// <summary>
        /// Returns the stage of this parameter block
        /// </summary>
        public ShaderStage Stage
        {
            get
            {
                return m_Stage;
            }
        }


        /// <summary>
        /// Sets a float parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public void SetFloatParameter(string parameterName, float value)
        {
            for (int i = 0; i < m_FloatParameters.Count; i++)
            {
                if (m_FloatParameters[i].Name == parameterName)
                {
                    m_FloatParameters[i].Data = value;
                }
            }
        }

        /// <summary>
        /// Sets a matrix4x4 parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public void SetMatrix4x4Parameter(string parameterName,in Matrix4 value)
        {
            for (int i = 0; i < m_Matrix4x4Parameters.Count; i++)
            {
                if (m_Matrix4x4Parameters[i].Name == parameterName)
                {
                    m_Matrix4x4Parameters[i].Data = value;
                }
            }
        }

        /// <summary>
        /// Sets a vecto4 parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public void SetVector4Parameter(string parameterName, in Vector4 value)
        {
            for (int i = 0; i < m_Vector4Parameters.Count; i++)
            {
                if (m_Vector4Parameters[i].Name == parameterName)
                {
                    m_Vector4Parameters[i].Data = value;
                }
            }
        }

        /// <summary>
        /// Sets a texture2 parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="texture"></param>
        public void SetTexture2DParameter(string parameterName, in Texture2D texture)
        {
            for (int i = 0; i < m_Texture2DParameters.Count; i++)
            {
                if (m_Texture2DParameters[i].Name == parameterName)
                {
                    m_Texture2DParameters[i].Data = texture;
                }
            }
        }

        /// <summary>
        /// Returns all float parameter
        /// </summary>
        public MaterialParameterField<float>[] FloatParameters
        {
            get
            {
                return m_FloatParameters.ToArray();
            }
        }

        /// <summary>
        /// Returns all texture2D parameters
        /// </summary>
        public MaterialParameterField<Texture2D>[] Texture2DParameters
        {
            get
            {
                return m_Texture2DParameters.ToArray();
            }
        }

        /// <summary>
        /// Returns all matrix4x4 parameter
        /// </summary>
        public MaterialParameterField<Matrix4>[] Matrix4x4Parameters
        {
            get
            {
                return m_Matrix4x4Parameters.ToArray();
            }
        }

        /// <summary>
        /// Returns all vecto4 parameters
        /// </summary>
        public MaterialParameterField<Vector4>[] Vector4Parameters
        {
            get
            {
                return m_Vector4Parameters.ToArray();
            }
        }

        private List<MaterialParameterField<float>> m_FloatParameters;
        private List<MaterialParameterField<Vector4>> m_Vector4Parameters;
        private List<MaterialParameterField<Matrix4>> m_Matrix4x4Parameters;
        private List<MaterialParameterField<Texture2D>> m_Texture2DParameters;
        private ShaderStage m_Stage;
    }
}
