using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Fang.Commands;
using System.Reflection;
using ImGuiNET;
using Vex.Types;
namespace Bite.GUI
{
    [ObjectLayout(typeof(World))]
    public sealed class WorldGUIObjectLayout : ObjectLayout
    {
        public override void OnAttach()
        {
            /*
             * Get world
             */
            m_TargetWorld = Object as World;

            /*
             * Get views
             */
            m_Views = m_TargetWorld.Views;

            /*
             * Collect view types
             */
            m_ViewTypes = EmittedWorldViewTypes.Types;

            /*
             * Collect resolver types
             */
            m_ResolverTypes = EmittedWorldViewResolverTypes.Types;
        }

        public override void OnDetach()
        {

        }

        public override void OnLayoutRender()
        {
            /*
             * Render header
             */
            GUIRenderCommands.CreateText("World", "");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();

            /*
             * Get view types
             */

            /*
             * Render world views
             */
            if (GUIRenderCommands.CreateTreeNode("Views","worldViews"))
            {
                foreach(Type viewType in m_ViewTypes)
                {
                    /*
                     * Try get view
                     */
                    WorldView foundView = GetView(viewType);

                    /*
                     * Validate found view
                     */
                    if(foundView != null)
                    {
                        if (GUIRenderCommands.CreateCollapsingHeader(viewType.Name, "vw" + viewType.Name))
                        {
                            /*
                             * ICollect resolvers
                             */
                            List<Type> resolverTypes = new List<Type>();
                            foreach (Type type in m_ResolverTypes)
                            {
                                TargetViewAttribute viewAttribute = type.GetCustomAttribute<TargetViewAttribute>();
                                if (viewAttribute != null && viewAttribute.TargetViewType == foundView.GetType())
                                {
                                    resolverTypes.Add(type);
                                }
                            }

                            /*
                             * Display resolver types
                             */
                            foreach (Type type in resolverTypes)
                            {
                                GUIRenderCommands.CreateText(type.Name, "");
                                GUILayoutCommands.StayOnSameLine();

                                /*
                                 * Validate game session and render resolvers
                                 */
                                if (!Session.GamePlayState)
                                {
                                    if (IsViewContains(foundView, type))
                                    {
                                        if (GUIRenderCommands.CreateButton("Unregister", type.Name + "bbtn"))
                                        {
                                            foundView.RemoveResolver(type);
                                        }
                                    }
                                    else
                                    {
                                        if (GUIRenderCommands.CreateButton("Register", type.Name + "bbtn"))
                                        {
                                            foundView.RegisterResolver(type);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        GUIRenderCommands.CreateText(viewType.Name , "");
                        GUILayoutCommands.StayOnSameLine();
                        if(GUIRenderCommands.CreateButton("Register","btn_" + viewType.Name))
                        {
                            /*
                             * Create new view
                             */
                            m_TargetWorld.AddView(viewType, true);
                            Refresh();
                        }
                    }
                   
                    GUIRenderCommands.CreateEmptySpace();
                }
                GUIRenderCommands.FinalizeTreeNode();
            }

            /*
             * Render view menu
             */
            GUIRenderCommands.CreateEmptySpace();
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();
           
        }
        private bool IsViewContains(WorldView view,Type type)
        {
            foreach (IWorldResolver resolver in view.Resolvers)
                if (resolver.GetType() == type)
                    return true;
            return false;
        }
        private WorldView GetView(Type type)
        {
            foreach (WorldView view in m_Views)
                if (view.GetType() == type)
                    return view;
            return null;
        }
        private void Refresh()
        {
            m_Views = m_TargetWorld.Views;
        }     

        private List<WorldView> m_Views;
        private List<Type> m_ViewTypes;
        private List<Type> m_ResolverTypes;
        private World m_TargetWorld;
    }
}
