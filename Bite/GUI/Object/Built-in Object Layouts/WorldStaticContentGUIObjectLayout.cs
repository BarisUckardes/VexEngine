using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Fang.Commands;
namespace Bite.GUI
{
    [ObjectLayout(typeof(StaticWorldContent))]
    public sealed class WorldStaticContentGUIObjectLayout : ObjectLayout
    {
        public override void OnAttach()
        {
            m_WorldContent = Object as StaticWorldContent;
        }

        public override void OnDetach()
        {
            m_WorldContent = null;
        }

        public override void OnLayoutRender()
        {
            /*
             * Render Entity Count
             */
            GUIRenderCommands.CreateText("Total Entities: " + m_WorldContent.EntityCount, " ");
            GUIRenderCommands.CreateText("Total Assets: " + m_WorldContent.AssetCount, " ");
            GUIRenderCommands.CreateText("Total Component Types: " + m_WorldContent.ComponentTypeCount, " ");
            GUIRenderCommands.CreateText("Total Components: " + m_WorldContent.ComponentCount," ");
            GUIRenderCommands.CreateText("Total Views: " + m_WorldContent.ViewCount, " ");
            GUIRenderCommands.CreateText("Total Resolvers: " + m_WorldContent.ResolverCount, " ");
        }

        private StaticWorldContent m_WorldContent;
    }
}
