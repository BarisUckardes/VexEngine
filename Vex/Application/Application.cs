using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Engine;
using Vex.Platform;
using Vex.Profiling;

namespace Vex.Application
{
    /// <summary>
    /// Represents the whole application
    /// </summary>
    public sealed class Application
    {
        public Application(string applicationTitle,WindowCreateParams windowCreateParams,WindowUpdateParams windowUpdateParams,string targetDomainRootDirectory,string[] commandLineArguments)
        {
            /*
            * Initialize local lists
            */
            m_ActiveModules = new List<EngineModule>();
            m_AttachPendingModules = new List<EngineModule>();
            m_DetachPendingModules = new List<EngineModule>();
            m_ModuleEventDelegates = new List<ReceivePlatformEventDelegate>();

            /*
             * Create window
             */
            m_WindowInterface = new WindowInterface(applicationTitle,windowCreateParams, windowUpdateParams);
            m_WindowInterface.LocalWindow.SetApplicationEventDelegate(OnPlatformEvent);

            /*
             * Set target domain directory
             */
            m_TargetDomainRootDirectory = targetDomainRootDirectory;
        }

        /// <summary>
        /// Returns the current application session
        /// </summary>
        public ApplicationSession Session
        {
            get
            {
                return m_Session;
            }
        }

        /// <summary>
        /// Returns the current application window interface
        /// </summary>
        public WindowInterface WindowInterface
        {
            get
            {
                return m_WindowInterface;
            }
        }

        /// <summary>
        /// Application main loop
        /// </summary>
        public void Run()
        {
            /*
             * Create exit state
             */
            bool hasExitRequest = false;
            ApplicationExitType exitType = ApplicationExitType.Undefined;
            string exitMessage = string.Empty;

            /*
             * Set initialization properties
             */
            CommandLineArguments.Arguments = m_CommandLineArguments;
            PlatformPaths.DomainRootDirectoy = m_TargetDomainRootDirectory;

            /*
             * Create session
             */
            m_Session = new ApplicationSession(m_WindowInterface);

            /*
             * Detach modules
             */
            for (int i = 0; i < m_DetachPendingModules.Count; i++)
            {
                m_DetachPendingModules[i].OnAttach();
            }
            m_DetachPendingModules.Clear();

            /*
             * Attach modules
             */
            for (int i = 0; i < m_AttachPendingModules.Count; i++)
            {
                m_AttachPendingModules[i].SetSession(m_Session);
                m_AttachPendingModules[i].OnAttach();
                m_ActiveModules.Add(m_AttachPendingModules[i]);
            }
            m_AttachPendingModules.Clear();

            /*
             * Run application loop
             */
            while (!hasExitRequest)
            {

                /*
                * Valdiate exit request
                */
                if (m_WindowInterface.HasWindowExitRequest)
                {
                    hasExitRequest = true;
                    exitType = ApplicationExitType.WindowClose;
                    exitMessage = "Window closed manually";
                }
                else if (m_Session.HasShutdownRequest)
                {
                    hasExitRequest = true;
                    exitType = ApplicationExitType.SessionRequest;
                    exitMessage = m_Session.ShutdownRequestMessage;
                }

                Profiler.StartProfileSession();

                /*
                 * Update window
                 */
                Profiler.StartProfile("Update window input");
                m_WindowInterface.UpdateInput();
                Profiler.EndProfile();

                /*
                 * Stream through events
                 */
                Profiler.StartProfile("Broadcast events");
                PlatformEvent[] events = m_WindowInterface.Events;
                for (int eventIndex = 0; eventIndex < events.Length; eventIndex++)
                {

                    for (int moduleIndex = m_ActiveModules.Count - 1; moduleIndex >= 0; moduleIndex--)
                    {
                        /*
                         * Send this event thVexugh all the module
                         */
                        m_ActiveModules[moduleIndex].OnEvent(events[eventIndex]);

                        /*
                         * Validate if this event consumed and handled
                         */
                        if (events[eventIndex].IsHandled) // handled 
                        {
                            break;
                        }
                    }
                   
                }
                Profiler.EndProfile();


                /*
                 * Run application modules
                 */
                Profiler.StartProfile("Application Modules");
                for(int i=0;i<m_ActiveModules.Count;i++)
                {
                    m_ActiveModules[i].OnUpdate();
                }
                Profiler.EndProfile();

                /*
                 * Swap window buffer
                 */
                Profiler.StartProfile("Swapbuffers");
                m_WindowInterface.Swapbuffers();
                Profiler.EndProfile();

               

                Profiler.EndProfileSession();
            }

            /*
             * Detach modules
             */
            for (int i = 0; i < m_ActiveModules.Count; i++)
            {
                m_ActiveModules[i].OnDetach();
            }

            /*
             * Shutdown session
             */
            m_Session.Shutdown();
            m_Session = null;

            /*
             * Display shutdown message
             */
            Console.WriteLine("Application close reason: [" + exitMessage + "]");
        }
    
        /// <summary>
        /// Registers an engine module to this application
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        public TModule RegisterModule<TModule>() where TModule: EngineModule,new()
        {
            /*
             * Create new module
             */
            TModule module = new TModule();

            /*
             * Register new module to attach pending list
             */
            m_AttachPendingModules.Add(module);

            /*
             * Register new moodule on event delegate
             */
            m_ModuleEventDelegates.Add(module.OnEvent);
            return module;
        }
        public void RegisterModules(in List<EngineModule> modules)
        {
            /*
             * Attach and register each of them
             */
            foreach(EngineModule module in modules)
            {
                m_AttachPendingModules.Add(module);
                m_ModuleEventDelegates.Add(module.OnEvent);
            }
        }
        public void OnPlatformEvent(PlatformEvent evData)
        {
            /*
             * Invoke all module on event delegates
             */
            foreach (ReceivePlatformEventDelegate delegateIt in m_ModuleEventDelegates)
                delegateIt.Invoke(evData);
        }

        private List<EngineModule> m_AttachPendingModules;
        private List<EngineModule> m_DetachPendingModules;
        private List<EngineModule> m_ActiveModules;
        private List<ReceivePlatformEventDelegate> m_ModuleEventDelegates;
        private ApplicationSession m_Session;
        private WindowInterface m_WindowInterface;
        private string[] m_CommandLineArguments;
        private string m_TargetDomainRootDirectory;
    }
}
