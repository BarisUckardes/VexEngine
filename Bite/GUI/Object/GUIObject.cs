using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;

namespace Bite.GUI
{
    /// <summary>
    /// Event delegate which used for broadcasting the object observer's target object changed
    /// </summary>
    /// <param name="targetObject"></param>
    public delegate void ObjectChangedDelegate(VexObject targetObject);

    /// <summary>
    /// GUI layout for objects which will appear on the object observer widnow
    /// </summary>
    public static class GUIObject
    {
        /// <summary>
        /// Try get a object layout via its target type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns all the object layout entries
        /// </summary>
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

        /// <summary>
        /// Registers a object change listener
        /// </summary>
        /// <param name="functionPtr"></param>
        internal static void RegisterListener(ObjectChangedDelegate functionPtr)
        {
            OnSignalObjectChanged += functionPtr;

        }

        /// <summary>
        /// Called when anew object selected for the object observer window
        /// </summary>
        /// <param name="obj"></param>
        internal static void SignalNewObject(VexObject obj)
        {
            OnSignalObjectChanged?.Invoke(obj);
        }

        private static event ObjectChangedDelegate OnSignalObjectChanged;
        private static List<ObjectLayoutEntry> s_Entries;
    }
}
