using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
namespace Fang.Commands
{
    public static class GUIEventCommands
    {
        public static bool IsCurrentItemClicked()
        {
            return ImGui.IsItemClicked(ImGuiMouseButton.Left);
        }
        public static bool IsCurrentItemDoubleClicked()
        {
            return ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left);
        }
        public static bool IsCurrentItemHavored()
        {
            return ImGui.IsItemHovered();
        }
    }
}
