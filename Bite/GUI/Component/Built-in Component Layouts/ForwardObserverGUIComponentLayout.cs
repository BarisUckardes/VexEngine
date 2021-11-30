using Fang.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;
namespace Bite.GUI
{
    [ComponentLayout(typeof(ForwardMeshObserver))]
    public sealed class ForwardObserverGUIComponentLayout : ComponentLayout
    {
        public override void OnAttach()
        {
            m_TargetObserver = TargetComponent as ForwardMeshObserver;
        }

        public override void OnDetach()
        {
            m_TargetObserver = null;
        }

        public override void OnLayoutRender()
        {
            /*
             * Render clear color picker
             */
            Vector4 color = new Vector4(m_TargetObserver.ClearColor.R, m_TargetObserver.ClearColor.G, m_TargetObserver.ClearColor.B, m_TargetObserver.ClearColor.A);
            GUIRenderCommands.CreateColorPicker("forward_observer_clearColor", ref color);
            m_TargetObserver.ClearColor = new OpenTK.Mathematics.Color4(color.X, color.Y, color.Z, color.W);
        }
        private ForwardMeshObserver m_TargetObserver;
    }
}
