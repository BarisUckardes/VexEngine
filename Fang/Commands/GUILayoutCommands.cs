using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
namespace Fang.Commands
{
    public static class GUILayoutCommands
    {
        public static void StayOnSameLine()
        {
            ImGui.SameLine();
        }
        public static void Spacing()
        {
            ImGui.Spacing();
        }


    }
}
