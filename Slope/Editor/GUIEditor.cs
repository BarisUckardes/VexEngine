using Fang.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slope.Windowing;
using Vex.Platform;

namespace Slope.Editor
{
    internal class GUIEditor
    {

        public GUIEditor(in WindowCreateParams createParameters,in WindowUpdateParams updateParameters)
        {
            m_Window = new Slope.Windowing.Window("Slope Launcher",createParameters,updateParameters);
            m_Window.SetApplicationEventDelegate(OnReceiveWindowEvent);
            m_Renderer = new GUIRenderer(createParameters.Width, createParameters.Height);
            m_GUILayout = new EditorGUILayout();
        }
        public void Run()
        {
            /*
             * Initialize
             */
            m_GUILayout.Initialize();

            while (!m_Window.HasWindowCloseRequest)
            {
                /*
                 * Update input
                 */
                m_Window.UpdateInput();

                /*
                 * Clear window
                 */

                /*
                 * Start rendering gui
                 */
                m_Renderer.Begin(m_Window, 1.0f / 60.0f, new OpenTK.Mathematics.Vector2(m_Window.Width, m_Window.Height));

                /*
                 * Render gui
                 */
                m_GUILayout.Render();

                /*
                 * End rendering
                 */
                m_Renderer.Finalize();

                /*
                 * Swap buffers
                 */
                m_Window.SwapBuffers();
            }

            /*
             * Finalize
             */
            m_GUILayout.Finalize();
        }


        public void OnReceiveWindowEvent(PlatformEvent eventData)
        {
            if (eventData.Type == PlatformEventType.KeyChar)
            {
                PlatformKeyCharEvent ev = (PlatformKeyCharEvent)eventData;
                m_Renderer.PressChar((char)ev.KeyCode);
            }
            else if (eventData.Type == PlatformEventType.MouseScVexlled)
            {
                PlatformMouseScrolledEvent ev = (PlatformMouseScrolledEvent)eventData;
                m_Renderer.MouseScVexll(new OpenTK.Mathematics.Vector2(ev.AmountX, ev.AmountY));
            }
        }


        private Slope.Windowing.Window m_Window;
        private GUIRenderer m_Renderer;
        private EditorGUILayout m_GUILayout;
    }
}
