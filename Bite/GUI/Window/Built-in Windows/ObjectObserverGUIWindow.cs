﻿using Fang.Commands;
using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    public sealed class ObjectObserverGUIWindow : WindowGUILayout
    {
        public override void OnAttach()
        {
            /*
             * Register itself
             */
            GUIObject.RegisterListener(OnNewObject);
        }

        public override void OnDetach()
        {
            /*
             * Detach layout
             */
            m_Layout?.OnDetach();
            m_Layout = null;
        }

        public override void OnRenderLayout()
        {
            if(m_Layout == null)
            {
                GUIRenderCommands.CreateText($"This object has no defined layout to render", GetType().GUID.ToString());
            }
            else
            {
                RendeVexbjectLayout();
            }
        }
        
        private void RendeVexbjectLayout()
        {
            m_Layout.OnLayoutRender();
        }
        private void OnNewObject(EngineObject obj)
        {
            /*
             * Debug
             */
            Console.WriteLine("Received new signal");

            /*
             * Detach former layout
             */
            m_Layout?.OnDetach();
            m_Layout = null;

            /*
             * Try fetch object layout
             */
            m_Layout = GUIObject.FetchLayout(obj.GetType());

            /*
             * Validate layout null
             */
            m_Layout?.SetObject(obj);

            /*
             * Attach layout
             */
            m_Layout?.OnAttach();
        }

        private ObjectLayout m_Layout;
    }
}
