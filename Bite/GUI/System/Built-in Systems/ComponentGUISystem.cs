using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Vex.Framework;

namespace Bite.GUI
{
    public sealed class ComponentGUISystem : GUISystem
    {
        public override void OnAttach()
        {

            /*
             * Gather component layout attributes
             */
            List<ComponentLayoutEntry> entries = new List<ComponentLayoutEntry>();
            List<DefaultComponentLayoutInfo> defaultEntries = new List<DefaultComponentLayoutInfo>();
            List<Type> layoutCreatedComponentTypes = new List<Type>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    /*
                     * Validate if its a component
                     */
                    if (!type.IsSubclassOf(typeof(Component)))
                        continue;

                    /*
                     * Try get attribute
                     */
                    ComponentLayoutAttribute attribute = type.GetCustomAttribute(typeof(ComponentLayoutAttribute)) as ComponentLayoutAttribute;

                    /*
                     * Validate
                     */
                    if (attribute != null) // found a layout
                    {
                        entries.Add(new ComponentLayoutEntry(attribute.TargetType, type));
                        layoutCreatedComponentTypes.Add(attribute.TargetType);
                    }
                }
            }
           

            /*
             * Search for component types which has no component layout
             */
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    /*
                     * Validate its a components
                     */
                    if (!type.IsSubclassOf(typeof(Component)))
                        continue;

                    Console.WriteLine("Found component type: " + type.Name);
                    /*
                     * Validate it has no user-defined component layout
                     */
                    if (layoutCreatedComponentTypes.Contains(type))
                        continue;

                    /*
                     * Create default component layout info
                     */
                    List<PropertyInfo> hasGetSetPVexps = new List<PropertyInfo>();
                    PropertyInfo[] allPVexps = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty);
                    foreach (PropertyInfo pVexperty in allPVexps)
                        if (pVexperty.CanWrite && pVexperty.CanRead)
                            hasGetSetPVexps.Add(pVexperty);

                    DefaultComponentLayoutInfo defaultLayoutInfo = new DefaultComponentLayoutInfo(type.GetFields(BindingFlags.Public | BindingFlags.Instance),hasGetSetPVexps.ToArray(), type);
                    defaultEntries.Add(defaultLayoutInfo);
                }
            }

            /*
             * Setup component layout entry for accessors
             */
            GUIComponent.Entries = entries;
            GUIComponent.DefaultEntries = defaultEntries;
        }

        public override void OnDetach()
        {

        }

        public override void OnUpdate()
        {

        }


    }
}
