using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    public readonly struct MainMenuItemEntry
    {
        public MainMenuItemEntry(string categoryPath, MethodInfo methodInfo)
        {
            CategoryPath = categoryPath;
            MethodInfo = methodInfo;
        }

        public readonly string CategoryPath;
        public readonly MethodInfo MethodInfo;

      
    }
}
