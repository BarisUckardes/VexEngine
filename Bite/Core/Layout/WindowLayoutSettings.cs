using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    public class WindowLayoutSettings
    {
        public static List<WindowLayoutSettings> CreateDefaultLayout()
        {
            List<WindowLayoutSettings> layouts = new List<WindowLayoutSettings>();

            layouts.Add(new WindowLayoutSettings("", "DomainObserverGUIWindow", Guid.NewGuid()));
            layouts.Add(new WindowLayoutSettings("", "GameObserverGUIWindow", Guid.NewGuid()));
            layouts.Add(new WindowLayoutSettings("", "ObjectObserverGUIWindow", Guid.NewGuid()));
            layouts.Add(new WindowLayoutSettings("", "WorldObserverGUIWindow", Guid.NewGuid()));
            return layouts;
        }
        public WindowLayoutSettings(string assemblyName, string typeName, Guid iD)
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
            ID = iD;
        }
        public WindowLayoutSettings()
        {
            AssemblyName = "None";
            TypeName = "None";
            ID = Guid.Empty;
        }

        public string AssemblyName;
        public string TypeName;
        public Guid ID;

       
    }
}
