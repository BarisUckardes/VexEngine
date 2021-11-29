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
            Vector2 windowSize = GUILayoutCommands.GetCurrentWindowSize();
            GUILayoutCommands.SetCursorPos(GUILayoutCommands.GetCursor() + new Vector2(windowSize.X/2.0f - 32,0));
            if(Session.GamePlayState)
            {
                if(GUIRenderCommands.CreateImageButton(m_GameStopButtonTexture,"Stop_buttn", new Vector2(64, 64), uv0, uv1))
                {
                    Console.WriteLine("Game stop");
                    Session.StopGamePlaySession();
                }
            }
            else
            {
                if(GUIRenderCommands.CreateImageButton(m_GamePlayButtonTexture,"Play_buttn", new Vector2(64, 64), uv0, uv1))
                {
                    Console.WriteLine("Game play");
                    Session.StartGamePlaySession();
                }
            }
            windowSize -= new Vector2(0, 64);

            /*
             * Draw game viewport
             */
            Vector2 windowAnchor = GUILayoutCommands.GetCursor();
            Vector2 halfWindowSize = windowSize / 2.0f;
            float textureWidth = windowSize.X*1.0f;
            float textureHeight = (windowSize.Y * 1.0f);
            float halfTextureWidth = textureWidth / 2.0f;
            float helfTextureHeight = textureHeight / 2.0f;
            float offsetX = halfWindowSize.X - halfTextureWidth;
            float offsetY = halfWindowSize.Y - helfTextureHeight;
            GUILayoutCommands.SetCursorPos(windowAnchor + new Vector2(offsetX,offsetY));

            /*
             * Set camera aspect ratio
             */
            if(m_PrimaryObserver!=null)
                m_PrimaryObserver.AspectRatio = textureWidth / textureHeight;

            /*
             * Render framebuffer image
             */
            GUIRenderCommands.CreateImage(m_PrimaryFramebuffer == null ? null : m_PrimaryFramebuffer.BackTexture, new Vector2(textureWidth, textureHeight-40),uv0,uv1);
        }

        public override void OnVisible()
        {

        }

        private void ValidateAndRefreshObserver()
        {
            if(m_PrimaryObserver != ObserverComponent.PrimalObserver)
            {
                m_PrimaryObserver = ObserverComponent.PrimalObserver;
                m_PrimaryFramebuffer = m_PrimaryObserver != null ? m_PrimaryObserver.Framebuffer : null;
            }
        }
        private ObserverComponent m_PrimaryObserver;
        private Framebuffer m_PrimaryFramebuffer;
        private Texture2D m_GamePlayButtonTexture;
        private Texture2D m_GameStopButtonTexture;
    }
}
