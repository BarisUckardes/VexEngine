using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using System.Numerics;
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

        public static Vector2 GetCursor()
        {
            return ImGui.GetCursorPos();
        }
        public static Vector2 GetCursorScreenPos()
        {
            return ImGui.GetCursorScreenPos();
        }
        public static Vector2 GetMousePos()
        {
            return ImGui.GetMousePos();
        }
        public static void SetCursorPos(in Vector2 pos)
        {
            ImGui.SetCursorPos(pos);
            
        }
        public static float GetCurrentFontSize()
        {
            return ImGui.GetFontSize();
        }
        public static Vector2 GetCurrentWindowSize()
        {
            return ImGui.GetWindowSize();
        }
        public static Vector2 GetAvailableSpace()
        {
            return ImGui.GetContentRegionAvail();
        }
        public static void SetNextItemWidth(float width)
        {
            ImGui.SetNextItemWidth(width);
        }
        public static Vector2 GetTextSize(string text)
        {
            return ImGui.CalcTextSize(text);
        }
       
    }
}
