using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    /// <summary>
    /// Data volume for menu item entries
    /// </summary>
    public readonly struct MainMenuItemEntry
    {
        public MainMenuItemEntry(string categoryPath, MethodInfo methodInfo)
        {
            CategoryPath = categoryPath;
            MethodInfo = methodInfo;
        }

        /// <summary>
        /// The menu path.(Windows/Rendering/LightSettingsWindow)
        /// </summary>
        public readonly string CategoryPath;

        /// <summary>
        /// The target static method info to call when clicked on
        /// </summary>
        public readonly MethodInfo MethodInfo;

      
    }
}
