using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;
using Fang.Commands;
namespace Bite.GUI
{
    [ObjectLayout(typeof(ShaderProgram))]
    public sealed class ShaderProgramObjectLayout : ObjectLayout
    {
        public override void OnAttach()
        {
            m_TargetProgram = Object as ShaderProgram;
        }

        public override void OnDetach()
        {
            m_TargetProgram = null;
        }

        public override void OnLayoutRender()
        {
            GUIRenderCommands.CreateText("SHADER PROGRAM", " ");
            GUIRenderCommands.CreateSeperatorLine();

            GUIRenderCommands.CreateText("Category: ", " ");
            GUILayoutCommands.StayOnSameLine();
            GUIRenderCommands.CreateText(m_TargetProgram.Category, " ");

            GUIRenderCommands.CreateText("Category Name: ", " ");
            GUILayoutCommands.StayOnSameLine();
            GUIRenderCommands.CreateText(m_TargetProgram.CategoryName, " ");
            GUIRenderCommands.CreateEmptySpace();

            GUIRenderCommands.CreateText("SHADER SET", " ");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();
            Shader[] shaders = m_TargetProgram.Shaders;
            for(int shaderIndex = 0;shaderIndex < shaders.Length;shaderIndex++)
            {
                /*
                 * Get shader
                 */
                Shader shader = shaders[shaderIndex];
                GUIRenderCommands.CreateSelectableItem(shader.Type.ToString(), " ");
                GUIRenderCommands.CreateEmptySpace();
            }

        }

        private ShaderProgram m_TargetProgram;
    }
}
