using Vex.Engine;
using Vex.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bite.Core;
namespace Bite.Module
{
    public sealed class BiteCoreModule : EngineModule
    {

        public void RegisterCoreSystem<TSystem>() where TSystem : CoreSystem,new()
        {
            TSystem system = new TSystem();
            m_PendingCreateCoreSystem.Add(system);
        }
        public override void OnAttach()
        {
            /*
             * Initialize local variable
             */
            m_PendingCreateCoreSystem = new List<CoreSystem>();
            m_PendingDeleteCoreSystem = new List<CoreSystem>();
            m_ActiveCoreSystems = new List<CoreSystem>();

            /*
             * Create default core systems
             */
            RegisterCoreSystem<AssemblyCoreSystem>();
            RegisterCoreSystem<PVexjectCoreSystem>();
            RegisterCoreSystem<DomainCoreSystem>();
        }

        public override void OnDetach()
        {
            m_ActiveCoreSystems.Clear();
            m_PendingCreateCoreSystem.Clear();
            m_PendingDeleteCoreSystem.Clear();
        }

        public override void OnEvent(Event eventData)
        {

        }

        public override void OnUpdate()
        {
            /*
             * Execute delete pending list
             */
            foreach (CoreSystem system in m_PendingDeleteCoreSystem)
                system.OnDetach();

            m_PendingDeleteCoreSystem.Clear();

            /*
             * Execute create pending list
             */
            foreach(CoreSystem system in m_PendingCreateCoreSystem)
            {
                system.OnAttach();
                m_ActiveCoreSystems.Add(system);
            }
            m_PendingCreateCoreSystem.Clear();

            /*
             * Update core systems
             */
            foreach (CoreSystem system in m_ActiveCoreSystems)
                system.OnUpdate();
        }

        private List<CoreSystem> m_PendingCreateCoreSystem;
        private List<CoreSystem> m_PendingDeleteCoreSystem;
        private List<CoreSystem> m_ActiveCoreSystems;
    }
}
