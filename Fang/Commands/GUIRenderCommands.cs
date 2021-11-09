using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.GUI;
using ImGuiNET;
using OpenTK.Mathematics;

namespace Fang.Commands
{
    public static class GUIRenderCommands
    {
        public static void CreateDemoWindow()
        {
            ImGui.ShowDemoWindow();
        }
        public static bool CreateWindow(string name,string code,ref bool exitRequest,WindowCreateFlags flags = WindowCreateFlags.None)
        {
            bool exRequest = true;
            bool isVisible = ImGui.Begin(CreateNameCode(name, code), ref exRequest, (ImGuiWindowFlags)flags);
            exitRequest = !exRequest;
            return isVisible;
        }
        public static bool CreateWindow(string name, string code, WindowCreateFlags flags = WindowCreateFlags.None)
        {
            return ImGui.Begin(CreateNameCode(name, code), (ImGuiWindowFlags)flags);
        }

        public static void FinalizeWindow()
        {
            ImGui.End();
        }

        public static bool CreateMainMenuBar()
        {
            return ImGui.BeginMainMenuBar();
        }
        public static void FinalizeMainMenuBar()
        {
            ImGui.EndMainMenuBar();
        }

        public static bool CreateMenu(string name,string code)
        {
            return ImGui.BeginMenu(CreateNameCode(name,code));
        }
        public static void FinalizeMenu()
        {
            ImGui.EndMenu();
        }
        public static bool CreateMenuItem(string name,string code)
        {
            return ImGui.MenuItem(CreateNameCode(name, code));
        }
       
        public static bool CreateButton(string name,string code)
        {
            return ImGui.Button(CreateNameCode(name, code));
        }

        public static bool CreateTreeNode(string name,string code)
        {
            return ImGui.TreeNode(CreateNameCode(name, code));
        }
        public static void FinalizeTreeNode()
        {
            ImGui.TreePop();
        }
        public static bool CreateCollapsingHeader(string name,string code)
        {
            return ImGui.CollapsingHeader(CreateNameCode(name, code));
        }

        public static void CreateText(string text,string code)
        {
            ImGui.Text(CreateNameCode(text, code));
        }

        public static void CreateSeperatorLine()
        {
            ImGui.Separator();
        }

        public static bool CreateSelectableItem(string name,string code)
        {
            return ImGui.Selectable(CreateNameCode(name, code));
        }

        public static bool CreateVector3Slider(string name,string code,ref Vector3 vector,float min =0.00f,float max = 5.0f)
        {
            System.Numerics.Vector3 intermediateVec = new System.Numerics.Vector3(vector.X, vector.Y, vector.Z);
            bool state = ImGui.SliderFloat3(CreateNameCode(name, code),ref intermediateVec,min,max);

            vector.X = intermediateVec.X;
            vector.Y = intermediateVec.Y;
            vector.Z = intermediateVec.Z;
            return state;
        }
        public static bool CreateVector2Slider(string name, string code, ref Vector2 vector, float min = 0.00f, float max = 5.0f)
        {
            System.Numerics.Vector2 intermediateVec = new System.Numerics.Vector2(vector.X, vector.Y);
            bool state = ImGui.SliderFloat2(CreateNameCode(name, code), ref intermediateVec, min, max);

            vector.X = intermediateVec.X;
            vector.Y = intermediateVec.Y;
            return state;
        }
        public static bool CreateFloatSlider(string name,string code,ref float value,float min = 0.00f,float max = 5.0f)
        {
            bool state = ImGui.SliderFloat(CreateNameCode(name, code),ref value, min, max);

            return state;
        }
        public static bool CreateTextInput(string name,string code,ref string value,uint length = 20)
        {
            bool state = ImGui.InputText(CreateNameCode(name, code), ref value,length);
            return state;
        }
        public static bool CreateCheckbox(string name,string code,ref bool value)
        {
            bool state = ImGui.Checkbox(CreateNameCode(name, code), ref value);
            return state;
        }
        private static string CreateNameCode(string name,string code)
        {
            return name + "##" + code;
        }
    }
}
