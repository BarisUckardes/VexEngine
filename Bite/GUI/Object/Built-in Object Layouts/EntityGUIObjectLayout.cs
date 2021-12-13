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
                GUIRenderCommands.EnableStyle(ImGuiStyleVar.FrameRounding, 5);
                GUIRenderCommands.CreateSeperatorLine();
                GUILayoutCommands.SetNextItemWidth(GUILayoutCommands.GetTextSize(m_Layouts[i].TargetComponent.GetType().Name).X);
                if(GUIRenderCommands.CreateCollapsingHeader(m_Layouts[i].TargetComponent.GetType().Name,m_Layouts[i].TargetComponent.ID.ToString()))
                {
                    GUIRenderCommands.DisableStyle();
                    m_Layouts[i].OnLayoutRender();
                }
            }
            GUIRenderCommands.CreateSeperatorLine();

        }

        private void RecreateComponentLayouts()
        {
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
    }
}
