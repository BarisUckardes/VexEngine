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

namespace Bite.Module
{
    public sealed class BiteGUIModule : EngineModule
    {
        public void RegisterGUISystem<TSystem>() where TSystem : GUISystem, new()
        {
            TSystem system = new TSystem();
            m_PendingCreateGUISystems.Add(system);

        }
        public override void OnAttach()
        {
            /*
             * Set window
             */
            m_Window = Session.Window;

            /*
             * Create renderer
             */
            m_Renderer = new GUIRenderer(m_Window.LocalWindow.Width,m_Window.LocalWindow.Height);

            /*
             * Initialize GUI systems
             */
            m_PendingCreateGUISystems = new List<GUISystem>();
            m_PendingDeleteGUISystems = new List<GUISystem>();
            m_ActiveGUISystems = new List<GUISystem>();

            /*
             * Add built-in GUI systems
             */
            RegisterGUISystem<MainMenuGUISystem>();
            RegisterGUISystem<WindowGUISystem>();
            RegisterGUISystem<ComponentGUISystem>();
            RegisterGUISystem<ObjectGUISystem>();
        }

        public override void OnDetach()
        {
            /*
             * Dereference window
             */
            m_Window = null;

            /*
             * GUI renderer
             */
            m_Renderer.Dispose();
            m_Renderer = null;
        }

        public override void OnEvent(Event eventData)
        {

        }
        public override void OnUpdate()
        {
            /*
             * Delete pending systems
             */
            foreach(GUISystem system in m_PendingDeleteGUISystems)
            {
                system.OnDetach();
            }
            m_PendingDeleteGUISystems.Clear();

            /*
             * Create pending systems
             */
            foreach(GUISystem system in m_PendingCreateGUISystems)
            {
                system.OnAttach();
                m_ActiveGUISystems.Add(system);
            }
            m_PendingCreateGUISystems.Clear();


            /*
            * Start listening render command
            */
            m_Renderer.Begin(m_Window.LocalWindow, 1.0f / 60.0f);

            /*
             * Run GUI Systems
             */
            foreach (GUISystem system in m_ActiveGUISystems)
            {
                system.OnUpdate();
            }

            /*
            * Finalize&Render GUI
            */
            m_Renderer.Finalize();
        }

      

        private List<GUISystem> m_PendingCreateGUISystems;
        private List<GUISystem> m_PendingDeleteGUISystems;
        private List<GUISystem> m_ActiveGUISystems;
        private WindowInterface m_Window;
        private GUIRenderer m_Renderer;
    }
}
