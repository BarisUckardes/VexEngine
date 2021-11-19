using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using Vex.Profiling;

namespace Bite.GUI
{
    public struct abc
    {
        public float A;
        public float B;
        public float C;
        public float D;
        public float E;

        public abc(float a, float b, float c, float d, float e)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
        }
    }
    [WindowLayout("Performance Observer")]
    public sealed class PerformanceObserverGUIWindow : WindowGUILayout
    {
        public override void OnInVisible()
        {

        }

        public override void OnLayoutBegin()
        {
            m_Values = new List<float>(180);
        }

        public override void OnLayoutFinalize()
        {

        }

        public override void OnRenderLayout()
        {
            ProfileTree rootTree = Profiler.GetLastSessionResult();
           

            if(ImGui.CollapsingHeader("Cpu"))
            {
                float ms = rootTree.SubTrees[0].ElapsedTime;
                m_Values.Add(ms);
                if (m_Values.Count > 180)
                    m_Values.RemoveAt(0);
                ImGui.PlotLines("", ref m_Values.ToArray()[0],m_Values.Count, 0, "", 0, 1,new Vector2(ImGui.GetWindowSize().X,128));
            }
            if(ImGui.CollapsingHeader("Performance Tree"))
            {
                RenderProfilerTree(rootTree);
            }
           
        }
        private void RenderProfilerTree(ProfileTree tree)
        {
            List<ProfileTree> subTrees = tree.SubTrees;
            if (ImGui.TreeNode(tree.Title))
            {
                ImGui.SameLine();
                ImGui.Text(tree.ElapsedTime.ToString() + $"[{tree.InvokeCount.ToString()}]");
 
                for(int subTreeIndex = 0;subTreeIndex < subTrees.Count;subTreeIndex++)
                {
                    RenderProfilerTree(subTrees[subTreeIndex]);
                }
                ImGui.TreePop();
            }
            else
            {
                ImGui.SameLine();
                ImGui.Text(tree.ElapsedTime.ToString() + $"[{tree.InvokeCount.ToString()}]");
            }
        }
        public override void OnVisible()
        {

        }

        private List<float> m_Values;
    }
}
