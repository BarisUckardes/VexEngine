using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;
using Fang.Commands;
using OpenTK.Mathematics;
using Vex.Extensions;

namespace Bite.GUI
{
    [ObjectLayout(typeof(Material))]
    public sealed class MaterialGUIObjectLayout : ObjectLayout
    {
        public override void OnAttach()
        {
            m_TargetMaterial = Object as Material;
        }

        public override void OnDetach()
        {
            m_TargetMaterial = null;
        }

        public override void OnLayoutRender()
        {
            
            /*
             * Render header
             */
            GUIRenderCommands.CreateText("MATERIAL", " ");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();

            /*
             * Render shader program
             */
            GUIRenderCommands.CreateText("Shader Program: "," ");
            GUILayoutCommands.StayOnSameLine();
            ShaderProgram program = GUIRenderCommands.CreateObjectField(m_TargetMaterial.Program, "shaderProgram0") as ShaderProgram;

            if(m_TargetMaterial.Program != program && program != null)
            {
                m_TargetMaterial.Program = program;
            }

            /*
             * Render stage parameters
             */
            MaterialStageParameters[] stageParameters = m_TargetMaterial.StageParameters;

            /*
             * Iterate stage parameters
             */
            for(int stageIndex = 0;stageIndex < stageParameters.Length;stageIndex++)
            {
                /*
                 * Get stage
                 */
                MaterialStageParameters stage = stageParameters[stageIndex];

                /*
                 * Render header
                 */
                if (GUIRenderCommands.CreateCollapsingHeader($"{stage.Stage.ToString()} Parameters","stg" + stageIndex))
                {
                    /*
                     * Render float parameters
                     */
                    MaterialParameterField<float>[] floatParameters = stage.FloatParameters;
                    for(int parameterIndex = 0;parameterIndex < floatParameters.Length;parameterIndex++)
                    {
                        /*
                         * Render float header
                         */
                        float value = floatParameters[parameterIndex].Data;
                        GUIRenderCommands.CreateText(floatParameters[parameterIndex].Name, " ");
                        GUILayoutCommands.StayOnSameLine();
                        GUIRenderCommands.CreateFloatSlider(floatParameters[parameterIndex].Name," ", ref value);
                        
                    }

                    /*
                     * Render vector2 parameters
                     */
                    MaterialParameterField<Vector2>[] vector2Parameters = stage.Vector2Parameters;
                    for (int parameterIndex = 0; parameterIndex < vector2Parameters.Length; parameterIndex++)
                    {
                        System.Numerics.Vector2 value = vector2Parameters[parameterIndex].Data.GetAsNumerics();
                        GUIRenderCommands.CreateText(vector2Parameters[parameterIndex].Name, " ");
                        GUILayoutCommands.StayOnSameLine();
                        GUIRenderCommands.CreateVector2Slider(floatParameters[parameterIndex].Name," ", ref value);
                    }

                    /*
                     * Render vector3 parameters
                     */
                    MaterialParameterField<Vector3>[] vector3Parameters = stage.Vector3Parameters;
                    for (int parameterIndex = 0; parameterIndex < vector3Parameters.Length; parameterIndex++)
                    {
                        System.Numerics.Vector3 value = vector3Parameters[parameterIndex].Data.GetAsNumerics();
                        GUIRenderCommands.CreateText(vector3Parameters[parameterIndex].Name, " ");
                        GUILayoutCommands.StayOnSameLine();
                        GUIRenderCommands.CreateVector3Slider(vector3Parameters[parameterIndex].Name," ",ref value);
                    }

                    /*
                     * Render vector4 parameters
                     */
                    MaterialParameterField<Vector4>[] vector4Parameters = stage.Vector4Parameters;
                    for (int parameterIndex = 0; parameterIndex < vector4Parameters.Length; parameterIndex++)
                    {
                        System.Numerics.Vector4 value = vector4Parameters[parameterIndex].Data.GetAsNumerics();
                        GUIRenderCommands.CreateText(vector4Parameters[parameterIndex].Name, " ");
                        GUILayoutCommands.StayOnSameLine();
                        GUIRenderCommands.CreateVector4Slider(vector4Parameters[parameterIndex].Name," ",ref value);
                    }

                    /*
                     * Render matrix4x4 parameters
                     */
                    MaterialParameterField<Matrix4>[] matrix4x4Parameters = stage.Matrix4x4Parameters;
                    for (int parameterIndex = 0; parameterIndex < matrix4x4Parameters.Length; parameterIndex++)
                    {
                        Matrix4 value = matrix4x4Parameters[parameterIndex].Data;
                        GUIRenderCommands.CreateText(matrix4x4Parameters[parameterIndex].Name, " ");
                    }

                    /*
                     * Render texture2D parameters
                     */
                    MaterialParameterField<Texture2D>[] texture2DParameters = stage.Texture2DParameters;
                    for (int parameterIndex = 0; parameterIndex < texture2DParameters.Length; parameterIndex++)
                    {
                        Texture2D value = texture2DParameters[parameterIndex].Data;
                        GUIRenderCommands.CreateText(texture2DParameters[parameterIndex].Name, " ");
                        GUILayoutCommands.StayOnSameLine();
                        GUIRenderCommands.CreateObjectField(value, " ");
                    }
                }
            }


        }

        private Material m_TargetMaterial;
    }
}
