using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Application;
using Vex.Framework;
using Fang.Commands;
using Vex.Asset;
using System.IO;

namespace Bite.GUI
{
    [WindowLayout("World Observer")]
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
            if(GUIRenderCommands.CreateCollapsingHeader(m_TargetWorld.Name,"world_"+m_TargetWorld.ID.ToString()))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    if (GUIRenderCommands.CreateTreeNode(entities[i].Name, entities[i].ID.ToString()))
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

            /*
             * Render save button
             */
            if(GUIRenderCommands.CreateButton("Save","save_world_btn"))
            {
                //Session.UpdateDomainAsset(m_TargetWorld.ID, m_TargetWorld);

                AssetInterface assetInterface = new AssetInterface(null);
                string worldYaml = assetInterface.GenerateObjectString(AssetType.World, m_TargetWorld);
                File.WriteAllText(@"C:\Users\baris\Desktop\FolderTest\worldtest.vasset",worldYaml);
            }
        }
        private World m_TargetWorld;
    }
}
