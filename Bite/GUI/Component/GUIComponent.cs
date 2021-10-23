using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    internal static class GUIComponent
    {

        internal static ComponentLayout FetchComponentLayout(Type componentType)
        {
            /*
             * Try find user defined entry
             */
            foreach(ComponentLayoutEntry entry in s_Entries)
            {
                if (entry.TargetComponentType == componentType)
                    return Activator.CreateInstance(entry.ComponentLayoutType) as ComponentLayout;
            }

            /*
             * Get default component layout info
             */
            DefaultComponentLayoutInfo info = new DefaultComponentLayoutInfo(null, null,null);
            for(int i=0;i<s_DefaultEntries.Count;i++)
            {
                if(s_DefaultEntries[i].TargetComponentType == componentType)
                {
                    info = s_DefaultEntries[i];
                    break;
                }    
            }

            /*
             * Create default component layout
             */
            DefaultComponentLayout defaultLayout = new DefaultComponentLayout(info);

            return defaultLayout;
        }
        internal static List<ComponentLayoutEntry> Entries
        {
            get
            {
                return s_Entries;
            }
            set
            {
                s_Entries = value;
            }
        }
        internal static List<DefaultComponentLayoutInfo> DefaultEntries
        {
            get
            {
                return s_DefaultEntries;
            }
            set
            {
                s_DefaultEntries = value;
            }
        }

        
        private static List<ComponentLayoutEntry> s_Entries;
        private static List<DefaultComponentLayoutInfo> s_DefaultEntries;
    }
}
