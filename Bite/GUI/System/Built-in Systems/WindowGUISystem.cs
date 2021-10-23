using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fang.Commands;
namespace Bite.GUI
{
  
    public sealed class WindowGUISystem : GUISystem
    {
        public WindowGUISystem()
        {
            m_Windows = new List<WindowGUILayout>();
            m_CreatePendingWindows = new List<WindowGUILayout>();
        }
        public override void OnAttach()
        {
            /*
             * Set pending window list reference to GUIWindow class
             */
            GUIWindow.SetListInternal(m_CreatePendingWindows);
        }

        public override void OnDetach()
        {
            /*
             * Detach all windows
             */
            foreach (WindowGUILayout layout in m_Windows)
                layout.OnDetach();

            m_Windows.Clear();
            m_Windows = null;
        }

        public override void OnUpdate()
        {
            /*
             * Create pending windows
             */
            for(int i=0;i< m_CreatePendingWindows.Count;i++)
            {
                m_CreatePendingWindows[i].OnAttach();
                m_Windows.Add(m_CreatePendingWindows[i]);
            }
            m_CreatePendingWindows.Clear();

            /*
             * Render window layouts
             */
            for(int i=0;i<m_Windows.Count;i++)
            {
                bool isOpen = true;
                if(GUIRenderCommands.CreateWindow(m_Windows[i].GetType().Name,i.ToString(),ref isOpen))
                {
                    m_Windows[i].OnRenderLayout();
                }

                /*
                 * Validate exit request
                 */
                if(!isOpen || m_Windows[i].HasDetachRequest)
                {
                    m_Windows[i].OnDetach();
                    m_Windows.RemoveAt(i);
                    i--;
                }
            }
        }

        private List<WindowGUILayout> m_CreatePendingWindows;
        private List<WindowGUILayout> m_Windows;
    }
}
