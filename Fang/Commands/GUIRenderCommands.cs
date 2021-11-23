using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Fang.GUI;
using ImGuiNET;
using Vex.Graphics;
using Vex.Extensions;
using System.Runtime.InteropServices;
using Vex.Framework;

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
        public static void CreateEmptySpace()
        {
            ImGui.Spacing();
        }

        public static bool CreateSelectableItem(string name,string code)
        {
            bool s = false;
            ImGui.PushID(code);
            bool state = ImGui.Selectable(name,ref s,ImGuiSelectableFlags.DontClosePopups);
            ImGui.PopID();
            return state;
        }
        public static bool CreateSelectableItem(string name, string code,in Vector2 size)
        {
            bool s = false;
            ImGui.PushID(code);
            bool state = ImGui.Selectable(name, ref s, ImGuiSelectableFlags.DontClosePopups,size);
            ImGui.PopID();
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
        public static bool CreateVector3Slider(string name, string code, ref Vector3 vector, float min = 0.00f, float max = 5.0f)
        {
            System.Numerics.Vector3 intermediateVec = new System.Numerics.Vector3(vector.X, vector.Y,vector.Z);
            ImGui.PushID(code);
            bool state = ImGui.SliderFloat3(name, ref intermediateVec, min, max);
            ImGui.PopID();
            vector.X = intermediateVec.X;
            vector.Y = intermediateVec.Y;
            vector.Z = intermediateVec.Z;
            return state;
        }
        public static bool CreateVector4Slider(string name, string code, ref Vector4 vector, float min = 0.00f, float max = 5.0f)
        {
            System.Numerics.Vector4 intermediateVec = new System.Numerics.Vector4(vector.X, vector.Y, vector.Z,vector.W);
            ImGui.PushID(code);
            bool state = ImGui.SliderFloat4(name, ref intermediateVec, min, max);
            ImGui.PopID();
            vector.X = intermediateVec.X;
            vector.Y = intermediateVec.Y;
            vector.Z = intermediateVec.Z;
            vector.W = intermediateVec.W;
            return state;
        }
        public static bool CreateFloatSlider(string name,string code,ref float value,float min = 0.00f,float max = 5.0f)
        {
            ImGui.PushID(code);
            bool state = ImGui.SliderFloat(name,ref value, min, max);
            ImGui.PopID();
            return state;
        }
        public static bool CreateIntInput(string name,string code,ref int value)
        {
            ImGui.PushID(code);
            bool state =ImGui.InputInt(name, ref value);
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
        public static bool CreateMultilineTextInput(string name,string code,ref string value,in Vector2 size,ImGuiInputTextFlags flags = ImGuiInputTextFlags.None,uint length = 1500)
        {

            ImGui.PushID(code);
            bool state = ImGui.InputTextMultiline("", ref value, length, size,flags);
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

        public static void CreateImage(Texture2D texture,string code, in Vector2 size)
        {
            ImGui.PushID(code);
            ImGui.Image((IntPtr)texture.Handle, size);
            ImGui.PopID();
        }
        public static void CreateImage(Texture texture, in Vector2 size)
        {
            ImGui.Image((IntPtr)texture.Handle, size);
        }
        public static void CreateImage(Texture texture, in Vector2 size,in Vector2 uv0,in Vector2 uv1)
        {
            ImGui.Image((IntPtr)texture.Handle, size,uv0,uv1);
        }
        public static void CreateMainDockspace()
        {
            ImGui.DockSpace(0,new Vector2(0,0),ImGuiDockNodeFlags.None | ImGuiDockNodeFlags.NoResize | ImGuiDockNodeFlags.PassthruCentralNode);
        }

        public static void SignalPopupCreate(string popupID)
        {
            ImGui.OpenPopup(popupID);
        }
        public static bool CreatePopup(string popupID)
        {
            return ImGui.BeginPopup(popupID);
        }
        public static void FinalizePopup()
        {
            ImGui.EndPopup();
        }
        public static void TerminateCurrentPopup()
        {
            ImGui.CloseCurrentPopup();
        }
        public static bool CreateCombo(string name,string preview,string code)
        {
            bool state = false;
            ImGui.PushID(code);
            state = ImGui.BeginCombo(name, preview);
            ImGui.PopID();
            return state;
        }
        public static void FinalizeCombo()
        {
            
            ImGui.EndCombo();
        }
        public static void EnableStyleColor(ImGuiCol target,in Vector4 color)
        {
            ImGui.PushStyleColor(target,color);
        }
        public static void DisableStyleColor()
        {
            ImGui.PopStyleColor();
        }
        public static void EnableStyle(ImGuiStyleVar target,float value)
        {
            ImGui.PushStyleVar(target,value);
        }
        public static void DisableStyle()
        {
            ImGui.PopStyleVar();
        }
       
        private static VexObject s_LastVexObjectObjectField;
        public static VexObject CreateObjectField(VexObject targetObject, string code)
        {
            /*
             * Render a selectable
            */
            GUIRenderCommands.EnableStyleColor(ImGuiCol.Text, new Vector4(1.0f, 1.0f, 102.0f/255.0f, 1.0f));
            GUIRenderCommands.CreateSelectableItem(targetObject == null ? "Empty Object" : $"{targetObject.Name}({targetObject.GetType().Name})", code);
            GUIRenderCommands.DisableStyleColor();

            /*
             * Start drag drop source
             */
            if (ImGui.BeginDragDropSource()) // dragged from here
            {
                s_LastVexObjectObjectField = targetObject;
                ImGui.SetDragDropPayload("Object Field", IntPtr.Zero, 0);
                ImGui.EndDragDropSource();
            }


            /*
             * Draw drag drop
             */
            if (ImGui.BeginDragDropTarget()) // dropped here
            {
                /*
                 * Get payload ptr
                 */
                ImGuiPayloadPtr ptr = ImGui.AcceptDragDropPayload("Object Field");

                /*
                 * Validate object
                 */
                ImGui.EndDragDropTarget();
            }
            
            /*
             * Receive drag drop 
             */
            if (ImGui.BeginDragDropTarget() && ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem) && ImGui.IsMouseReleased(ImGuiMouseButton.Left)) // dropped here
            {
                /*
                 * Get payload ptr
                 */
                ImGuiPayloadPtr ptr = ImGui.AcceptDragDropPayload("Object Field");

                /*
                 * Validate object
                 */
                ImGui.EndDragDropTarget();
                Console.WriteLine("Drag accepted by code " + code);

                return s_LastVexObjectObjectField;
            }

            return targetObject;
        }

        public static VexObject CreateObjectField(VexObject targetObject, string code,in Vector2 size,bool showName = false)
        {
            /*
            * Render a selectable
            */
            GUIRenderCommands.CreateSelectableItem(showName == true ? targetObject == null ? "Empty Object" : $"{targetObject.Name}({targetObject.GetType().Name})" : "", code,size);

            /*
             * Start drag drop source
             */
            if (ImGui.BeginDragDropSource()) // dragged from here
            {
                s_LastVexObjectObjectField = targetObject;
                ImGui.SetDragDropPayload("Object Field", IntPtr.Zero, 0);
                ImGui.EndDragDropSource();
            }


            /*
             * Draw drag drop
             */
            if (ImGui.BeginDragDropTarget()) // dropped here
            {
                /*
                 * Get payload ptr
                 */
                ImGuiPayloadPtr ptr = ImGui.AcceptDragDropPayload("Object Field");

                /*
                 * Validate object
                 */
                ImGui.EndDragDropTarget();
            }

            /*
             * Receive drag drop 
             */
            if (ImGui.BeginDragDropTarget() && ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem) && ImGui.IsMouseReleased(ImGuiMouseButton.Left)) // dropped here
            {
                /*
                 * Get payload ptr
                 */
                ImGuiPayloadPtr ptr = ImGui.AcceptDragDropPayload("Object Field");

                /*
                 * Validate object
                 */
                ImGui.EndDragDropTarget();
                Console.WriteLine("Drag accepted by code " + code);

                return s_LastVexObjectObjectField;
            }

            return targetObject;
        }

        public static GUIOpenFileDialogHandle CreateOpenFileDialog(List<string> targetExtensions,string buttonName,Texture2D folderTexture,Texture2D fileTexture,bool showFolders = true)
        {
            return new GUIOpenFileDialogHandle(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),buttonName, targetExtensions, showFolders,folderTexture,fileTexture);
        }
        public static void CloseOpenFileDialog(GUIOpenFileDialogHandle handle)
        {
            handle.CloseHandle();
        }


    }
}
