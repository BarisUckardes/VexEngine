using Fang.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;
using Bite.Core;
using Vex.Framework;
using Vex.Types;
namespace Bite.GUI
{
    [ComponentLayout(typeof(FreeGameObserver))]
    public sealed class FreeGameObserverComponentLayout : ComponentLayout
    {
        public override void OnAttach()
        {
            m_TargetObserver = TargetComponent as FreeGameObserver;
            m_GraphicsViewInformationBlock = m_TargetObserver.OwnerEntity.World.GetView<WorldGraphicsView>().InformationBlock;
        }

        public override void OnDetach()
        {
            m_TargetObserver = null;
            m_GraphicsViewInformationBlock = null;
        }

        public override void OnLayoutRender()
        {
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateEmptySpace();

            /*
             * Create framebuffer resources header
             */
            //GUIRenderCommands.CreateText("Framebuffer2D Resources","");
            //GUIRenderCommands.CreateSeperatorLine();
            //GUIRenderCommands.CreateEmptySpace();
            //GUIRenderCommands.CreateEmptySpace();

            ///*
            // * Render framebuffer resources
            // */
            //List<Framebuffer2D> framebuffer2DResources = m_TargetObserver.Framebuffer2DResources;

            ///*
            // * Render framebuffer list
            // */
            //foreach(Framebuffer2D framebuffer in framebuffer2DResources)
            //{
            //    GUIRenderCommands.CreateEmptySpace();
            //    Vector2 treeCursorPos = GUILayoutCommands.GetCursorScreenPos();
            //    if (GUIRenderCommands.CreateTreeNode("Framebuffer ##" + framebuffer.ID.ToString(),framebuffer.ID.ToString()))
            //    {
            //        /*
            //         * Render back rect filled
            //         */
            //        Vector2 cursorPos = GUILayoutCommands.GetCursorScreenPos();
            //        GUIRenderCommands.DrawRectangleFilled(cursorPos+ new Vector2(-5,-2), cursorPos + new Vector2(GUILayoutCommands.GetAvailableSpace().X,50), new Vector4(0.15f, 0.1505f, 0.151f, 1.0f),0);
            //        GUIRenderCommands.DrawRectangle(cursorPos + new Vector2(-5, -2), cursorPos + new Vector2(GUILayoutCommands.GetAvailableSpace().X, 50), new Vector4(0.45f, 0.4505f, 0.451f, 1.0f), 0,ImGuiNET.ImDrawCornerFlags.Left,1.0f);

            //        string name = framebuffer.Name;
            //        GUIRenderCommands.CreateText("Name:", "");
            //        GUILayoutCommands.StayOnSameLine();
            //        GUIRenderCommands.CreateText(framebuffer.ID.ToString(),"");
            //        framebuffer.Name = name;
            //        GUIRenderCommands.CreateText("Framebuffer:", "");
            //        GUILayoutCommands.StayOnSameLine();
            //        GUIRenderCommands.CreateObjectField(framebuffer, "Frm2d");
            //        GUIRenderCommands.CreateEmptySpace();
            //        GUIRenderCommands.CreateEmptySpace();
            //        GUIRenderCommands.FinalizeTreeNode();
            //    }
            //}

            /*
             * Create framebuffer2D button
             */
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateSeperatorLine();
           
        }
       

        private FreeGameObserver m_TargetObserver;
        private GraphicsViewInformationBlock m_GraphicsViewInformationBlock;
    }
}
