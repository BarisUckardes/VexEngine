using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
using ImGuiNET;

namespace Slope.Editor
{
    internal sealed class EditorGUILayout
    {
        public void Initialize()
        {

        }
        public void Finalize()
        {

        }
        bool open = false;
        public void Render()
        {
            /*
             * Create main window strecteh to background
             */
            ImGuiWindowFlags flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;
            ImGui.SetNextWindowPos(ImGui.GetMainViewport().Pos);
            ImGui.SetNextWindowSize(ImGui.GetMainViewport().Size);
            ImGui.SetNextWindowViewport(ImGui.GetMainViewport().ID);

            /*
             * Create background window
             */
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(0, 0));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            ImGui.Begin("Main Window",ref open,flags);
            ImGui.PopStyleVar();
            ImGui.End();


            /*
             * Create project editor view
             */
        }
    }
}
