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
            m_GraphicsResolverTypes = EmittedWorldViewResolverTypes.Types;
            for(int typeIndex = 0;typeIndex < m_GraphicsResolverTypes.Count;typeIndex++)
            {
                /*
                 * Get type
                 */
                Type type = m_GraphicsResolverTypes[typeIndex];

                /*
                 * Validate type
                 */
                if(!type.IsAssignableTo(typeof(GraphicsResolver)))
                {
                    m_GraphicsResolverTypes.RemoveAt(typeIndex);
                    typeIndex--;
                }

            }
        }

        public override void OnDetach()
        {
            m_TargetObserver = null;
        }

        public override void OnLayoutRender()
        {
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateEmptySpace();
            /*
             * Create framebuffer resources header
             */
            GUIRenderCommands.CreateText("Framebuffer2D Resources","");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateEmptySpace();

            /*
             * Render framebuffer resources
             */
            List<Framebuffer2D> framebuffer2DResources = m_TargetObserver.Framebuffer2DResources;

            /*
             * Render framebuffer list
             */
            foreach(Framebuffer2D framebuffer in framebuffer2DResources)
            {
                GUIRenderCommands.CreateEmptySpace();
                Vector2 treeCursorPos = GUILayoutCommands.GetCursorScreenPos();
                if (GUIRenderCommands.CreateTreeNode("Framebuffer ##" + framebuffer.ID.ToString(),framebuffer.ID.ToString()))
                {
                    /*
                     * Render back rect filled
                     */
                    Vector2 cursorPos = GUILayoutCommands.GetCursorScreenPos();
                    GUIRenderCommands.DrawRectangleFilled(cursorPos+ new Vector2(-5,-2), cursorPos + new Vector2(GUILayoutCommands.GetAvailableSpace().X,50), new Vector4(0.15f, 0.1505f, 0.151f, 1.0f),0);
                    GUIRenderCommands.DrawRectangle(cursorPos + new Vector2(-5, -2), cursorPos + new Vector2(GUILayoutCommands.GetAvailableSpace().X, 50), new Vector4(0.45f, 0.4505f, 0.451f, 1.0f), 0,ImGuiNET.ImDrawCornerFlags.Left,1.0f);

                    string name = framebuffer.Name;
                    GUIRenderCommands.CreateText("Name:", "");
                    GUILayoutCommands.StayOnSameLine();
                    GUIRenderCommands.CreateTextInput("", framebuffer.ID.ToString(), ref name);
                    framebuffer.Name = name;
                    GUIRenderCommands.CreateText("Framebuffer:", "");
                    GUILayoutCommands.StayOnSameLine();
                    GUIRenderCommands.CreateObjectField(framebuffer, "Frm2d");
                    GUIRenderCommands.CreateEmptySpace();
                    GUIRenderCommands.CreateEmptySpace();
                    GUIRenderCommands.FinalizeTreeNode();
                }
            }

            /*
             * Create framebuffer2D button
             */
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateSeperatorLine();
            if(GUIRenderCommands.CreateButton("Create framebuffer2d","ibtn"))
            {
                GUIRenderCommands.SignalPopupCreate("Create_Framebuffer_Popup");
                //m_TargetObserver?.CreateFramebuffer2DResource(512, 512, TextureFormat.Rgb, TextureInternalFormat.Rgb8);
            }
            if(GUIRenderCommands.CreatePopup("Create_Framebuffer_Popup"))
            {
                RenderCreateFramebufferPopup();
                GUIRenderCommands.FinalizePopup();
            }
            /*
             * Render passes
             */
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateText("Render passes", "");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();

            List<RenderPass> renderPasses = m_TargetObserver.RenderPasses;
            foreach(RenderPass pass in renderPasses)
            {
                /*
                 * Render selected framebuffer
                 */
                if(GUIRenderCommands.CreateTreeNode("Render Pass",pass.GetHashCode().ToString()))
                {
                    /*
                     * Get resolver pairs
                     */
                    List<RenderPassResolverMaterialPair> pairs = pass.ResolverMaterialPairs;

                    /*
                     * Draw rects
                     */
                    Vector2 cursorPos = GUILayoutCommands.GetCursorScreenPos();
                  

                    /*
                     * Draw pass name
                     */
                    string name = pass.PassName;
                    GUIRenderCommands.CreateText("Pass name", "");
                    GUILayoutCommands.StayOnSameLine();
                    GUIRenderCommands.CreateTextInput("", "it", ref name);
                    pass.PassName = name;

                    /*
                     * Draw framebuffer
                     */
                    GUIRenderCommands.CreateText("Target framebuffer", "");
                    GUILayoutCommands.StayOnSameLine();
                    pass.TargetFramebuffer = GUIRenderCommands.CreateObjectField(pass.TargetFramebuffer, "ipas") as Framebuffer2D;

                    /*
                     * Render pipeline state
                     */
                    GUIRenderCommands.CreateText("Enable Depth Testing:", "");
                    GUILayoutCommands.StayOnSameLine();
                    pass.UseDepthTest = GUIRenderCommands.CreateCheckbox("", "",pass.UseDepthTest);

                    GUIRenderCommands.CreateText("Use Clear Color:", "");
                    GUILayoutCommands.StayOnSameLine();
                    pass.UseClearColor = GUIRenderCommands.CreateCheckbox("", "useClearColor", pass.UseClearColor);
                    if(pass.UseClearColor)
                    {
                        GUIRenderCommands.CreateText("Clear Color:", "");
                        GUILayoutCommands.StayOnSameLine();
                        pass.ClearColor = GUIRenderCommands.CreateColorPicker("clearColor", pass.ClearColor);
                    }

                    GUIRenderCommands.CreateText("Use Clear Depth:", "");
                    GUILayoutCommands.StayOnSameLine();
                    pass.UseClearDepth = GUIRenderCommands.CreateCheckbox("", "useClearDepth", pass.UseClearDepth);
                    if (pass.UseClearDepth)
                    {
                        GUIRenderCommands.CreateText("Clear Depth:", "");
                        GUILayoutCommands.StayOnSameLine();
                        pass.ClearDepthValue = GUIRenderCommands.CreateFloatSlider("","clearColor", pass.ClearDepthValue,0,1);
                    }

                    GUIRenderCommands.CreateText("Front Face:","");
                    GUILayoutCommands.StayOnSameLine();
                    pass.FrontFace = (TriangleFrontFace)GUIRenderCommands.CreateEnumBox("", "frontFaceCombo", pass.FrontFace);

                    GUIRenderCommands.CreateText("Cull Face:", "");
                    GUILayoutCommands.StayOnSameLine();
                    pass.CullFace = (CullFace)GUIRenderCommands.CreateEnumBox("", "cullFaceEnum", pass.CullFace);

                    GUIRenderCommands.CreateText("Polygon Fill Method:", "");
                    GUILayoutCommands.StayOnSameLine();
                    pass.FillMethod = (PolygonFillMethod)GUIRenderCommands.CreateEnumBox("", "fillMethodEnum", pass.FillMethod);

                    GUIRenderCommands.CreateText("Fill Face Method:", "");
                    GUILayoutCommands.StayOnSameLine();
                    pass.FillFace = (PolygonFillFace)GUIRenderCommands.CreateEnumBox("", "fillFaceMethod", pass.FillFace);

                    GUIRenderCommands.CreateText("Depth Function:", "");
                    GUILayoutCommands.StayOnSameLine();
                    pass.DepthFunction = (DepthFunction)GUIRenderCommands.CreateEnumBox("", "depthFunctionEnum", pass.DepthFunction);

                    /*
                     * Render pairs
                     */
                    if(GUIRenderCommands.CreateTreeNode("Resolver-Material Pairs","r-m pairs"))
                    {
                        foreach (RenderPassResolverMaterialPair pair in pairs)
                        {
                            if(GUIRenderCommands.CreateTreeNode("Pair",pair.GetHashCode().ToString()))
                            {
                                /*
                                 * Draw rect
                                 */
                                GUIRenderCommands.DrawRectangleFilled(GUILayoutCommands.GetCursorScreenPos() + new Vector2(-5, -2), GUILayoutCommands.GetCursorScreenPos() + new Vector2(GUILayoutCommands.GetAvailableSpace().X, 45), new Vector4(0.15f, 0.1505f, 0.151f, 1.0f), 0);
                                GUIRenderCommands.DrawRectangle(GUILayoutCommands.GetCursorScreenPos() + new Vector2(-5, -2), GUILayoutCommands.GetCursorScreenPos() + new Vector2(GUILayoutCommands.GetAvailableSpace().X, 45), new Vector4(0.45f, 0.4505f, 0.451f, 1.0f), 0, ImGuiNET.ImDrawCornerFlags.All, 1.0f);

                                /*
                                 * Set target material
                                 */
                                GUIRenderCommands.CreateText("Material:", "");
                                GUILayoutCommands.StayOnSameLine();
                                pair.TargetMaterial = GUIRenderCommands.CreateObjectField(pair.TargetMaterial, "materialType") as Material;

                                /*
                                 * Set resolver type
                                 */
                                GUIRenderCommands.CreateText("Target resolver", "");
                                GUILayoutCommands.StayOnSameLine();
                                if (GUIRenderCommands.CreateCombo("", "", "resolverType"))
                                {
                                    foreach (Type type in m_GraphicsResolverTypes)
                                        if (GUIRenderCommands.CreateButton("Select " + type.Name, "typeSelectButton"))
                                            pair.TargetResolver = type;

                                    GUIRenderCommands.FinalizeCombo();
                                }
                                GUIRenderCommands.FinalizeTreeNode();
                            }
                            GUIRenderCommands.CreateEmptySpace();
                            GUIRenderCommands.CreateEmptySpace();
                        }

                        
                        GUIRenderCommands.FinalizeTreeNode();
                    }

                    /*
                     * Render pair create button
                     */
                    GUIRenderCommands.CreateEmptySpace();
                    if (GUIRenderCommands.CreateButton("Create resolver pair", "createRenderPassPairButton"))
                    {
                        pass.RegisterPair(new RenderPassResolverMaterialPair());
                    }

                   // GUIRenderCommands.DrawRectangleFilled(cursorPos + new Vector2(-5, -2), GUILayoutCommands.GetCursorScreenPos() + new Vector2(GUILayoutCommands.GetAvailableSpace().X, 0), new Vector4(0.15f, 0.1505f, 0.151f, 1.0f), 0);
                    GUIRenderCommands.DrawRectangle(cursorPos + new Vector2(-5, -2), GUILayoutCommands.GetCursorScreenPos() + new Vector2(GUILayoutCommands.GetAvailableSpace().X, 0), new Vector4(0.45f, 0.4505f, 0.451f, 1.0f), 0, ImGuiNET.ImDrawCornerFlags.All, 1.0f);

                    GUIRenderCommands.CreateEmptySpace();
                    GUIRenderCommands.CreateEmptySpace();
                    GUIRenderCommands.FinalizeTreeNode();
                }

               

                GUIRenderCommands.CreateEmptySpace();
                GUIRenderCommands.CreateEmptySpace();
            }

            if(GUIRenderCommands.CreateButton("Create render pass","ipbtn"))
            {
                m_TargetObserver.CreateRenderPass("Helloo pass");
            }
        }
        private void RenderCreateFramebufferPopup()
        {
            GUIRenderCommands.CreateText("Create framebuffer 2d","");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();

            GUIRenderCommands.CreateText("Width:", "");
            m_FramebufferWidth = GUIRenderCommands.CreateIntInput("","width_input",m_FramebufferWidth);

            GUIRenderCommands.CreateText("Height:", "");
            m_FramebufferHeight = GUIRenderCommands.CreateIntInput("", "height_input", m_FramebufferHeight);

            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateText("Texture Format:", "");
            m_FramebufferTextureFormat = (TextureFormat)GUIRenderCommands.CreateEnumBox("", "framebuffer_texture_format", m_FramebufferTextureFormat);

            GUIRenderCommands.CreateText("Texture Internal Format:", "");
            m_FramebufferInternalTextureFormat = (TextureInternalFormat)GUIRenderCommands.CreateEnumBox("", "framebuffer_texture_internal_format",m_FramebufferInternalTextureFormat);

            GUIRenderCommands.CreateText("Texture Internal Format:", "");
            m_FramebufferDataType = (TextureDataType)GUIRenderCommands.CreateEnumBox("", "framebuffer_texture_data_type", m_FramebufferDataType);

            if (GUIRenderCommands.CreateButton("Create","create"))
            {
                m_TargetObserver.CreateFramebuffer2DResource(m_FramebufferWidth, m_FramebufferHeight, m_FramebufferTextureFormat, m_FramebufferInternalTextureFormat,m_FramebufferDataType);
                GUIRenderCommands.TerminateCurrentPopup();
            }

        }

        private FreeGameObserver m_TargetObserver;
        private List<Type> m_GraphicsResolverTypes;
        private TextureFormat m_FramebufferTextureFormat = TextureFormat.RedInteger;
        private TextureInternalFormat m_FramebufferInternalTextureFormat = TextureInternalFormat.R32ui;
        private TextureDataType m_FramebufferDataType = TextureDataType.UnsignedInt;
        private int m_FramebufferWidth = 512;
        private int m_FramebufferHeight = 512;
    }
}
