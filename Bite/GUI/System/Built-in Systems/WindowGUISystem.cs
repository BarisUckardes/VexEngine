using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
using System.Reflection;
using ImGuiNET;
using System.Numerics;
using Bite.Core;
using Vex.Platform;
using System.IO;
using YamlDotNet.Serialization;

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
             * Collect window types across the appdomain
             */
            List<Type> validWindowLayoutTypes = new List<Type>();
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(WindowGUILayout)))
                        validWindowLayoutTypes.Add(type);
                }
            }

            /*
            * Validate layout file
            */
            string layoutPath = PlatformPaths.DomainRootDirectoy + @"\Project Settings\EditorLayout.vsettings";
            List<WindowLayoutCreateParams> layoutCreateParams = new List<WindowLayoutCreateParams>();
            if (File.Exists(layoutPath))
            {
                /*
                * Load layout
                */
                string yamlContent = File.ReadAllText(layoutPath);
                List<WindowLayoutSettings> layouts = new DeserializerBuilder().Build().Deserialize<List<WindowLayoutSettings>>(yamlContent);
                Console.WriteLine("Read intial layout count: " + layouts.Count);
                for (int layoutIndex = 0; layoutIndex < layouts.Count; layoutIndex++)
                {
                    /*
                     * Find type
                     */
                    foreach (Type type in validWindowLayoutTypes)
                        if (type.Name == layouts[layoutIndex].TypeName)
                            layoutCreateParams.Add(new WindowLayoutCreateParams(type, layouts[layoutIndex].ID));
                }
            }
            else
            {
                Console.WriteLine("Editor layout file dont exists on path: " + layoutPath);
            }

            /*
             * Set pending window list reference to GUIWindow class
             */
            GUIWindow.Initialize(Session,layoutCreateParams);
        }

        public override void OnDetach()
        {
            /*
            * Shutdown GUI window static class and collect final window settings
            */
            List<WindowLayoutSettings> finalLayouts = GUIWindow.Shutdown();

            /*
            * Validate or create new editor layout settings
            */
            string layoutPath = PlatformPaths.DomainRootDirectoy + @"\Project Settings\EditorLayout.vsettings";

            string yamlContent = new SerializerBuilder().Build().Serialize(finalLayouts);
            File.WriteAllText(layoutPath, yamlContent);

            Console.WriteLine("layout detach");

           
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
                isVisible = GUIRenderCommands.CreateWindow(layout.GetType().Name, layout.ID.ToString(),ref isExitRequested);

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
