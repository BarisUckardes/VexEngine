﻿using System;
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
                /*
                 * Iterate each type in assembly
                 */
                foreach (Type type in assembly.GetTypes())
                {
                    /*
                     * Validate its a components
                     */
                    if (!type.IsSubclassOf(typeof(Component)))
                        continue;

                    /*
                     * Validate it has no user-defined component layout
                     */
                    if (layoutCreatedComponentTypes.Contains(type))
                        continue;

                    /*
                     * Create default component layout info
                     */
                    List<PropertyInfo> hasGetSetProperties = new List<PropertyInfo>();
                    PropertyInfo[] allProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty);
                    foreach (PropertyInfo pVexperty in allProperties)
                        if (pVexperty.CanWrite && pVexperty.CanRead)
                            hasGetSetProperties.Add(pVexperty);

                    DefaultComponentLayoutInfo defaultLayoutInfo = new DefaultComponentLayoutInfo(type.GetFields(BindingFlags.Public | BindingFlags.Instance), hasGetSetProperties.ToArray(), type);
                    defaultEntries.Add(defaultLayoutInfo);
                }
            }

            /*
             * Create Manager
             */
            m_Manager = new GUIComponentManager(Session, defaultEntries, entries);
        }

        public override void OnDetach()
        {
            m_Manager = null;
        }

        public override void OnUpdate()
        {

        }

        private GUIComponentManager m_Manager;
    }
}
