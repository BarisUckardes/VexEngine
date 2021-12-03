using Bite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    /// <summary>
    /// A static class for gui window management
    /// </summary>
    public static class GUIWindow
    {

        /// <summary>
        /// Creates a new window out of the target type
        /// </summary>
        /// <param name="type"></param>
        public static void CreateWindow(Type type)
        {
            /*
             * Validate type correction
             */
            if(type.IsSubclassOf(typeof(WindowGUILayout)))
            {
                /*
                 * Create new layout
                 */
                WindowGUILayout layout = Activator.CreateInstance(type) as WindowGUILayout;

                /*
                 * Set editor session
                 */
                layout.SetEditorSession(s_Session);

                /*
                 * On layout begin
                 */
                layout.OnLayoutBegin();

                /*
                 //* Register window
                 */
                s_Windows.Add(layout);
            }
               
        }

        /// <summary>
        /// An internal create window which used for inital window creation
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        internal static void CreateWindow(Type type,in Guid id)
        {
            /*
             * Validate type correction
             */
            if (type.IsSubclassOf(typeof(WindowGUILayout)))
            {
                /*
                 * Create new layout
                 */
                WindowGUILayout layout = Activator.CreateInstance(type) as WindowGUILayout;

                /*
                 * Set internal id
                 */
                layout.ID = id;

                /*
                 * Set editor session
                 */
                layout.SetEditorSession(s_Session);

                /*
                 * On layout begin
                 */
                layout.OnLayoutBegin();

                /*
                 //* Register window
                 */
                s_Windows.Add(layout);
            }

        }

        /// <summary>
        /// Returns all the active window layouts
        /// </summary>
        internal static List<WindowGUILayout> WindowLayouts
        {
            get
            {
                return s_Windows;
            }
        }

        /// <summary>
        /// Initializes this static class
        /// </summary>
        /// <param name="session"></param>
        /// <param name="initialLayouts"></param>
        internal static void Initialize(EditorSession session,List<WindowLayoutCreateParams> initialLayouts)
        {
            /*
             * Initialize variables
             */
            s_Windows = new List<WindowGUILayout>();
            s_Session = session;

            /*
             * Create intial windows
             */
            foreach(WindowLayoutCreateParams createParameter in initialLayouts)
            {
                CreateWindow(createParameter.LayoutType, createParameter.ID);
            }
            
        }

        /// <summary>
        /// Removes an active window
        /// </summary>
        /// <param name="layout"></param>
        internal static void RemoveWindow(WindowGUILayout layout)
        {
            layout.OnLayoutFinalize();
            s_Windows.Remove(layout);
        }

        /// <summary>
        /// Shutdowns the windows and returns the last active windows
        /// </summary>
        /// <returns></returns>
        internal static List<WindowLayoutSettings> Shutdown()
        {
            /*
             * Finalize layouts
             */
            for(int windowIndex = 0;windowIndex < s_Windows.Count;windowIndex++)
            {
                s_Windows[windowIndex]?.OnLayoutFinalize();
            }

            /*
             * Generate layout settings
             */
            List<WindowLayoutSettings> settings = new List<WindowLayoutSettings>();
            for(int settingIndex = 0; settingIndex < s_Windows.Count;settingIndex++)
            {
                /*
                 * Get layout
                 */
                WindowGUILayout layout = s_Windows[settingIndex];

                /*
                 * Create and register settings
                 */
                settings.Add(new WindowLayoutSettings(layout.GetType().Assembly.FullName, layout.GetType().Name, layout.ID));
            }


            /*
             * Clear layouts
             */
            s_Windows.Clear();
            
            /*
             * Set session null
             */
            s_Session = null;

            return settings;
        }
        private static List<WindowGUILayout> s_Windows;
        private static EditorSession s_Session;

    }
}
