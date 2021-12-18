using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Vex.Types;

namespace Vex.Graphics
{
    public sealed class DeferredPointLight : Component
    {

        public float LightPower
        {
            get
            {
                return m_LightPower;
            }
            set
            {
                m_LightPower = value;
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
            OwnerEntity.World.GetView<WorldGraphicsView>().RegisterGraphicsObject(this);
        }
        protected override void OnDetach()
        {
            base.OnDetach();
            OwnerEntity.World.GetView<WorldGraphicsView>().RemoveGraphicsObject(this);
        }

        [ExposeThis]
        private float m_LightPower;
    }
}
