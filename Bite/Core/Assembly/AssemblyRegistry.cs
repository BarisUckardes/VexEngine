using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Bite.Core
{
    public class AssemblyRegistry
    {
        public static string[] SpecialAssemblyNames = new string[] { "Bite","Vex","UserGameCode","UserEditorCode" };
        public static AssemblyRegistry Current
        {
            get
            {
                return s_Current;
            }
        }
        private static AssemblyRegistry s_Current;
        public AssemblyRegistry()
        {
            /*
             * Initialize local variables
             */
            m_SpecialAssemblies = new List<AssemblyEntry>();
            m_AssembliesFound = new List<AssemblyEntry>();

            /*
             * Get assemblies
             */
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            /*
             * Gather assemblies
             */
            GatherAssemblyNames(assemblies, ref m_AssembliesFound,ref m_SpecialAssemblies);

            /*
             * Set current
             */
            s_Current = this;
        }

        private void GatherAssemblyNames(Assembly[] assemblies,ref List<AssemblyEntry> assembliesFound,ref List<AssemblyEntry> specialAssemblies)
        {
            /*
             * Iterate assemblies
             */
            foreach(Assembly assembly in assemblies)
            {
                /*
                 * Add entry
                 */
                m_AssembliesFound.Add(new AssemblyEntry(assembly.GetName().Name, assembly));

                /*
                 * Check if its a special assembly
                 */
                if(SpecialAssemblyNames.Contains(assembly.GetName().Name))
                {
                    m_SpecialAssemblies.Add(new AssemblyEntry(assembly.GetName().Name, assembly));
                }
            }
        }


        private List<AssemblyEntry> m_SpecialAssemblies;
        private List<AssemblyEntry> m_AssembliesFound;
    }
}
