using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;

namespace Bite.GUI
{
    public delegate void ObjectChangedDelegate(VexObject targetObject);
    public static class GUIObject
    {
        
        public static ObjectLayout FetchLayout(Type type)
        {
            /*
             * Try fetch object layout
             */
            for(int i=0;i<s_Entries.Count;i++)
            {
                if(s_Entries[i].TargetObjectType == type)
                {
                    return Activator.CreateInstance(s_Entries[i].ObjectLayoutType) as ObjectLayout;
                }
            }

            return null;
        }

        internal static List<ObjectLayoutEntry> Entries
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
        internal static void RegisterListener(ObjectChangedDelegate functionPtr)
        {
            OnSignalObjectChanged += functionPtr;

        }
        internal static void SignalNewObject(VexObject obj)
        {
            OnSignalObjectChanged?.Invoke(obj);
        }

        private static event ObjectChangedDelegate OnSignalObjectChanged;
        private static List<ObjectLayoutEntry> s_Entries;
    }
}
