using Vex.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
using Vex.Types;
using ImGuiNET;
using Vex.Graphics;

namespace Bite.GUI
{
    [ObjectLayout(typeof(Entity))]
    public sealed class EntityGUIObjectLayout : ObjectLayout
    {
        public override void OnAttach()
        {
            m_TargetEntity = Object as Entity;
            m_Layouts = new List<ComponentLayout>();
            m_ComponentIcon = Session.GetEditorResource("ComponentIcon", Vex.Asset.AssetType.Texture2D) as Texture2D;
            m_DeleteIcon = Session.GetEditorResource("DeleteIcon", Vex.Asset.AssetType.Texture2D) as Texture2D;

            /*
             * Get all commponents
             */
            m_AllComponentTypes = EmittedComponentTypes.ComponentTypes;

            /*
             * recreate component layouts
             */
            RecreateComponentLayouts();
        }

        public override void OnDetach()
        {
            for(int i=0;i<m_Layouts.Count;i++)
            {
                m_Layouts[i].OnDetach();
            }
            m_Layouts.Clear();
        }

        public override void OnLayoutRender()
        {
            /*
             * Validate entity
             */
            if(m_TargetEntity == null || m_TargetEntity.IsDestroyed)
            {
                GUIRenderCommands.CreateText("This entity seems null","");
                m_TargetEntity = null;
                return;
            }

            /*
             * Render entity layout
             */
            RenderEntityLayout();
        }
        private void RenderEntityLayout()
        {
            /*
             * Create invalidate state
             */
            bool needsInvalidation = false;

            /*
             * Display entity name
             */
            GUIRenderCommands.CreateText(m_TargetEntity.Name, "");
            GUIRenderCommands.CreateSeperatorLine();

            /*
             * Create components
             */
            GUILayoutCommands.Spacing();
            GUIRenderCommands.CreateText("Components","");
            GUILayoutCommands.StayOnSameLine();
            if(GUIRenderCommands.CreateButton("+","add_new_entty"))
            {
                GUIRenderCommands.SignalPopupCreate("Entity_Add_Component");
            }
            GUIRenderCommands.CreateSeperatorLine();
            GUILayoutCommands.Spacing();

            /*
             * Render popup
             */
            if(GUIRenderCommands.CreatePopup("Entity_Add_Component"))
            {
                RenderAddComponentPopup();
                GUIRenderCommands.FinalizePopup();
            }

            /*
             * Render component layouts
             */
            for(int i=0;i<m_Layouts.Count;i++)
            {
                /*
                 * Create hover state
                 */
                bool componentHeaderHovered = false;

                /*
                 * Get layout
                 */
                ComponentLayout componentLayout = m_Layouts[i];

                /*
                 * Render component header
                 */
                GUIRenderCommands.EnableStyle(ImGuiStyleVar.FrameRounding, 5);
                GUIRenderCommands.CreateSeperatorLine();
                GUILayoutCommands.SetNextItemWidth(GUILayoutCommands.GetTextSize(componentLayout.TargetComponent.GetType().Name).X);
                if(GUIRenderCommands.CreateCollapsingHeader(componentLayout.TargetComponent.GetType().Name, componentLayout.TargetComponent.ID.ToString()))
                {
                    if (GUIEventCommands.IsCurrentItemHavored())
                    {
                        componentHeaderHovered = true;
                    }
                    GUIRenderCommands.DisableStyle();

                    /*
                     * Render component layout
                     */
                    componentLayout.OnLayoutRender();
                }
                else
                {
                    if (GUIEventCommands.IsCurrentItemHavored())
                    {
                        componentHeaderHovered = true;
                    }
                }

                /*
                * Validate component quick menu
                */
                if (componentHeaderHovered && GUIEventCommands.IsMouseRightButtonClicked())
                {
                    m_QuickMenuComponent = componentLayout.TargetComponent;
                    GUIRenderCommands.SignalPopupCreate("Component_Quick_Popup");
                }
            }
            GUIRenderCommands.CreateSeperatorLine();

            /*
             * Render component quick popup
             */
            if(GUIRenderCommands.CreatePopup("Component_Quick_Popup"))
            {
                GUIRenderCommands.CreateImage(m_DeleteIcon,new System.Numerics.Vector2(16,16));
                GUILayoutCommands.StayOnSameLine();
                if(GUIRenderCommands.CreateSelectableItem("Delete","delete_component"))
                {
                    /*
                     * Validate and delete
                     */
                    if (m_QuickMenuComponent.OwnerEntity.DeleteComponent(m_QuickMenuComponent))
                    {
                        needsInvalidation = true;
                    }
                    GUIRenderCommands.TerminateCurrentPopup();
                }
                GUIRenderCommands.FinalizePopup();
            }

            /*
             * Invalidate if requested
             */
            if (needsInvalidation)
                RecreateComponentLayouts();
        }

        private void RecreateComponentLayouts()
        {
            /*
             * Reset quick component target
             */
            m_QuickMenuComponent = null;

            /*
             * Finalize former layouts
             */
            foreach (ComponentLayout layout in m_Layouts)
                layout.OnDetach();

            /*
             * Clear former layouts
             */
            m_Layouts.Clear();

            /*
             * Create layouts
             */
           List<Component> components = m_TargetEntity.Components;

            for (int i = 0; i < components.Count; i++)
            {
                ComponentLayout layout = GUIComponentManager.Current.FetchComponentLayout(components[i].GetType());
                layout.TargetComponent = components[i];
                layout.OnAttach();
                m_Layouts.Add(layout);
            }
        }
        private void RenderAddComponentPopup()
        {
            
            /*
             * Render header
             */
            GUIRenderCommands.CreateText("Add new component","");
            GUIRenderCommands.CreateSeperatorLine();
            GUIRenderCommands.CreateEmptySpace();

            /*
             * Render List components
             */
            for(int componentIndex = 0;componentIndex < m_AllComponentTypes.Count;componentIndex++)
            {
                /*
                 * Render selectable
                 */
                GUIRenderCommands.CreateImage(m_ComponentIcon, new System.Numerics.Vector2(16, 16));
                GUILayoutCommands.StayOnSameLine();
                if(GUIRenderCommands.CreateSelectableItem(m_AllComponentTypes[componentIndex].Name, "cmp_list_" + componentIndex))
                {
                    m_TargetEntity.AddComponent(m_AllComponentTypes[componentIndex]);
                    RecreateComponentLayouts();
                    GUIRenderCommands.TerminateCurrentPopup();
                }
            }
            
        }

        private List<ComponentLayout> m_Layouts;
        private List<Type> m_AllComponentTypes;
        private Entity m_TargetEntity;
        private Texture m_ComponentIcon;
        private Texture m_DeleteIcon;
        private Component m_QuickMenuComponent;
    }
}
