using Bite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    /// <summary>
    /// A manager which handles component layout creation and handling
    /// </summary>
    public class GUIComponentManager
    {
        /// <summary>
        /// Returns the current component manager
        /// </summary>
        public static GUIComponentManager Current
        {
            get
            {
                return s_Current;
            }
        }
        private static GUIComponentManager s_Current;

        internal GUIComponentManager(in EditorSession session,in List<DefaultComponentLayoutInfo> defaultLayouts,in List<ComponentLayoutEntry> layoutEntries)
        {
            m_DefaultEntries = defaultLayouts;
            m_Entries = layoutEntries;
            m_Session = session;
            s_Current = this;
           
        }
      
        /// <summary>
        /// Tries to create type target specific component layout.If non found will create a default component layout instead
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public ComponentLayout FetchComponentLayout(Type componentType)
        {
            /*
             * Try find user defined entry
             */
            foreach(ComponentLayoutEntry entry in m_Entries)
            {
                if (entry.TargetComponentType == componentType)
                    return Activator.CreateInstance(entry.ComponentLayoutType) as ComponentLayout;
            }

            /*
             * Get default component layout info
             */
            DefaultComponentLayoutInfo info = new DefaultComponentLayoutInfo(null, null,null);
            for(int i=0;i< m_DefaultEntries.Count;i++)
            {
                if(m_DefaultEntries[i].TargetComponentType == componentType)
                {
                    info = m_DefaultEntries[i];
                    break;
                }    
            }

            /*
             * Create default component layout
             */
            DefaultComponentLayout defaultLayout = new DefaultComponentLayout(info);

            return defaultLayout;
        }

        private List<ComponentLayoutEntry> m_Entries;
        private List<DefaultComponentLayoutInfo> m_DefaultEntries;
        private EditorSession m_Session;
    }
}
