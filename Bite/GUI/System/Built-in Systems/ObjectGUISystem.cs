using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Vex.Engine;

namespace Bite.GUI
{
    public sealed class ObjectGUISystem : GUISystem
    {
        public override void OnAttach()
        {
            /*
             * Gather object layout attributes
             */
            List<ObjectLayoutEntry> entries = new List<ObjectLayoutEntry>();

            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            foreach(Type type in currentAssembly.GetTypes())
            {
               

                /*
                 * Try get attribute
                 */
                ObjectLayoutAttribute attribute = type.GetCustomAttribute(typeof(ObjectLayoutAttribute)) as ObjectLayoutAttribute;

                /*
                 * Validate attribute
                 */
                if(attribute != null)
                {
                   
                    /*
                     * Validate if its an engine object
                    */
                    if (!attribute.TargetObjectType.IsSubclassOf(typeof(EngineObject)))
                        continue;

                    entries.Add(new ObjectLayoutEntry(attribute.TargetObjectType, type));
                }
            }

            /*
             * Upload entry data
             */
            GUIObject.Entries = entries;
        }

        public override void OnDetach()
        {

        }

        public override void OnUpdate()
        {

        }


    }
}
