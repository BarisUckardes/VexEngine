using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;
using Fang.Commands;
using System.Numerics;
namespace Bite.GUI
{
    [WindowLayout("Game Observer")]
    public sealed class GameObserverGUIWindow : WindowGUILayout
    {
        public override void OnInVisible()
        {

        }

        public override void OnLayoutBegin()
        {
            /*
             * Get resources
             */
            m_GamePlayButtonTexture = Session.GetEditorResource("GamePlayButtonIcon",Vex.Asset.AssetType.Texture2D) as Texture2D;
            m_GameStopButtonTexture = Session.GetEditorResource("GameStopButtonIcon", Vex.Asset.AssetType.Texture2D) as Texture2D;
            m_PrimaryFramebuffer = Framebuffer2D.IntermediateFramebuffer;
        }


        public override void OnLayoutFinalize()
        {
            m_PrimaryObserver = null;
        }

        public override void OnRenderLayout()
        {
            /*
             * Validate observer
             */
            ValidateAndRefreshObserver();

            /*
             * Create flipped uvs
             */
            Vector2 uv0 = new Vector2(0, 1);
            Vector2 uv1 = new Vector2(1, 0);

            /*
             * Render buttons
             */
            Vector2 availableSpace = GUILayoutCommands.GetAvailableSpace();
            GUILayoutCommands.SetCursorPos(GUILayoutCommands.GetCursor() + new Vector2(availableSpace.X/2.0f - 32,0));
            if(Session.GamePlayState)
            {
                if(GUIRenderCommands.CreateImageButton(m_GameStopButtonTexture,"Stop_buttn", new Vector2(64, 64), uv0, uv1))
                {
                    Session.StopGamePlaySession();
                }
            }
            else
            {
                if(GUIRenderCommands.CreateImageButton(m_GamePlayButtonTexture,"Play_buttn", new Vector2(64, 64), uv0, uv1))
                {
                    Session.StartGamePlaySession();
                }
            }




            /*
             * Set camera aspect ratio
             */
            availableSpace = GUILayoutCommands.GetAvailableSpace();
            if (m_PrimaryObserver!=null)
                m_PrimaryObserver.AspectRatio = availableSpace.X / availableSpace.Y;


            /*
             * Validate aspect ratio
             */
            if(m_PrimaryObserver!= null && m_PrimaryObserver.AspectRatio < 0)
            {
                m_PrimaryObserver.AspectRatio = 1.0f;
                
            }

            

            /*
             * Render framebuffer image
             */
            GUIRenderCommands.CreateImage(m_PrimaryFramebuffer.BackTexture, availableSpace, uv0,uv1);

            /*
             * Catch game input
             */
            if (GUIEventCommands.IsWindowFocused())
            {
                Session.HandleInputs = false;
            }
            else
            {
                Session.HandleInputs = true;
            }


        }

        public override void OnVisible()
        {

        }

        private void ValidateAndRefreshObserver()
        {
            if(m_PrimaryObserver != ObserverComponent.PrimalObserver)
            {
                m_PrimaryObserver = ObserverComponent.PrimalObserver;
                //m_PrimaryFramebuffer = m_PrimaryObserver != null ? m_PrimaryObserver.Framebuffer as Framebuffer2D : null;
            }
        }

        private ObserverComponent m_PrimaryObserver;
        private Framebuffer2D m_PrimaryFramebuffer;
        private Texture2D m_GamePlayButtonTexture;
        private Texture2D m_GameStopButtonTexture;

    }
}
