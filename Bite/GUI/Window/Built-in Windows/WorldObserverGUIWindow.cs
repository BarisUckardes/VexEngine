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

        }
        public override void OnInVisible()
        {

        }
        public override void OnLayoutBegin()
        {
            m_TargetWorld = Session.CurrentWorld;
            m_EntityIcon = Session.GetEditorResource("EntityIcon", AssetType.Texture2D) as Texture2D;
        }

        public override void OnLayoutFinalize()
        {
            m_TargetWorld = null;

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
            if (m_TargetWorld != null)
                RenderWorldView(m_TargetWorld);
            else
                GUIRenderCommands.CreateText("No world to be observed","w");
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
                /*
                 * if current item clicked
                 */
                if(GUIEventCommands.IsCurrentItemClicked())
                {
                    GUIObject.SignalNewObject(world);
                }

                for (int entityIndex = 0; entityIndex < entities.Length; entityIndex++)
                {
                    /*
                     * Get entity
                     */
                    Entity entity = entities[entityIndex];

                    /*
                     * Render image
                     */
                    Vector2 anchorPosition = GUILayoutCommands.GetCursor();
                    GUIRenderCommands.CreateImage(m_EntityIcon, new System.Numerics.Vector2(16, 16));

                    /*
                     * Render selectable
                     */
                    GUILayoutCommands.SetCursorPos(anchorPosition + new Vector2(20, 0));
                    if (GUIRenderCommands.CreateSelectableItem(entity.Name, entity.ID.ToString()))
                    {
                        m_SelectEntity = entity;
                        GUIObject.SignalNewObject(entity);
                    }

                    /*
                     * Catch selected entity
                     */
                    if(GUIEventCommands.IsCurrentItemHavored() && GUIEventCommands.IsMouseRightButtonClicked())
                    {
                        m_SelectEntity = entity;
                        GUIRenderCommands.SignalPopupCreate("World_Create_Entity_Popup");
                    }
                  
                }
            }

            /*
            * if current item clicked
            */
            if (GUIEventCommands.IsCurrentItemClicked())
            {
                GUIObject.SignalNewObject(world);
            }

            /*
             * Create world create menu popup
             */
            if (GUIEventCommands.IsWindowHovered() && !GUIEventCommands.IsAnyItemHavored() && GUIEventCommands.IsMouseRightButtonClicked())
            {
                GUIRenderCommands.SignalPopupCreate("World_Create_Menu");
            }

            /*
             * Render world create menu
             */
            if (GUIRenderCommands.CreatePopup("World_Create_Menu"))
            {
                RenderCreateContextPopup();
                GUIRenderCommands.FinalizePopup();
            }

            /*
             * Render entity popup
             */
            bool isRenameEntity = false;
            if(GUIRenderCommands.CreatePopup("World_Create_Entity_Popup"))
            {
                RenderEntityPopup(ref isRenameEntity);
                GUIRenderCommands.FinalizePopup();
            }

            /*
             * Signal rename popup
             */
            if (isRenameEntity)
                GUIRenderCommands.SignalPopupCreate("World_Rename_Entity_Popup");

            /*
             * Render entity rename
             */
            if(GUIRenderCommands.CreatePopup("World_Rename_Entity_Popup"))
            {
                RenderEntityRenamePopup();
                GUIRenderCommands.FinalizePopup();
            }

        }

        private void RenderCreateContextPopup()
        {
            if(GUIRenderCommands.CreateMenu("Create", "world_create_menu"))
            {
                if(GUIRenderCommands.CreateMenuItem("Entity","w_create_entity"))
                {
                    Entity newEntity = new Entity("New Entity", m_TargetWorld);
                    GUIRenderCommands.TerminateCurrentPopup();
                }
                GUIRenderCommands.FinalizeMenu();
            }
        }
        private void RenderEntityPopup(ref bool isRenameEntity)
        {
            if (GUIRenderCommands.CreateMenuItem("Delete", "w_delete_entity"))
            {
                m_SelectEntity?.Destroy();
            }
            if(GUIRenderCommands.CreateMenuItem("Rename", "w_delete_entity"))
            {
                isRenameEntity = true;
            }
        }
        private void RenderEntityRenamePopup()
        {
            /*
             * Render header
             */
            GUIRenderCommands.CreateText("Rename entity", "");

            /*
             * Render input text
             */
            GUIRenderCommands.CreateTextInput("", "w_i_t", ref m_EntityRenameInputText);

            /*
             * Render button
             */
            if(GUIRenderCommands.CreateButton("Rename","w_e_r"))
            {
                m_SelectEntity.Name = m_EntityRenameInputText;
                m_EntityRenameInputText = string.Empty;
                GUIRenderCommands.TerminateCurrentPopup();
            }
        }

        private Entity m_SelectEntity;
        private Texture2D m_EntityIcon;
        private World m_TargetWorld;
        private string m_EntityRenameInputText = string.Empty;
    }
}
