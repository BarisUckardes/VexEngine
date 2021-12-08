using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using Vex.Input;

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
        public static bool IsMouseRightButtonClicked()
        {
            return ImGui.IsMouseClicked(ImGuiMouseButton.Right);
        }
        public static bool IsMouseLeftButtonClicked()
        {
            return ImGui.IsMouseClicked(ImGuiMouseButton.Left);
        }
        public static bool IsRightButtonReleased()
        {
            return ImGui.IsMouseReleased(ImGuiMouseButton.Right);
        }
        public static Vector2 GetMousePosition()
        {
            return ImGui.GetMousePos();
        }
        public static bool IsKeyDown(Keys key)
        {
            return ImGui.IsKeyDown((int)key);
        }
        public static bool IsCurrentItemHavored()
        {
            return ImGui.IsItemHovered();
        }
     
        public static bool IsAnyItemHavored()
        {
            return ImGui.IsAnyItemHovered();
        }
        public static bool IsWindowFocused()
        {
            return ImGui.IsWindowFocused();
        }
        public static bool IsWindowHovered()
        {
            return ImGui.IsWindowHovered();
        }
    }
}
