using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
using System.Reflection;
using Fang.Commands;
namespace Bite.GUI
{
    public sealed class MainMenuGUISystem : GUISystem
    {
        public override void OnAttach()
        {
            /*
             * Initialize local variables
             */
            m_AttributeEntries = new List<MainMenuItemEntry>();

            /*
             * Gather main menu attributes
             */
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    {
                        Attribute attribute = method.GetCustomAttribute(typeof(MainMenuItemAttribute));
                        if (attribute != null)
                        {
                            MainMenuItemAttribute foundAttribute = (MainMenuItemAttribute)method.GetCustomAttribute(typeof(MainMenuItemAttribute));
                            m_AttributeEntries.Add(new MainMenuItemEntry(foundAttribute.Category, method));
                        }
                    }
                }
            }

            /*
             * Create hierarchy
             */
            m_Hierarchy = new MenuHierarchy();
            for(int i=0;i<m_AttributeEntries.Count;i++)
            {
                m_Hierarchy.RegisterNode(m_AttributeEntries[i].CategoryPath,m_AttributeEntries[i].MethodInfo);
            }

            /*
             * Validate hiearchy
             */
            m_Hierarchy.ValidateHiearchy();

        }

        public override void OnDetach()
        {
            m_Hierarchy = null;
            m_AttributeEntries.Clear();
            m_AttributeEntries = null;
        }

        public override void OnUpdate()
        {
            /*
             * Start main menu bar
             */
            GUIRenderCommands.CreateMainMenuBar();

            /*
             * Render hierarch
             */
            m_Hierarchy.RenderHierarchy();

            /*
             * Finalize main menu bar
             */
            GUIRenderCommands.FinalizeMainMenuBar();
        }

        private MenuHierarchy m_Hierarchy;
        private List<MainMenuItemEntry> m_AttributeEntries;
    }
}
