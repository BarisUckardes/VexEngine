using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.GUI;
using ImGuiNET;
using OpenTK.Mathematics;
using Vex.Graphics;

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
            ImGui.PushID(code);
            bool isVisible = ImGui.Begin(name, ref exRequest, (ImGuiWindowFlags)flags);
            ImGui.PopID();
            exitRequest = !exRequest;
            return isVisible;
        }
        public static bool CreateWindow(string name, string code, WindowCreateFlags flags = WindowCreateFlags.None)
        {
            ImGui.PushID(code);
            bool state = ImGui.Begin(name, (ImGuiWindowFlags)flags);
            ImGui.PopID();
            return state;
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
            ImGui.PushID(code);
            bool state = ImGui.BeginMenu(name);
            ImGui.PopID();
            return state;
        }
        public static void FinalizeMenu()
        {
            ImGui.EndMenu();
        }
        public static bool CreateMenuItem(string name,string code)
        {
            ImGui.PushID(code);
            bool state =  ImGui.MenuItem(name);
            ImGui.PopID();
            return state;
        }
       
        public static bool CreateButton(string name,string code)
        {
            ImGui.PushID(code);
            bool state = ImGui.Button(name);
            ImGui.PopID();
            return state;
        }

        public static bool CreateTreeNode(string name,string code)
        {
            ImGui.PushID(code);
            bool state = ImGui.TreeNode(name);
            ImGui.PopID();
            return state;
        }
        public static void FinalizeTreeNode()
        {
            ImGui.TreePop();
        }
        public static bool CreateCollapsingHeader(string name,string code)
        {
            ImGui.PushID(code);
            bool state =  ImGui.CollapsingHeader(name);
            ImGui.PopID();
            return state;
        }

        public static void CreateText(string text,string code)
        {
            ImGui.PushID(code);
            ImGui.Text(text);
            ImGui.PopID();
        }

        public static void CreateSeperatorLine()
        {
            ImGui.Separator();
        }

        public static bool CreateSelectableItem(string name,string code)
        {
            ImGui.PushID(code);
            bool state = ImGui.Selectable(name);
            ImGui.PopID();
            return state;
        }

        public static bool CreateVector3Slider(string name,string code,ref Vector3 vector,float min =0.00f,float max = 5.0f)
        {
            ImGui.PushID(code);
            System.Numerics.Vector3 intermediateVec = new System.Numerics.Vector3(vector.X, vector.Y, vector.Z);
            bool state = ImGui.SliderFloat3(name,ref intermediateVec,min,max);
            ImGui.PopID();
            vector.X = intermediateVec.X;
            vector.Y = intermediateVec.Y;
            vector.Z = intermediateVec.Z;
            return state;
        }
        public static bool CreateVector2Slider(string name, string code, ref Vector2 vector, float min = 0.00f, float max = 5.0f)
        {
            System.Numerics.Vector2 intermediateVec = new System.Numerics.Vector2(vector.X, vector.Y);
            ImGui.PushID(code);
            bool state = ImGui.SliderFloat2(name, ref intermediateVec, min, max);
            ImGui.PopID();
            vector.X = intermediateVec.X;
            vector.Y = intermediateVec.Y;
            return state;
        }
        public static bool CreateFloatSlider(string name,string code,ref float value,float min = 0.00f,float max = 5.0f)
        {
            ImGui.PushID(code);
            bool state = ImGui.SliderFloat(name,ref value, min, max);
            ImGui.PopID();
            return state;
        }
        public static bool CreateTextInput(string name,string code,ref string value,uint length = 20)
        {
            ImGui.PushID(code);
            bool state = ImGui.InputText(name, ref value,length);
            ImGui.PopID();
            return state;
        }
        public static bool CreateCheckbox(string name,string code,ref bool value)
        {
            ImGui.PushID(code);
            bool state = ImGui.Checkbox(name, ref value);
            ImGui.PopID();
            return state;
        }

        public static void CreateImage(Texture2D texture)
        {
            ImGui.Image((IntPtr)texture.Handle,new System.Numerics.Vector2(128,128));
        }
     
    }
}
