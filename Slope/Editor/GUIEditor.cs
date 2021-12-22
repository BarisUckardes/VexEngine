using Fang.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slope.Windowing;
using Vex.Platform;
using System.IO;

namespace Slope.Editor
{
    internal class GUIEditor
    {

        public GUIEditor(in WindowCreateParams createParameters,in WindowUpdateParams updateParameters)
        {
            /*
             * Create new window
             */
            m_Window = new Slope.Windowing.Window("Slope Launcher",createParameters,updateParameters);

            /*
             *  Set application delegate
             */
            m_Window.SetApplicationEventDelegate(OnReceiveWindowEvent);

            /*
             * Create renderer
             */
            m_Renderer = new GUIRenderer(createParameters.Width, createParameters.Height);

            /*
             * Create editor layout
             */
            string localApplicationPath = PlatformPaths.LocalApplicationData;
            string localApplicationVexPath = localApplicationPath + @"\Vex";
            string localApplicationSlopePath = localApplicationVexPath + @"\Slope";
            string localApplicationSlopeProjectsPath = localApplicationSlopePath + @"\validatedProjectes.txt";

            /*
             * Validate local vex path
             */
            if (!Directory.Exists(localApplicationVexPath))
            {
                Directory.CreateDirectory(localApplicationVexPath);
            }

            /*
             * Validate local slope path
             */
            if (!Directory.Exists(localApplicationSlopePath))
            {
                Directory.CreateDirectory(localApplicationSlopePath);
            }

            /*
             * Validate validated projects file
             */
            if(!File.Exists(localApplicationSlopeProjectsPath))
            {
                File.CreateText(localApplicationSlopeProjectsPath);
            }

            /*
             * Create gui layout
             */
            m_GUILayout = new MainGUILayout(File.ReadAllLines(localApplicationSlopeProjectsPath));

        }
        public void Run()
        {
            /*
             * Initialize
             */
            m_GUILayout.Initialize();

            while (!m_Window.HasWindowCloseRequest)
            {
                if (m_GUILayout.ExitCall)
                    break;
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
            else if (eventData.Type == PlatformEventType.MouseScrolled)
            {
                PlatformMouseScrolledEvent ev = (PlatformMouseScrolledEvent)eventData;
                m_Renderer.MouseScrolled(new OpenTK.Mathematics.Vector2(ev.AmountX, ev.AmountY));
            }
        }


        private Slope.Windowing.Window m_Window;
        private GUIRenderer m_Renderer;
        private MainGUILayout m_GUILayout;
    }
}
