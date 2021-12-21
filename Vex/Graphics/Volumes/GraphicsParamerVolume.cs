using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Vex.Graphics;
namespace Vex.Graphics
{
    /// <summary>
    /// A volume component which creates a interface to the graphics resolver parameters and interpolates them
    /// </summary>
    public sealed class GraphicsParamerVolume : Component
    {

        /// <summary>
        /// Whether this parameter volume is a global volume or not
        /// </summary>
        public bool IsGlobal
        {
            get
            {
                return m_Global;
            }
            set
            {
                m_Global = true;
            }
        }

        public override bool ShouldTick
        {
            get
            {
                return false;
            }
        }

        protected override void OnAttach()
        {
            base.OnAttach();
        }
        protected override void OnDetach()
        {
            base.OnDetach();
        }

        private bool m_Global;
    }
}
