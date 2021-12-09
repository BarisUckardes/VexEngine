using Bite.Core;
using Fang.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Vex.Graphics;
namespace Bite.GUI
{
    [WindowLayout("Free Game Observer")]
    public sealed class FreeGameObserverGUIWindow : WindowGUILayout
    {
        public override void OnInVisible()
        {

        }

        public override void OnLayoutBegin()
        {
            CreateNewObserver();
        }

        public override void OnLayoutFinalize()
        {
            if (m_Observer != null && m_Observer.OwnerEntity != null)
                m_Observer.OwnerEntity.Destroy();
                
            m_Observer = null;
        }

        public override void OnRenderLayout()
        {
            /*
             * Test observer
             */
            if(Session.CurrentWorld != m_Observer.OwnerEntity.World)
            {
                CreateNewObserver();
            }

            /*
            * Create flipped uvs
            */
            Vector2 uv0 = new Vector2(0, 1);
            Vector2 uv1 = new Vector2(1, 0);

            /*
             * Draw game viewport
             */
            Vector2 windowSize = GUILayoutCommands.GetCurrentWindowSize();
            Vector2 windowAnchor = GUILayoutCommands.GetCursor();
            float textureWidth = windowSize.X;
            float textureHeight = windowSize.Y-30;

            /*
             * Calculate aspect ratio
             */
            float aspectRatio = textureWidth / textureHeight;
           
            /*
             * Render framebuffer image
             */
            if(m_Observer != null)
            {
                /*
                 * Adjust aspect ratio
                 */
                m_Observer.AspectRatio = aspectRatio;

                /*
                 * Render frambuffer image
                 */
                GUIRenderCommands.CreateImage(m_Observer.Framebuffer.BackTexture, new Vector2(textureWidth, textureHeight), uv0, uv1);

                /*
                 * Enable disable viewport transform
                 */
                if(GUIEventCommands.IsCurrentItemHavored() && GUIEventCommands.IsMouseRightButtonClicked() && !m_ViewportTransformActive)
                {
                    //Console.WriteLine("Viewport transform enabled");
                    m_LastMousePosition = GUIEventCommands.GetMousePosition();
                    m_ActivationPosition = GUIEventCommands.GetMousePosition();
                    m_ViewportTransformActive = true;
                    Session.MouseCursorLocked = true;
                    Session.MouseCursorVisible = false;
                }
                else if(m_ViewportTransformActive && GUIEventCommands.IsRightButtonReleased())
                {
                   // Console.WriteLine("Viewport transfor disabled");
                    m_ViewportTransformActive = false;
                    Session.MouseCursorLocked = false;
                    Session.MouseCursorVisible = true;
                    Session.MousePosition = m_ActivationPosition;
                }   
            }

            /*
             * Process viewport transform
             */
            if(m_ViewportTransformActive)
            {
                /*
                 * Catch w-a-s-d movement
                 */
                if (GUIEventCommands.IsKeyDown(Vex.Input.Keys.W))
                {
                    m_Observer.Spatial.Position += m_Observer.Spatial.Forward * 0.01f * 5;
                }
                if (GUIEventCommands.IsKeyDown(Vex.Input.Keys.S))
                {
                    m_Observer.Spatial.Position += m_Observer.Spatial.Backward * 0.01f * 5;
                }
                if (GUIEventCommands.IsKeyDown(Vex.Input.Keys.A))
                {
                    m_Observer.Spatial.Position += m_Observer.Spatial.Right * 0.01f * 5;
                }
                if (GUIEventCommands.IsKeyDown(Vex.Input.Keys.D))
                {
                    m_Observer.Spatial.Position += m_Observer.Spatial.Left * 0.01f * 5;
                }

                /*
                 * Catch rotation movement
                 */
                Vector2 currentMousePosition = GUIEventCommands.GetMousePosition();
                Vector2 mousePositionDelta = GUIEventCommands.GetMousePosition() - m_LastMousePosition;
                m_Observer.Spatial.Rotation += new Vector3(mousePositionDelta.X,-mousePositionDelta.Y,0)*0.1f;

                /*
                 * Set cached positions
                 */
                m_Position = m_Observer.Spatial.Position;
                m_Rotation = m_Observer.Spatial.Rotation;
                m_LastMousePosition = currentMousePosition;

                /*
                 * 
                 */
                float tiledX = currentMousePosition.X;
                float tiledY = currentMousePosition.Y;
                bool swap = false;
                if(currentMousePosition.X + mousePositionDelta.X >= Session.WindowWidth)
                {
                    tiledX = 0;
                    swap = true;
                }
                else if (currentMousePosition.X + mousePositionDelta.X < 0)
                {
                    tiledX = Session.WindowWidth;
                    swap = true;
                }

                if (currentMousePosition.Y + mousePositionDelta.Y >= Session.WindowHeight)
                {
                    tiledY = 0;
                    swap = true;
                }
                if (currentMousePosition.Y + mousePositionDelta.Y < 0)
                {
                    tiledY = Session.WindowHeight-5;
                    swap = true;
                }

                if (swap)
                {
                    Vector2 tiledPosition = new Vector2(tiledX, tiledY);
                    Session.MousePosition = tiledPosition;
                    m_LastMousePosition = tiledPosition;
                }
                
            }
        }

        public override void OnVisible()
        {

        }

        private void CreateNewObserver()
        {
            Console.WriteLine("New free game observer");
            /*
             * Validate and destroy former entity
             */
            if(m_Observer != null)
            {
                Session.CurrentWorld.GetEntityViaID(m_EntityID).Destroy();
            }

            Entity observerEntity = new Entity("Free game observer", Session.CurrentWorld);
            m_Observer = observerEntity.AddComponent<FreeGameObserver>();
            m_Observer.Spatial.Position = m_Position;
            m_Observer.Spatial.Rotation = m_Rotation;
            m_EntityID = observerEntity.ID;
        }

        private FreeGameObserver m_Observer;
        private Guid m_EntityID;
        private Vector3 m_Position;
        private Vector3 m_Rotation;
        private Vector2 m_LastMousePosition;
        private Vector2 m_ActivationPosition;
        private bool m_ViewportTransformActive;
    }
}
