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
                s_Windows.Add(Activator.CreateInstance(type) as WindowGUILayout);
        }
        internal static void SetListInternal(List<WindowGUILayout> windowRef)
        {
            s_Windows = windowRef;
        }

        private static List<WindowGUILayout> s_Windows;
    }
}
