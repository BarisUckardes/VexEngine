using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Fang.Commands;
using System.Reflection;
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
             * Render world views
             */
            if (GUIRenderCommands.CreateTreeNode("Views","worldViews"))
            {
                foreach (WorldView view in m_Views)
                {
                    if(GUIRenderCommands.CreateCollapsingHeader(view.GetType().Name,"vw" + view.GetType().Name))
                    {
                        /*
                         * Iterate brute resolvers
                         */
                        List<Type> resolverType = new List<Type>();
                        foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            foreach(Type type in assembly.GetTypes())
                            {
                                if(type.IsAssignableTo(typeof(IWorldResolver)))
                                {
                                    /*
                                     * Try get target view
                                     */
                                    TargetViewAttribute viewAttribute = type.GetCustomAttribute<TargetViewAttribute>();
                                    if (viewAttribute !=null && viewAttribute.TargetViewType == view.GetType())
                                    {
                                        resolverType.Add(type);
                                    }
                                }
                            }
                        }

                        /*
                         * Display resolver types
                         */
                        foreach(Type type in resolverType)
                        {
                            GUIRenderCommands.CreateText(type.Name, "");
                            GUILayoutCommands.StayOnSameLine();

                            if(IsViewContains(view,type))
                            {
                                if (GUIRenderCommands.CreateButton("-", type.Name + "bbtn"))
                                {
                                    view.RemoveResolver(type);
                                }
                            }
                            else
                            {
                                if (GUIRenderCommands.CreateButton("+", type.Name + "bbtn"))
                                {
                                    view.RegisterResolver(type);
                                }
                            }
                        }
                    }
                    GUIRenderCommands.CreateEmptySpace();
                }
                GUIRenderCommands.FinalizeTreeNode();
            }
           
        }
        private bool IsViewContains(WorldView view,Type type)
        {
            foreach (IWorldResolver resolver in view.Resolvers)
                if (resolver.GetType() == type)
                    return true;
            return false;
        }
        private List<WorldView> m_Views;
        private List<List<Type>> m_ViewResolvers;
        private World m_TargetWorld;
    }
}
