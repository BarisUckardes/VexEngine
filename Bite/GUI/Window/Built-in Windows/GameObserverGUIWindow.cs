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
    public sealed class GameObserverGUIWindow : WindowGUILayout
    {
        public override void OnInVisible()
        {

        }

        public override void OnLayoutBegin()
        {
            m_PrimaryObserver = ObserverComponent.PrimalObserver.Framebuffer;
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
            float textureSize = MathF.Min(windowSize.X, windowSize.Y)*0.8f;
            float halfTextureSize = textureSize / 2.0f;
            float offsetX = halfWindowSize.X - halfTextureSize;
            float offsetY = halfWindowSize.Y - halfTextureSize;
            GUILayoutCommands.SetCursorPos(windowAnchor + new Vector2(offsetX,offsetY));

            /*
             * Create flipped uvs
             */
            Vector2 uv0 = new Vector2(0, 0);
            Vector2 uv1 = new Vector2(1, 1);
            GUIRenderCommands.CreateImage(m_PrimaryObserver.BackTexture, new Vector2(textureSize, textureSize));
        }

        public override void OnVisible()
        {

        }


        private Framebuffer m_PrimaryObserver;
    }
}
