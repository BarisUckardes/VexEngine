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
            /*
             * Create framebuffer resources header
             */
            GUIRenderCommands.CreateText("Framebuffer2D Resources","");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();

            /*
             * Render framebuffer resources
             */
            List<Framebuffer2D> framebuffer2DResources = m_TargetObserver.Framebuffer2DResources;

            /*
             * Draw list
             */
            foreach(Framebuffer2D framebuffer in framebuffer2DResources)
            {
                GUIRenderCommands.CreateEmptySpace();
                string name = framebuffer.Name;
                GUIRenderCommands.CreateTextInput("", framebuffer.ID.ToString(),ref name);
                framebuffer.Name = name;
                GUIRenderCommands.CreateObjectField(framebuffer, "Frm2d");
            }

            /*
             * Create framebuffer2D button
             */
            if(GUIRenderCommands.CreateButton("Create framebuffer2d","ibtn"))
            {
                m_TargetObserver?.CreateFramebuffer2DResource(512, 512, TextureFormat.Rgba, TextureInternalFormat.Rgba32f);
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
                string name = pass.PassName;
                GUIRenderCommands.CreateTextInput("", "it",ref name);
                pass.PassName = name;
                pass.TargetFramebuffer = GUIRenderCommands.CreateObjectField(pass.TargetFramebuffer, "ipas") as Framebuffer2D;

                /*
                 * Render pairs
                 */
                GUIRenderCommands.CreateText("Resolver pairs", "");
                GUIRenderCommands.CreateSeperatorLine();
                GUIRenderCommands.CreateEmptySpace();
                List<RenderPassResolverMaterialPair> pairs = pass.ResolverMaterialPairs;
                foreach(RenderPassResolverMaterialPair pair in pairs)
                {
                    /*
                     * Set target material
                     */
                    pair.TargetMaterial = GUIRenderCommands.CreateObjectField(pair.TargetMaterial, "materialType") as Material;

                    /*
                     * Set resolver type
                     */
                    if(GUIRenderCommands.CreateCombo("","","resolverType"))
                    {
                        foreach (Type type in m_GraphicsResolverTypes)
                            if (GUIRenderCommands.CreateButton("Select " + type.Name,"typeSelectButton"))
                                pair.TargetResolver = type;

                        GUIRenderCommands.FinalizeCombo();
                    }


                    GUIRenderCommands.CreateSeperatorLine();
                    GUIRenderCommands.CreateEmptySpace();
                }

                /*
                * Render pair create button
                */
                GUIRenderCommands.CreateEmptySpace();
                if (GUIRenderCommands.CreateButton("Create resolver pair", "createRenderPassPairButton"))
                {
                    pass.RegisterPair(new RenderPassResolverMaterialPair());
                }

                GUIRenderCommands.CreateEmptySpace();
                GUIRenderCommands.CreateSeperatorLine();
                GUIRenderCommands.CreateEmptySpace();
            }

            if(GUIRenderCommands.CreateButton("Create render pass","ipbtn"))
            {
                m_TargetObserver.CreateRenderPass("Helloo pass");
            }

        }

        private FreeGameObserver m_TargetObserver;
        private List<Type> m_GraphicsResolverTypes;
    }
}
