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
using Vex.Graphics;
using System.Numerics;

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
            m_TargetWorld = Session.CurrentWorld;
            m_EntityIcon = Session.GetEditorResource("EntityIcon", AssetType.Texture2D) as Texture2D;
        }

        public override void OnLayoutFinalize()
        {
            m_TargetWorld = null;
            Console.WriteLine("Finalized");
        }

        public override void OnRenderLayout()
        {
            /*
             * Set current world
             */
            m_TargetWorld = Session.CurrentWorld;

            /*
             * Validate and render world
             */
            if(m_TargetWorld != null)
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
                    /*
                     * Render image
                     */
                    Vector2 anchorPosition = GUILayoutCommands.GetCursor();
                    GUIRenderCommands.CreateImage(m_EntityIcon, new System.Numerics.Vector2(16, 16));

                    /*
                     * Render selectable
                     */
                    GUILayoutCommands.SetCursorPos(anchorPosition + new Vector2(20, 0));
                    if (GUIRenderCommands.CreateSelectableItem(entities[i].Name, entities[i].ID.ToString()))
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
                Session.UpdateDomainAsset(m_TargetWorld.ID, m_TargetWorld);
            }
           
        }
        private Texture2D m_EntityIcon;
        private World m_TargetWorld;
    }
}
