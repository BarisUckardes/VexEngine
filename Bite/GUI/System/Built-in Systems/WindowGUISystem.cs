using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
using System.Reflection;
using ImGuiNET;
using System.Numerics;

namespace Bite.GUI
{
  
    public sealed class WindowGUISystem : GUISystem
    {
        public WindowGUISystem()
        {

        }
        public override void OnAttach()
        {
            /*
             * Set pending window list reference to GUIWindow class
             */
            GUIWindow.Initialize(Session);
        }

        public override void OnDetach()
        {
            GUIWindow.Shutdown();
        }

        public override void OnUpdate()
        {
            /*
             * Create finalized windows
             */
            List<WindowGUILayout> layouts = GUIWindow.WindowLayouts;
            List<WindowGUILayout> finalizedLayouts = new List<WindowGUILayout>();

            for(int windowIndex = 0;windowIndex < layouts.Count;windowIndex++ )
            {
                /*
                 * Get layout
                 */
                WindowGUILayout layout = layouts[windowIndex];

                /*
                 * Intialize window state
                 */
                bool isExitRequested = false;
                bool isVisible = false;

                /*
                 * Draw window
                 */
                WindowLayoutAttribute layoutAttribute = (WindowLayoutAttribute)layout.GetType().GetCustomAttribute<WindowLayoutAttribute>();
                isVisible = GUIRenderCommands.CreateWindow(layoutAttribute != null ? layoutAttribute.WindowName : layout.GetType().Name,"",ref isExitRequested);

                /*
                 * Check if window is visible
                 */
                if(isVisible && !isExitRequested)
                {
                    /*
                     * Set visiblity invokes
                     */
                    if(!layout.IsVisible)
                    {
                        layout.OnVisible();
                    }

                    /*
                     * Render layout
                     */
                    layout.OnRenderLayout();

                    /*
                     * Set layout visibility state
                     */
                    layout.IsVisible = true;
                }
                else if (!isVisible && !isExitRequested)
                {
                    /*
                     * Set visibility invokes
                     */
                    if(layout.IsVisible)
                    {
                        layout.OnInVisible();
                    }

                    /*
                     * Set visiblity state
                     */
                    layout.IsVisible = false;
                }
                else if(isExitRequested)
                {
                    /*
                     * Remove window
                     */
                    finalizedLayouts.Add(layout);
                }

                /*
                 * Finalize window
                 */
                GUIRenderCommands.FinalizeWindow();
            }

            /*
             * Finalize windows
             */
            for(int windowIndex = 0;windowIndex < finalizedLayouts.Count;windowIndex++)
            {
                GUIWindow.RemoveWindow(finalizedLayouts[windowIndex]);
            }
            finalizedLayouts.Clear();
        }

      
    }
}
