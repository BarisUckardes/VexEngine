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
            m_Vector2Parameters = new List<MaterialParameterField<Vector2>>();
            m_Vector3Parameters = new List<MaterialParameterField<Vector3>>();
            m_Vector4Parameters = new List<MaterialParameterField<Vector4>>();
            m_Matrix4x4Parameters = new List<MaterialParameterField<Matrix4>>();
            m_Texture2DParameters = new List<MaterialParameterField<Texture2D>>();
            m_IntParameters = new List<MaterialParameterField<int>>();
            m_UIntParameters = new List<MaterialParameterField<uint>>();

            /*
             * Get parameters
             */
            for (int i = 0; i < stageParameters.Parameters.Length; i++)
            {
                switch (stageParameters.Parameters[i].Type)
                {
                    
                    case ShaderParameterType.Float:
                        m_FloatParameters.Add(new MaterialParameterField<float>(stageParameters.Parameters[i].Name, 0.0f));
                        break;
                    case ShaderParameterType.Vector2:
                        m_Vector2Parameters.Add(new MaterialParameterField<Vector2>(stageParameters.Parameters[i].Name, Vector2.Zero));
                        break;
                    case ShaderParameterType.Vector3:
                        m_Vector3Parameters.Add(new MaterialParameterField<Vector3>(stageParameters.Parameters[i].Name, Vector3.Zero));
                        break;
                    case ShaderParameterType.Vector4:
                        m_Vector4Parameters.Add(new MaterialParameterField<Vector4>(stageParameters.Parameters[i].Name, Vector4.Zero));
                        break;
                    case ShaderParameterType.Color:
                        break;
                    case ShaderParameterType.Matrix3x3:
                        break;
                    case ShaderParameterType.Matrix4x4:
                        m_Matrix4x4Parameters.Add(new MaterialParameterField<Matrix4>(stageParameters.Parameters[i].Name, Matrix4.Identity));
                        break;
                    case ShaderParameterType.Texture2D:
                        m_Texture2DParameters.Add(new MaterialParameterField<Texture2D>(stageParameters.Parameters[i].Name, null));
                        break;
                    case ShaderParameterType.Int:
                        m_IntParameters.Add(new MaterialParameterField<int>(stageParameters.Parameters[i].Name, 0));
                        break;
                    case ShaderParameterType.UInt:
                        m_UIntParameters.Add(new MaterialParameterField<uint>(stageParameters.Parameters[i].Name, 0));
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
        /// Sets a vector2 parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public void SetVector2Parameter(string parameterName, in Vector2 value)
        {
            for (int i = 0; i < m_Vector2Parameters.Count; i++)
            {
                if (m_Vector2Parameters[i].Name == parameterName)
                {
                    m_Vector2Parameters[i].Data = value;
                }
            }
        }

        /// <summary>
        /// Sets a vector3 parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public void SetVector3Parameter(string parameterName, in Vector3 value)
        {
            for (int i = 0; i < m_Vector4Parameters.Count; i++)
            {
                if (m_Vector3Parameters[i].Name == parameterName)
                {
                    m_Vector3Parameters[i].Data = value;
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
        /// Sets a int parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public void SetIntParameter(string parameterName,int value)
        {
            for(int i=0; i<m_IntParameters.Count;i++)
            {
                if (m_IntParameters[i].Name == parameterName)
                    m_IntParameters[i].Data = value;
            }
        }

        /// <summary>
        /// Sets a uint parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public void SetUIntParameter(string parameterName, uint value)
        {
            for (int i = 0; i < m_UIntParameters.Count; i++)
            {
                if (m_UIntParameters[i].Name == parameterName)
                    m_UIntParameters[i].Data = value;
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
        /// Returns all vector2 parameters
        /// </summary>
        public MaterialParameterField<Vector2>[] Vector2Parameters
        {
            get
            {
                return m_Vector2Parameters.ToArray();
            }
        }

        /// <summary>
        /// Returns all vector3 parameters
        /// </summary>
        public MaterialParameterField<Vector3>[] Vector3Parameters
        {
            get
            {
                return m_Vector3Parameters.ToArray();
            }
        }

        /// <summary>
        /// Returns all vector4 parameters
        /// </summary>
        public MaterialParameterField<Vector4>[] Vector4Parameters
        {
            get
            {
                return m_Vector4Parameters.ToArray();
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
        /// Returns all int parameters
        /// </summary>
        public MaterialParameterField<int>[] IntParameters
        {
            get
            {
                return m_IntParameters.ToArray();
            }
        }

        /// <summary>
        /// Returns all uint parameters
        /// </summary>
        public MaterialParameterField<uint>[] UIntParameters
        {
            get
            {
                return m_UIntParameters.ToArray();
            }
        }

       


        private List<MaterialParameterField<float>> m_FloatParameters;
        private List<MaterialParameterField<Vector2>> m_Vector2Parameters;
        private List<MaterialParameterField<Vector3>> m_Vector3Parameters;
        private List<MaterialParameterField<Vector4>> m_Vector4Parameters;
        private List<MaterialParameterField<Matrix4>> m_Matrix4x4Parameters;
        private List<MaterialParameterField<Texture2D>> m_Texture2DParameters;
        private List<MaterialParameterField<int>> m_IntParameters;
        private List<MaterialParameterField<uint>> m_UIntParameters;
        private ShaderStage m_Stage;
    }
}
