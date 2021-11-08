using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Application;
using Vex.Framework;
using Fang.Commands;
namespace Bite.GUI
{
    public sealed class WorldObserverGUIWindow : WindowGUILayout
    {
        public override void OnVisible()
        {
            Console.WriteLine("Visible");
        }
        public override void OnInVisible()
        {
            Console.WriteLine("Invisible");
        }
        public override void OnLayoutBegin()
        {
            m_TargetWorld = Session.Worlds.ElementAt(0);
        }

        public override void OnLayoutFinalize()
        {
            m_TargetWorld = null;
            Console.WriteLine("Finalized");
        }

        public override void OnRenderLayout()
        {
            RenderWorldView(m_TargetWorld);
        }

        private void RenderWorldView(World world)
        {
            /*
             * Get entities
             */
            Entity[] entities = world.GetView<WorldLogicView>().Entities;

            /*
             * Display each entity
             */
            for(int i=0;i<entities.Length;i++)
            {
                if (GUIRenderCommands.CreateTreeNode(entities[i].Name,entities[i].ID.ToString()))
                {
                    /*
                     * Check if this item clicked
                     */
                    
                    GUIRenderCommands.FinalizeTreeNode();
                }
                if (GUIEventCommands.IsCurrentItemClicked())
                {
                    GUIObject.SignalNewObject(entities[i]);
                }

            }
        }
        private World m_TargetWorld;
    }
}
