using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;
using Fang.Commands;
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
            GUIRenderCommands.CreateObjectField(m_TargetMaterial.Program, "prg");

        }

        private Material m_TargetMaterial;
    }
}
