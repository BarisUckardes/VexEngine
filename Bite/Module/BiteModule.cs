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
using ImGuiNET;
using System.Numerics;

namespace Bite.Module
{
    /// <summary>
    /// Bite's main module
    /// </summary>
    public sealed class BiteModule : EngineModule
    {
        public BiteModule(in List<CoreCommand> commands,in List<GUISystem> guiSystems)
        {
            /*
             * Set target core commands & gui systems
             */
            m_CoreCommands = commands;
            m_GUISystems = guiSystems;
        }
        public override void OnAttach()
        {
            /*
             * Create editor session
             */
            m_Session = new EditorSession(Session);

            /*
            * Set editor commands
            */
            EditorCommands.SetSession(m_Session);

            /*
             * Set window
             */
            m_Window = Session.Window;

            /*
             * Create renderer
             */
            m_Renderer = new GUIRenderer(m_Window.Width,m_Window.Height);

            /*
             * Set core command session
             */
            foreach (CoreCommand command in m_CoreCommands)
            {
                command.SetEditorSession(m_Session);
            }

            /*
             * Set gui system session
             */
            foreach (GUISystem system in m_GUISystems)
            {
                system.SetEditorSession(m_Session);
            }

            /*
             * Execute core command attach invokes
             */
            for (int commandIndex = 0;commandIndex < m_CoreCommands.Count;commandIndex++)
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

            /*
             * Set default play state
             */
           // m_Session.StopGamePlaySession();
        }

        public override void OnDetach()
        {
            /*
            * Execute core command detach invokes
            */
            for (int commandIndex = 0; commandIndex < m_CoreCommands.Count; commandIndex++)
            {
                m_CoreCommands[commandIndex].OnDetach();
            }
            m_CoreCommands.Clear();
            m_CoreCommands = null;

            /*
             * Execute gui module detach invokes
             */
            for(int moduleIndex = 0;moduleIndex < m_GUISystems.Count;moduleIndex++)
            {
                m_GUISystems[moduleIndex].OnDetach();
                m_GUISystems[moduleIndex].SetEditorSession(null);
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

            /*
             * Shutdown editor commands
             */
            EditorCommands.Shutdown();
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

            if (m_Session.HandleInputs)
            {
                eventData.MarkHandled();
            }
                
        }
        bool dockspaceState;
        public override void OnUpdate(bool active)
        {
            /*
            * Start listening render command
            */
            m_Renderer.Begin(m_Window, 1.0f / 165.0f,PlatformWindowProperties.Size.GetAsOpenTK());

            /*
             * Create main dockspace window
             */
            ImGuiWindowFlags flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus | ImGuiWindowFlags.MenuBar;
            ImGui.SetNextWindowPos(ImGui.GetMainViewport().Pos);
            ImGui.SetNextWindowSize(ImGui.GetMainViewport().Size);
            ImGui.SetNextWindowViewport(ImGui.GetMainViewport().ID);

            /*
             * Create main dockspace window
             */
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0,0));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, 0);
            ImGui.Begin("Dockspace", ref dockspaceState, flags);
            ImGui.PopStyleVar();

            /*
             * Create main dockspace
             */
            uint dockspaceID = ImGui.GetID("MyDockSpace");
            ImGui.DockSpace(dockspaceID, new System.Numerics.Vector2(0, 0),ImGuiDockNodeFlags.None | ImGuiDockNodeFlags.PassthruCentralNode);

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
            ImGui.End();
            m_Renderer.Finalize();
        }


        private List<GUISystem> m_GUISystems;
        private List<CoreCommand> m_CoreCommands;
        private Window m_Window;
        private GUIRenderer m_Renderer;
        private EditorSession m_Session;
    }
}
