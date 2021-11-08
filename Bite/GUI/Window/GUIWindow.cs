using Bite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    public static class GUIWindow
    {
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
                layout.Session = s_Session;

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

        internal static List<WindowGUILayout> WindowLayouts
        {
            get
            {
                return s_Windows;
            }
        }
        internal static void Initialize(EditorSession session)
        {
            s_Windows = new List<WindowGUILayout>();
            s_Session = session;
        }

        internal static void RemoveWindow(WindowGUILayout layout)
        {
            layout.OnLayoutFinalize();
            s_Windows.Remove(layout);
        }
        internal static void Shutdown()
        {
            for(int windowIndex = 0;windowIndex < s_Windows.Count;windowIndex++)
            {
                s_Windows[windowIndex]?.OnLayoutFinalize();
            }
            s_Windows.Clear();

            s_Session = null;
        }
        private static List<WindowGUILayout> s_Windows;
        private static EditorSession s_Session;

    }
}
