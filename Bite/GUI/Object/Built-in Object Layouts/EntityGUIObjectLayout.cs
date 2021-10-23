using Vex.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
namespace Bite.GUI
{
    [ObjectLayout(typeof(Entity))]
    public sealed class EntityGUIObjectLayout : ObjectLayout
    {
        public override void OnAttach()
        {
            m_TargetEntity = Object as Entity;
            m_Layouts = new List<ComponentLayout>();

            /*
             * Create layouts
             */
            Component[] components = m_TargetEntity.Components;

            for(int i=0;i<components.Length;i++)
            {
                ComponentLayout layout = GUIComponent.FetchComponentLayout(components[i].GetType());
                layout.TargetComponent = components[i];
                layout.OnAttach();
                m_Layouts.Add(layout);
            }
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
            if(m_TargetEntity == null)
            {
                GUIRenderCommands.CreateText("This entity seems null","");
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
             * Spacing between entity name and components
             */
            GUILayoutCommands.Spacing();

            GUIRenderCommands.CreateText("Components","");
            GUIRenderCommands.CreateSeperatorLine();
            GUILayoutCommands.Spacing();

            /*
             * Render component layouts
             */
            for(int i=0;i<m_Layouts.Count;i++)
            {
                if(GUIRenderCommands.CreateCollapsingHeader(m_Layouts[i].TargetComponent.GetType().Name,m_Layouts[i].TargetComponent.ID.ToString()))
                {
                    m_Layouts[i].OnLayoutRender();
                }
            }

        }

        private List<ComponentLayout> m_Layouts;
        private Entity m_TargetEntity;
    }
}
