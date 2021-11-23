using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;
using Fang.Commands;
using ImGuiNET;

namespace Bite.GUI
{
    [ObjectLayout(typeof(Shader))]
    public sealed class ShaderGUIObjectLayout : ObjectLayout
    {
        public override void OnAttach()
        {
            m_TargetShader = Object as Shader;
            m_ShaderSourceEditText = m_TargetShader.Source;
        }

        public override void OnDetach()
        {
            m_TargetShader = null;
        }

        public override void OnLayoutRender()
        {
            /*
             * Render shader name Text
             */
            string source = m_TargetShader.Source;
            GUIRenderCommands.CreateText("SHADER"," ");
            GUILayoutCommands.StayOnSameLine();
            GUIRenderCommands.CreateText("["+m_TargetShader.Type.ToString()+"]", " ");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateMultilineTextInput("","code",ref source,new System.Numerics.Vector2(ImGui.GetContentRegionMax().X,512),ImGuiInputTextFlags.ReadOnly);

            bool isEditSource = false;
            if(GUIRenderCommands.CreateButton("Edit Source"," "))
            {
                isEditSource = true;
            }
           
            if(m_TargetShader.LastErrorMessage.Length >0)
            {
                string errorString = m_TargetShader.LastErrorMessage;
                GUIRenderCommands.CreateEmptySpace();
                GUIRenderCommands.CreateText("ERRORS","txt");
                GUIRenderCommands.CreateSeperatorLine();
                GUIRenderCommands.EnableStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(0.8f, 0, 0, 1));
                GUIRenderCommands.CreateMultilineTextInput("","err",ref errorString, ImGui.GetContentRegionMax(), ImGuiInputTextFlags.ReadOnly);
                GUIRenderCommands.DisableStyleColor();
            }
            if (isEditSource)
                GUIRenderCommands.SignalPopupCreate("Shader_Edit_Source");

            if(GUIRenderCommands.CreatePopup("Shader_Edit_Source"))
            {
                RenderSourceTextPopup();
            }
        }

        private void RenderSourceTextPopup()
        {
            GUIRenderCommands.CreateText("EDIT SHADER SOURCE CODE", " ");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateMultilineTextInput(" ", "mmm", ref m_ShaderSourceEditText, new System.Numerics.Vector2(512, 512));
            if(GUIRenderCommands.CreateButton("Apply&Compile","apply-But"))
            {
                m_TargetShader.Compile(m_ShaderSourceEditText);
                GUIRenderCommands.TerminateCurrentPopup();
                Session.UpdateDomainAsset(m_TargetShader.ID,m_TargetShader);
                
            }

        }
        private string m_ShaderSourceEditText = string.Empty;
        private Shader m_TargetShader;
    }
}
