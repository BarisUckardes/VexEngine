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
            m_Shaders = m_TargetProgram.Shaders;
        }

        public override void OnDetach()
        {
            m_TargetProgram = null;
        }

        public override void OnLayoutRender()
        {
            /*
             * Render header
             */
            GUIRenderCommands.CreateText("SHADER PROGRAM", " ");
            GUIRenderCommands.CreateSeperatorLine();

            /*
             * Render category
             */
            GUIRenderCommands.CreateText("Category: ", " ");
            GUILayoutCommands.StayOnSameLine();
            GUIRenderCommands.CreateText(m_TargetProgram.Category, " ");

            /*
             * Render category name
             */
            GUIRenderCommands.CreateText("Category Name: ", " ");
            GUILayoutCommands.StayOnSameLine();
            GUIRenderCommands.CreateText(m_TargetProgram.CategoryName, " ");
            GUIRenderCommands.CreateEmptySpace();

            /*
             * Render shader set
             */
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateText("SHADER SET", " ");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();
            List<Shader> shaders = m_Shaders;
            for(int shaderIndex = 0;shaderIndex < shaders.Count;shaderIndex++)
            {
                shaders[shaderIndex] = GUIRenderCommands.CreateObjectField(shaders[shaderIndex], "shdr_" + shaderIndex) as Shader;
                GUIRenderCommands.CreateEmptySpace();
            }

            /*
             * Render add shader
             */
            if(GUIRenderCommands.CreateButton("+","add_shdr"))
            {
                m_Shaders.Add(null);
            }

            /*
             * Render button
             */
            if(GUIRenderCommands.CreateButton("Apply","apply"))
            {
                m_TargetProgram.LinkProgram(m_Shaders);
                Session.UpdateDomainAsset(m_TargetProgram.ID, m_TargetProgram);
            }

            /*
             * Render error
             */
            if(m_TargetProgram.LastErrorMessage != string.Empty)
            {
                string message = m_TargetProgram.LastErrorMessage;
                GUIRenderCommands.CreateMultilineTextInput("", "shdr_prg_err", ref message, ImGuiNET.ImGui.GetContentRegionMax(),ImGuiNET.ImGuiInputTextFlags.ReadOnly);
            }

 
        }

        private ShaderProgram m_TargetProgram;
        private List<Shader> m_Shaders;
    }
}
