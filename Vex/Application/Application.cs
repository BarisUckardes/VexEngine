using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Vex.Engine;
using Vex.Platform;
using Vex.Profiling;
using Vex.Framework;
using Vex.Types;
using System.IO;
using Vex.Graphics;

namespace Vex.Application
{
    /// <summary>
    /// Represents the whole application
    /// </summary>
    public sealed class Application
    {
        public Application(string applicationTitle,WindowCreateParams windowCreateParams,WindowUpdateParams windowUpdateParams,CultureInfo targetCultureInfo,List<string> additionalLibraries,string targetDomainRootDirectory,string[] commandLineArguments,bool startWorldImmediately = false,bool setIntermediateFramebufferAsSwapchain = true)
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
            m_ApplicationWindow = new Window(applicationTitle,windowCreateParams, windowUpdateParams,setIntermediateFramebufferAsSwapchain);
            m_ApplicationWindow.SetApplicationEventDelegate(OnPlatformEvent);

            /*
             * Set target domain directory
             */
            m_TargetDomainRootDirectory = targetDomainRootDirectory;

            /*
             * Set additional libraries
             */
            m_AdditionalLibraries = additionalLibraries.ToArray();

            /*
             * Set application target culture info
             */
            m_TargetCultureInfo = targetCultureInfo;

            /*
             * Set immediate mode
             */
            m_StartWorldImmediately = startWorldImmediately;

            /*
             * Set intermediate framebuffer
             */
            m_SetIntermediateFramebufferAsSwapchain = setIntermediateFramebufferAsSwapchain;
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
        public Window WindowInterface
        {
            get
            {
                return m_ApplicationWindow;
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
             * Set target culture info
             */
            CultureInfo.CurrentCulture = m_TargetCultureInfo;
            CultureInfo.CurrentUICulture = m_TargetCultureInfo;

            
            /*
             * Load additonal libraries
             */
            for (int libraryIndex = 0;libraryIndex < m_AdditionalLibraries.Length;libraryIndex++)
            {
                /*
                 * Get path
                 */
                string libraryPath = m_AdditionalLibraries[libraryIndex];

                /*
                 * Validate file
                 */
                if (!File.Exists(libraryPath))
                    continue;

                /*
                 * Load assembly into appdomain
                 */
                Assembly.LoadFrom(libraryPath);

                Console.WriteLine($"Application loaded assembly [{libraryPath}]");
            }

            /*
             * Get all component types
             */
            List<Type> componentTypes = new List<Type>(100);
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type type in assembly.GetTypes())
                {
                    /*
                     * Validate if its a component type
                     */
                    if(type.IsAssignableTo(typeof(Component)))
                    {
                        componentTypes.Add(type);
                    }
                }
            }

            /*
             * Get all view types
             */
            List<Type> worldViewTypes = new List<Type>(100);
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    /*
                     * Validate if its a component type
                     */
                    if (type != typeof(WorldView) && type.IsAssignableTo(typeof(WorldView)))
                    {
                        worldViewTypes.Add(type);
                    }
                }
            }

            /*
             * Get all resolver types
             */
            List<Type> resolverTypes = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    /*
                     * Validate if its a component type
                     */
                    if (type != typeof(IWorldResolver) && type.IsAssignableTo(typeof(IWorldResolver)))
                    {
                        resolverTypes.Add(type);
                    }
                }
            }

            /*
             * Set emitted component types
             */
            EmittedComponentTypes.ComponentTypes = componentTypes;

            /*
             * Set emitted world view types
             */
            EmittedWorldViewTypes.Types = worldViewTypes;

            /*
             * Set emitted world view resolver types
             */
            EmittedWorldViewResolverTypes.Types = resolverTypes;

            /*
             * Create session
             */
            m_Session = new ApplicationSession(m_ApplicationWindow);

            /*
            * Validate immediate mode
            */
            if (m_StartWorldImmediately)
            {
                /*
                 * Validate immediate mode file
                 */
                if (!File.Exists(PlatformPaths.DomainRootDirectoy + @"\ImmediateWorld.vsettings"))
                {
                    hasExitRequest = true;
                    exitType = ApplicationExitType.SessionRequest;
                    exitMessage = "No immediate world file for immediate mode";

                }
                else
                {
                    /*
                     * Get world id
                     */
                    Guid worldID = Guid.Parse(File.ReadAllText(PlatformPaths.DomainRootDirectoy + @"\ImmediateWorld.vsettings"));

                    /*
                     * Load and switch first world
                     */
                    World.LoadAndSwitch(worldID);

                    /*
                     * Set play active true
                     */
                    m_Session.PlayActive = true;
                }

            }

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
                if (m_ApplicationWindow.HasWindowCloseRequest)
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
                m_ApplicationWindow.UpdateInput();
                Profiler.EndProfile();

                /*
                 * Run application modules
                 */
                Profiler.StartProfile("Application Modules");
                for(int i=0;i<m_ActiveModules.Count;i++)
                {
                    m_ActiveModules[i].OnUpdate(m_Session.PlayActive);
                }
                Profiler.EndProfile();

                /*
                 * Swap window buffer
                 */
                Profiler.StartProfile("Swapbuffers");
                m_ApplicationWindow.SwapBuffers();
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
            for(int delegateIndex = m_ModuleEventDelegates.Count - 1;delegateIndex >= 0;delegateIndex--)
            {
                /*
                 * Execute module input delegate
                 */
                m_ModuleEventDelegates[delegateIndex].Invoke(evData);

                /*
                 * Terminate broadcast if current event is handled
                 */
                if (evData.IsHandled)
                    break;
            }
        }

        private List<EngineModule> m_AttachPendingModules;
        private List<EngineModule> m_DetachPendingModules;
        private List<EngineModule> m_ActiveModules;
        private List<ReceivePlatformEventDelegate> m_ModuleEventDelegates;
        private ApplicationSession m_Session;
        private Window m_ApplicationWindow;
        private CultureInfo m_TargetCultureInfo;
        private string[] m_AdditionalLibraries;
        private string[] m_CommandLineArguments;
        private string m_TargetDomainRootDirectory;
        private string m_ProjectName;
        private bool m_StartWorldImmediately;
        private bool m_SetIntermediateFramebufferAsSwapchain;
    }
}
