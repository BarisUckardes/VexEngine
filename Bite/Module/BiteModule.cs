using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Engine;
using Vex.Platform;
using Fang.Renderer;
using Fang.GUI;
using Fang.Commands;
using Bite.GUI;
using Vex.Extensions;
using Bite.Core;

namespace Bite.Module
{
    public sealed class BiteModule : EngineModule
    {
        public void RegisterGUISystem<TSystem>() where TSystem : GUISystem, new()
        {
            /*
             * Create new gui system
             */
            TSystem system = new TSystem();

            /*
             * Set editor session
             */
            system.Session = m_Session;

            /*
             * Add it to pending create gui systems
             */
            m_GUISystems.Add(system);
        }
        public void RegisterCoreCommand<TCommand>() where TCommand : CoreCommand,new()
        {
            /*
             * Create new core command
             */
            TCommand command = new TCommand();

            /*
             * Set editor session
             */
            command.EditorSession = m_Session;

            /*
             * Add it to core command list
             */
            m_CoreCommands.Add(command);
        }
            
        public override void OnAttach()
        {
            /*
             * Create editor session
             */
            m_Session = new EditorSession(Session);

            /*
             * Set window
             */
            m_Window = Session.Window;

            /*
             * Create renderer
             */
            m_Renderer = new GUIRenderer(m_Window.LocalWindow.Width,m_Window.LocalWindow.Height);

            /*
             * Initialize variables 
             */
            m_CoreCommands = new List<CoreCommand>();
            m_GUISystems = new List<GUISystem>();

            /*
             * Add built-in GUI systems
             */
            RegisterGUISystem<MainMenuGUISystem>();
            RegisterGUISystem<WindowGUISystem>();
            RegisterGUISystem<ComponentGUISystem>();
            RegisterGUISystem<ObjectGUISystem>();

            /*
             * Register built-in core commands
             */
            RegisterCoreCommand<DomainCoreCommand>();

            /*
             * Execute core command attach invokes
             */
            for(int commandIndex = 0;commandIndex < m_CoreCommands.Count;commandIndex++)
            {
                m_CoreCommands[commandIndex].OnAttach();
            }

            /*
             * Execute gui module attach invokes
             */
            for(int moduleIndex = 0; moduleIndex < m_GUISystems.Count;moduleIndex++)
            {
                m_GUISystems[moduleIndex].OnAttach();
            }
        }

        public override void OnDetach()
        {
            /*
            * Execute core command detach invokes
            */
            for (int commandIndex = 0; commandIndex < m_CoreCommands.Count; commandIndex++)
            {
                m_CoreCommands[commandIndex].OnAttach();
            }
            m_CoreCommands.Clear();
            m_CoreCommands = null;

            /*
             * Execute gui module detach invokes
             */
            for(int moduleIndex = 0;moduleIndex < m_GUISystems.Count;moduleIndex++)
            {
                m_GUISystems[moduleIndex].OnDetach();
                m_GUISystems[moduleIndex].Session = null;
            }
            m_GUISystems.Clear();
            m_GUISystems = null;

            /*
             * Dereference window
             */
            m_Window = null;

            /*
             * GUI renderer
             */
            m_Renderer.Dispose();
            m_Renderer = null;

            /*
             * Shutdown editor session
             */
            m_Session.Shutdown();
            m_Session = null;
        }

        public override void OnEvent(PlatformEvent eventData)
        {
            if(eventData.Type == PlatformEventType.KeyChar)
            {
                PlatformKeyCharEvent ev = (PlatformKeyCharEvent)eventData;
                m_Renderer.PressChar((char)ev.KeyCode);
            }
            else if (eventData.Type == PlatformEventType.MouseScVexlled)
            {
                PlatformMouseScrolledEvent ev = (PlatformMouseScrolledEvent)eventData;
                m_Renderer.MouseScVexll(new OpenTK.Mathematics.Vector2(ev.AmountX, ev.AmountY));
            }
        }
        public override void OnUpdate()
        {
            /*
            * Start listening render command
            */
            m_Renderer.Begin(m_Window.LocalWindow, 1.0f / 60.0f,PlatformWindowProperties.Size.GetAsOpenTK());

            /*
             * Run GUI Systems
             */
            foreach (GUISystem system in m_GUISystems)
            {
                system.OnUpdate();
            }

            /*
            * Finalize&Render GUI
            */
            m_Renderer.Finalize();
        }


        private List<GUISystem> m_GUISystems;
        private List<CoreCommand> m_CoreCommands;
        private WindowInterface m_Window;
        private GUIRenderer m_Renderer;
        private EditorSession m_Session;
    }
}
