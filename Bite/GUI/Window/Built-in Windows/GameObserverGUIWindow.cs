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
            m_PrimaryObserver = ObserverComponent.PrimalObserver;
            m_PrimaryFramebuffer = m_PrimaryObserver.Framebuffer;
        }

        public override void OnLayoutFinalize()
        {
            m_PrimaryObserver = null;
        }

        public override void OnRenderLayout()
        {
            /*
             * Draw game viewport
             */
            Vector2 windowAnchor = GUILayoutCommands.GetCursor();
            Vector2 windowSize = GUILayoutCommands.GetCurrentWindowSize();
            Vector2 halfWindowSize = windowSize / 2.0f;
            float textureWidth = windowSize.X*0.8f;
            float textureHeight = windowSize.Y * 0.8f;
            float halfTextureWidth = textureWidth / 2.0f;
            float helfTextureHeight = textureHeight / 2.0f;
            float offsetX = halfWindowSize.X - halfTextureWidth;
            float offsetY = halfWindowSize.Y - helfTextureHeight;
            GUILayoutCommands.SetCursorPos(windowAnchor + new Vector2(offsetX,offsetY));

            /*
             * Set camera aspect ratio
             */
            m_PrimaryObserver.AspectRatio = textureWidth / textureHeight;

            /*
             * Create flipped uvs
             */
            Vector2 uv0 = new Vector2(0, 1);
            Vector2 uv1 = new Vector2(1, 0);

            /*
             * Render framebuffer image
             */
            GUIRenderCommands.CreateImage(m_PrimaryFramebuffer.BackTexture, new Vector2(textureWidth, textureHeight),uv0,uv1);
        }

        public override void OnVisible()
        {

        }


        private ObserverComponent m_PrimaryObserver;
        private Framebuffer m_PrimaryFramebuffer;
    }
}
